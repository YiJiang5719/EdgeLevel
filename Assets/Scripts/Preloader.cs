using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float loadTime;
    private float minimunLogoTime = 3.0f; // minimun time of the scene

    private void Start() 
    {
        //grab the only canvasgroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();
        

        //start with a white screen
        fadeGroup.alpha = 1;

        //preload the game
        // $$

        // get a timestamp of the completion time
        // if loadtime is super , give it a small buffer time so we cane apreciate the logo
        if (Time.time < minimunLogoTime)
            loadTime = minimunLogoTime;
        else
            loadTime = Time.time;   
    }

    private void Update() 
    {
        // Fade-in
        if (Time.time < minimunLogoTime)
        {
            fadeGroup.alpha = 1 - Time.time;
        }

        // Fade-out
        if (Time.time > minimunLogoTime && loadTime != 0)
        {
            fadeGroup.alpha = Time.time - minimunLogoTime;
            if (fadeGroup.alpha >= 1)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
