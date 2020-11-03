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

    public TweenPosition tweenPos;
    public TweenScale tweenScale;

    public int indexOfMap;
    public string nameOfMap;
    public string spriteName;

    public bool isSpecialLevel;

    public bool isCompleted;
    public bool isUnlock;

    public void UpdateStateOfMap()
    {


        if (indexOfMap <= DataPlayer.CurrentPlayingMap)
        {
            isUnlock = true;

            spriteName = "Map" + (indexOfMap).ToString();

            UI2DSprite_MapImage.gameObject.SetActive(true);
            UI2DSprite_LockedImage.gameObject.SetActive(false);


        }
        else
        {
            isUnlock = false;
            spriteName = "Locked";

            UI2DSprite_MapImage.gameObject.SetActive(false);
            UI2DSprite_LockedImage.gameObject.SetActive(true);
        }
        isCompleted = DataPlayer.IsCompletedMap(indexOfMap);


        UI2DSprite_MapImage.sprite2D = Game.Instance.images[indexOfMap - 1].Image;

        index.text = nameOfMap;

        isCom.enabled = isCompleted ? true : false;
    }

    public void OnSelectThis()
    {
        if (!isUnlock) return;

        Game.Instance.currentMap = indexOfMap;

        //StartCoroutine(LoadMap());
        SelectMapUI.LoadMap();
    }


    //IEnumerator LoadMap()
    //{
    //    GameObject s = Instantiate(UI2DSprite_MapImage.gameObject,SelectMapUI.Panel.transform);
    //    s.GetComponent<UI2DSprite>().depth = 3; 
    //    s.GetComponent<TweenPosition>().from = transform.localPosition;
    //    s.GetComponent<TweenPosition>().to = Vector3.zero;

    //    s.GetComponent<TweenPosition>().PlayForward();
    //    yield return new WaitForSeconds(0.5f);
    //    Destroy(s.gameObject);

    //    SelectMapUI.LoadMap();
    //}

}
