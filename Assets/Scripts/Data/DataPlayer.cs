using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    public static bool IsPlayTutorial
    {
        set
        {
            PlayerPrefs.SetInt("PlayTutorial", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt("PlayTutorial", 0) == 1;
        }
    }
    public static int CurrentPlayingMap
    {
        set
        {
            PlayerPrefs.SetInt("PlayingMap", value);
        }

        get
        {
            return PlayerPrefs.GetInt("PlayingMap", 1);
        }
    }
    public static void CompletedMap(int indexOfMap)
    {
        PlayerPrefs.SetInt("IsCompletedMap" + indexOfMap.ToString(), 1);
    }

    public static bool IsCompletedMap(int indexOfMap)
    {
        return PlayerPrefs.GetInt("IsCompletedMap" + indexOfMap.ToString(), 0)==1;
    }


    public static int CompletedMapCount
    {
        set
        {
            PlayerPrefs.SetInt("CompletedMapCount", value);
        }
        get
        {
            return PlayerPrefs.GetInt("CompletedMapCount", 0);
        }
    }


    public static string GetTextOfCompletedMapCount()
    {
        return CompletedMapCount.ToString() + "/" + NumberOfMap.ToString();
    }

    public static int NumberOfMap
    {
        get
        {
            return PlayerPrefs.GetInt("NumberOfMap", 30);
        }
    }

    public static bool Sound
    {
        set
        {
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }

    }

    public static bool Fx
    {
        set
        {
            PlayerPrefs.SetInt("Fx", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt("Fx", 1) == 1;
        }

    }

    public static bool ShowAnswer
    {
        set
        {
            PlayerPrefs.SetInt("ShowAnswer", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt("ShowAnswer", 0) == 1;
        }

    }


    public static int Get10to9(int input)
    {
        if ( input %10 !=0)
        {
            return 9 * (input / 10) + input % 10;
        }
        else
        {
            return input / 10;
        }

    }



    public static bool ShowedSpecialLevel(int level)
    {
        PlayerPrefs.SetInt("ShowedLevel" + Get10to9(level), 1);

        return false;
    }

    public static bool IsShowedSpecialLevel(int level)
    {


        return PlayerPrefs.GetInt("ShowedLevel" + Get10to9(level), 0) == 1;
    }


    public static bool IsWatchAdsSpecialLevel(int level)
    {


        return false;
    }
}
