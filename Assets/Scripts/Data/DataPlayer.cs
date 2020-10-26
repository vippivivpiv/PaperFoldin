using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    
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
            return PlayerPrefs.GetInt("NumberOfMap", 120);
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




}
