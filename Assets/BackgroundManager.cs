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
        float scale = ((float)Screen.height / (float)Screen.width) / (16f / 9f);
        mainmenu.transform.localScale = new Vector3(mainmenu.transform.localScale.x/scale, mainmenu.transform.localScale.y, 0);
        if (instance != null) return;
        instance = this;
    }
}
