using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMapFrameUI : MonoBehaviour
{
    internal SelectMapUI SelectMapUI;

    public SpriteRenderer mapImage_SpriteRenderer;
    public SpriteRenderer lockedMap_SpriteRenderer;
    public UILabel index;
    public UISprite isCom;



    public int indexOfMap;
    public string spriteName;
    public bool isCompleted;
    public bool isUnlock;

    private void OnEnable()
    {
        //UpdateStateOfMap();
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

        //mapImage.spriteName = spriteName + "_1";

        Debug.Log(Game.instance.images.Length);

        mapImage_SpriteRenderer.sprite = Game.instance.images[indexOfMap - 1].Image;
        index.text = indexOfMap.ToString();
        isCom.enabled = isCompleted ? true : false;
    }

    public void OnSelectThis()
    {
      //  if (!isUnlock) return;
        Game.instance.currentMap = indexOfMap;
        SelectMapUI.LoadMap();
    }


}
