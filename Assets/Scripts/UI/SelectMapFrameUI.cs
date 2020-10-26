using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMapFrameUI : MonoBehaviour
{
    public SelectMapUI SelectMapUI;
    public UISprite mapImage;
    public UILabel index;
    public UISprite isCom;



    public int indexOfMap;
    public string spriteName;
    public bool isCompleted;
    public bool isUnlock;

    private void OnEnable()
    {
        UpdateStateOfMap();
    }

    public void UpdateStateOfMap()
    {


        if (indexOfMap <= DataPlayer.CurrentPlayingMap)
        {
            isUnlock = true;
            spriteName = "Map" + (indexOfMap).ToString();
        }
        else
        {
            isUnlock = false;
            spriteName = "Locked";
        }
        isCompleted = DataPlayer.IsCompletedMap(indexOfMap);

        mapImage.spriteName = spriteName;
        index.text = indexOfMap.ToString();
        isCom.enabled = isCompleted ? true : false;
    }

    public void OnSelectThis()
    {
        if (!isUnlock) return;
        Game.instance.currentMap = indexOfMap;
        SelectMapUI.LoadMap();
    }


}
