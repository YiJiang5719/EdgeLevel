using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set ; get ; }
    public SaveState state;

    private void Awake() 
    {
        //ResetSave();
        
        DontDestroyOnLoad(gameObject);
        Instance = this ;
        Load();

        // are we using the accelerometer AND can we use it 
        if ( state.usingAccelerometer && !SystemInfo.supportsAccelerometer)
        {
            // if we cant , make sure we're not trying next time
            state.usingAccelerometer = false;
            Save();
            
        }
    }

    //save the whole state of this savestate script to the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save",  Helper.Serialize<SaveState>(state));
    }

    //Load the previous saved state from the player prefs
    public void Load()
    {
       if (PlayerPrefs.HasKey("save")) 
       {
           state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
       }
       else
       {
           state = new SaveState();
           Save();
           Debug.Log("No save file found, creating a new one");
       }
    }

    // check if the color is owned 
    public bool IsColorOwned(int index)
    {
        // check if the bit is set , if so , the color is owned
        return ( state.colorOwned & (1 << index)) != 0;
    }

        // check if the color is owned 
    public bool IsTrailOwned(int index)
    {
        // check if the bit is set , if so , the color is owned
        return ( state.trailOwned & (1 << index)) != 0;
    }

    //Attempt buying a color , return true / false
    public bool BuyColor(int index, int cost)
    {
        if (state.gold >= cost)
        {
            //enough money, remove from the current gold stack
            state.gold -= cost;
            UnlockColor(index);

            //save progress
            Save();
            
            return true;

        }
        else
        {
            //not enough money
            return false;

        }
    }

    //Attempt buying a trail, return true / false
    public bool BuyTrail(int index, int cost)
    {
        if (state.gold >= cost)
        {
            //enough money, remove from the current gold stack
            state.gold -= cost;
            UnlockTrail(index);

            //save progress
            Save();
            
            return true;

        }
        else
        {
            //not enough money
            return false;

        }
    }

    // unlock a color in the "colorOwned" int 
    public void UnlockColor(int index)
    {
        // toggle on the bit at index
        state.colorOwned  |= 1 << index ;
    }

    // unlock a color in the "trailOwned" int 
    public void UnlockTrail(int index)
    {
        // toggle on the bit at index
        state.trailOwned  |= 1 << index ;
    }

    //complete the level
    public void CompleteLevel(int index)
    {
        // if this is the current active level
        if (state.completeLevel == index)
        {
            state.completeLevel ++;
            Save();
        }
    }

    // Reset the whole save file
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
