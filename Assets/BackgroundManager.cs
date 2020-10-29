using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;
    public GameObject mainmenu;
    public GameObject selectMap;
    public GameObject Ingame;
    private void Start()
    {
        if ( instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
}
