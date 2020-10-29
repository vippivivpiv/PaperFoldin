using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMapFrameUI : MonoBehaviour
{
    public SelectMapUI SelectMapUI;
    public InGameUI InGameUI;


    public SpriteRenderer mapImage_SpriteRenderer;
    public SpriteRenderer lockedMap_SpriteRenderer;

    public UI2DSprite UI2DSprite_MapImage;
    public UI2DSprite UI2DSprite_LockedImage;



    public UILabel index;
    public UISprite isCom;



    public int indexOfMap;
    public string spriteName;

    public string nameOfMap;

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

            //  mapImage_SpriteRenderer.gameObject.SetActive(true);
            // lockedMap_SpriteRenderer.gameObject.SetActive(false);
            UI2DSprite_MapImage.gameObject.SetActive(true);
            UI2DSprite_LockedImage.gameObject.SetActive(false);


        }
        else
        {
            isUnlock = false;
            spriteName = "Locked";

            //  mapImage_SpriteRenderer.gameObject.SetActive(false);
            // lockedMap_SpriteRenderer.gameObject.SetActive(true);

            UI2DSprite_MapImage.gameObject.SetActive(false);
            UI2DSprite_LockedImage.gameObject.SetActive(true);
        }
        isCompleted = DataPlayer.IsCompletedMap(indexOfMap);

        UI2DSprite_MapImage.sprite2D = Game.instance.images[indexOfMap - 1].Image;

        //  mapImage_SpriteRenderer.sprite = Game.instance.images[indexOfMap - 1].Image;
      //  nameOfMap = DataPlayer.Get10to9(indexOfMap).ToString();
        index.text = nameOfMap;

        isCom.enabled = isCompleted ? true : false;
    }

    public void OnSelectThis()
    {
          if (!isUnlock) return;
        Debug.Log(1);

        Game.instance.currentMap = indexOfMap;

        SelectMapUI.LoadMap();
    }


}
