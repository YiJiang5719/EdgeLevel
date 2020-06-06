using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    public RectTransform menuContainer;
    public Transform levelPanel;
    public Transform colorPanel;
    public Transform trailPanel;

    public Text colorBuySetText;
    public Text trailBuySetText;
    public Text goldText;

    private MenuCamera menuCam;

    private int [] colorCost = new int[] {0,5,5,5,5,10,10,10};
    private int [] trailCost = new int[] {0,20,40,40,60,60,80,100};
    private int selectedColorIndex;
    private int selectedTrailIndex;
    private int activeColorIndex;
    private int activeTrailIndex;

    private Vector3 desiredMenuPosition;

    private GameObject currentTrail; 

    public AnimationCurve enteringLevelZoomCurve;
    private bool isEnteringLevel = false;
    private float zoomDuration = 3.0f;
    private float zoomTransition;

    private Texture previousTrail;
    private GameObject lastPreviewObject;
    public Transform trailPreviewObject;
    public RenderTexture trailPreviewTexture;

    private void Start() 
    {
        // $$ TEMPORARY  
        SaveManager.Instance.state.gold = 999;

        // tell our gold text how much shoud be display
        UpdateGoldText();

        // find the only menuCam and assign it
        menuCam = FindObjectOfType<MenuCamera>();


        //position our camera to the foucus menu
        SetCameraTo(Manager.Instance.menuFocus);

         //grab the only canvasgroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();
        

        //start with a white screen
        fadeGroup.alpha = 1;

        //add button on-click events to shop buttons
        InitShop();

        //add button on-click events to levels
        InitLevel();

        //set player's preferences (color / trail)
        OnColorSelect(SaveManager.Instance.state.activeColor);
        SetColor(SaveManager.Instance.state.activeColor);
       
        OnTrailSelect(SaveManager.Instance.state.activeTrail);
        SetTrail(SaveManager.Instance.state.activeTrail);

        // make the button bigger for the selected items
        colorPanel.GetChild(SaveManager.Instance.state.activeColor).GetComponent<RectTransform>().localScale = Vector3.one * 1.15f ;
        trailPanel.GetChild(SaveManager.Instance.state.activeTrail).GetComponent<RectTransform>().localScale = Vector3.one * 1.15f ;
    
        // create the trail preview
        lastPreviewObject = GameObject.Instantiate(Manager.Instance.playerTrails[SaveManager.Instance.state.activeTrail]) as GameObject; 
        lastPreviewObject.transform.SetParent(trailPreviewObject);
        lastPreviewObject.transform.localPosition = Vector3.zero;
    }

    private void Update() 
    {
        //fade-in
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        // menu navigation ( smooth )
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 0.1f);

        //Entering level zoom
        if (isEnteringLevel)
        {
            //add to the zoomtransition float
            zoomTransition += (1/zoomDuration) * Time.deltaTime;

            //change the scale, following the animation curve
            menuContainer.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5, enteringLevelZoomCurve.Evaluate(zoomTransition));
        
            //change the desired position of the canvas, so it can follow the scale basically
            // this zooms in the center
            Vector3 newDesiredPosition = desiredMenuPosition * 5;

            // This adds to the specific position of the level on the canvas
            RectTransform rt = levelPanel.GetChild(Manager.Instance.currentLevel).GetComponent<RectTransform>();
            newDesiredPosition -= rt.anchoredPosition3D * 5;

            // This line will override the previous update
            menuContainer.anchoredPosition3D = Vector3.Lerp(desiredMenuPosition, newDesiredPosition, enteringLevelZoomCurve.Evaluate(zoomTransition));
            
            // fade to the white screen, this will override the first line of the update
            fadeGroup.alpha = zoomTransition;

            // are we done with the animation
            if (zoomTransition >= 1)
            {
                //enter the level
                SceneManager.LoadScene("GameCutScene");
            }
        }     
    }


    private void InitShop()
    {
        //Just make sure we've assigned the references
        if (colorPanel == null || trailPanel == null)
            Debug.Log("You did not assign the color / trail panel in the inspector ");

        //For evert children transform under our color panel, find the button and add onclick
        int i = 0;
        foreach (Transform t in colorPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(()=> OnColorSelect(currentIndex));

            // set color of the image, based on if owned or not
            Image img = t.GetComponent<Image>();
            img.color = SaveManager.Instance.IsColorOwned(i) 
                ? Manager.Instance.playerColors[currentIndex]
                : Color.Lerp(Manager.Instance.playerColors[currentIndex], new Color(0,0,0,1),0.25f);

            
            i++;
        }

        //Reset the index;
        i = 0;
        //Do the same for the trail Panel
        foreach (Transform t in trailPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(()=> OnTrailSelect(currentIndex));

            // set color of the image, based on if owned or not
            RawImage img = t.GetComponent<RawImage>();
            img.color = SaveManager.Instance.IsTrailOwned(i) 
                ? Color.white 
                : new Color(0.7f,0.7f,0.7f);


            i++;
        }

        //set the previous trail to prevent bug when swaping later;
        previousTrail = trailPanel.GetChild(SaveManager.Instance.state.activeTrail).GetComponent<RawImage>().texture;
    }
    private void InitLevel()
    {
        //Just make sure we've assigned the references
        if (levelPanel == null)
            Debug.Log("You did not assign the level panel in the inspector ");

        //For evert children transform under our level panel, find the button and add onclick
        int i = 0;
        foreach (Transform t in levelPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(()=> OnLevelSelect(currentIndex));

            Image img = t.GetComponent<Image>();

            //Is it unlocked?
            if ( i <= SaveManager.Instance.state.completeLevel)
            {
                // It is unlocked!
                if (i == SaveManager.Instance.state.completeLevel)
                {
                    //its not completed
                    img.color = Color.white;
                }
                else
                {
                    // Level is completed
                    img.color = Color.green;
                }
            }
            else
            {
                //Level isn't unlock , disabled the button
                b.interactable = false;

                // sete to a dark color
                img.color = Color.grey;
            }
            i++;
        }
    }

    private void SetCameraTo(int menuIndex)
    {
        NavigateTo(menuIndex);
        menuContainer.anchoredPosition3D = desiredMenuPosition;
        
    }

    private void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            // 0 && default case = main menu
            default:
            case 0:
                desiredMenuPosition = Vector3.zero;
                menuCam.BackToMainMenu();
                break;
            // 1 = play menu
            case 1 :
                desiredMenuPosition = Vector3.right * 1792;
                menuCam.MoveToLevel();
                break;
            // 2 = shop menu
            case 2 :
                desiredMenuPosition = Vector3.left * 1792;
                menuCam.MoveToShop();
                break;
        }
    }

    private void SetColor(int index)
    {
        //Set the active index
        activeColorIndex = index;
        SaveManager.Instance.state.activeColor = index;

        // Change the color on the player model
        Manager.Instance.playerMaterial.color = Manager.Instance.playerColors[index];

        // Change buy / set button text
        colorBuySetText.text = "current";

        //remember preferences
        SaveManager.Instance.Save();


    }

    private void SetTrail(int index)
    {
        // set the active index
        activeTrailIndex = index;
        SaveManager.Instance.state.activeTrail = index;

        // Change the trail on the player model

        if (currentTrail != null)
            Destroy(currentTrail);

        // create the new trail
        currentTrail = Instantiate(Manager.Instance.playerTrails[index]) as GameObject;

        //set it as a children of the player
        currentTrail.transform.SetParent(FindObjectOfType<MenuPlayer>().transform);

        //fix the wired scaling issues / rotation issues
        currentTrail.transform.localPosition = Vector3.zero;
        currentTrail.transform.localRotation = Quaternion.Euler(0,0,90);
        currentTrail.transform.localScale = Vector3.one * 0.01f;

        // Change buy / set button text
        trailBuySetText.text = "current";

        // remember preferences
        SaveManager.Instance.Save();
    }

    private void UpdateGoldText()
    {
        goldText.text = SaveManager.Instance.state.gold.ToString();
    }

    //Buttons
    public void OnClickedPlay()
    {
        NavigateTo(1);
        //Debug.Log("Play Button has been clicked");
    }

    public void OnClickedMemory()
    {
        NavigateTo(2);
        //Debug.Log("Memory Button has been clicked");
    }

    public void OnClickedBack()
    {
        NavigateTo(0);
        Debug.Log("Back Button has been clicked");
    }
    
    private void OnColorSelect(int currentIndex)
    {
        Debug.Log("Selecting color button:" + currentIndex);

        // if the button clicked is already selected, exit
        if (selectedColorIndex == currentIndex)
            return;

        //make the icon bigger
        colorPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        //puy the previous one on normal scale
        colorPanel.GetChild(selectedColorIndex).GetComponent<RectTransform>().localScale = Vector3.one;
       
        // set the selected color
        selectedColorIndex = currentIndex;

        //change the content of the buy / set button ,depending on the state of the color
        if (SaveManager.Instance.IsColorOwned(currentIndex))
        {
            //color is owned
            // is it already our current color
            if (activeColorIndex == currentIndex)
            {
                colorBuySetText.text = "current";
            }
            else
            {
                colorBuySetText.text = "Select";
            }
        }
        else
        {
            //color isnt owned
            colorBuySetText.text = "Buy: " + colorCost[currentIndex].ToString();
        }


    }

    private void OnTrailSelect(int currentIndex)
    {
        Debug.Log("Selecting trail button:" + currentIndex);

        // if the button clicked is already selected, exit
        if (selectedTrailIndex == currentIndex)
            return;


        //preview trail
        //get the image of the preview button
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RawImage>().texture = previousTrail;
        //keep the new trail's preview image in the previous trail
        previousTrail = trailPanel.GetChild(currentIndex).GetComponent<RawImage>().texture;
        //set the new trail preview image to the other camera
        trailPanel.GetChild(currentIndex).GetComponent<RawImage>().texture = trailPreviewTexture;

        //change the physical object of the trail preview
        if (lastPreviewObject != null)
            Destroy(lastPreviewObject);
        lastPreviewObject = GameObject.Instantiate(Manager.Instance.playerTrails[currentIndex]) as GameObject; 
        lastPreviewObject.transform.SetParent(trailPreviewObject);
        lastPreviewObject.transform.localPosition = Vector3.zero;

        //make the icon bigger
        trailPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        //puy the previous one on normal scale
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RectTransform>().localScale = Vector3.one;
       

        // set the selected trail
        selectedTrailIndex = currentIndex;

        //change the content of the buy / set button ,depending on the state of the trail
        if (SaveManager.Instance.IsTrailOwned(currentIndex))
        {
            // trail is owned
            // is it already our current color
            if (activeTrailIndex == currentIndex)
            {
                trailBuySetText.text = "current";
            }
            else
            {
                trailBuySetText.text = "Select";
            }
        }
        else
        {
            //trail isnt owned
            trailBuySetText.text = "Buy: " +  trailCost[currentIndex].ToString();
        }

    }

    private void OnLevelSelect(int currentIndex)
    {
        Manager.Instance.currentLevel = currentIndex;
        //SceneManager.LoadScene("Game");
        isEnteringLevel = true;
        Debug.Log("Selecting level button:" + currentIndex);
    }

    public void OnColorBuySet()
    {
        Debug.Log("Buy / Set color");

        // is the selected color owned 
        if (SaveManager.Instance.IsColorOwned(selectedColorIndex))
        {
            //set the color!
            SetColor(selectedColorIndex);
        }
        else
        {
            //attempt to buy the color
            if (SaveManager.Instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                // success!
                SetColor(selectedColorIndex);

                //change the color of the button
                colorPanel.GetChild(selectedColorIndex).GetComponent<Image>().color = Manager.Instance.playerColors[selectedColorIndex];

            

                //update gold text
                UpdateGoldText();
            }
            else
            {
                //do not have enough gold !
                // playe sound feedback;

                Debug.Log("not enough gold");
            }

        }
    }

    public void OnTrailBuySet()
    {
        Debug.Log("Buy / Set Trail");

        // is the selected trail owned 
        if (SaveManager.Instance.IsTrailOwned(selectedTrailIndex))
        {
            //set the color!
            SetTrail(selectedTrailIndex);
        }
        else
        {
            //attempt to buy the color
            if (SaveManager.Instance.BuyTrail(selectedTrailIndex, trailCost[selectedTrailIndex]))
            {
                // success!
                SetTrail(selectedTrailIndex);

                //change the color of the button
                trailPanel.GetChild(selectedTrailIndex).GetComponent<RawImage>().color = Color.Lerp(Manager.Instance.playerColors[selectedColorIndex], new Color(0,0,0,1),0.25f);

                //update gold text
                UpdateGoldText();

            }
            else
            {
                //do not have enough gold !
                // playe sound feedback;

                Debug.Log("not enough gold");
            }
        }
    }
}
