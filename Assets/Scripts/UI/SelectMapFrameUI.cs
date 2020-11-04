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

    public bool canClick;

    UI2DSprite s;
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

        UI2DSprite_MapImage.transform.SetParent(this.transform);
        UI2DSprite_MapImage.transform.localScale = Vector3.one;
        UI2DSprite_MapImage.transform.localPosition = new Vector3(0, 30, 0);

        index.text = nameOfMap;

        isCom.enabled = isCompleted ? true : false;

        canClick = true;
    }

    public void OnSelectThis()
    {
        if (!isUnlock) return;
        if (!SelectMapUI.canClick) return;

        SelectMapUI.canClick = false;
        Game.Instance.currentMap = indexOfMap;

<<<<<<< HEAD
<<<<<<< HEAD
        StartCoroutine(LoadMap());

        //SelectMapUI.LoadMap();
=======
        //StartCoroutine(LoadMap());
        SelectMapUI.LoadMap();
>>>>>>> parent of b44d83b... 1
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

<<<<<<< HEAD
       // SelectMapUI.LoadMap();
=======
        SelectMapUI.LoadMap();

        // Invoke("DestroyImage", 0.6f);
        //  StartCoroutine(LoadMap());

    }


    IEnumerator LoadMap()
    {

        UI2DSprite_MapImage.transform.SetParent(SelectMapUI.tweenImage.transform);
        UI2DSprite_MapImage.depth = 3;
        UI2DSprite_MapImage.enabled = false;
        UI2DSprite_MapImage.enabled = true;
        UI2DSprite_MapImage.GetComponent<TweenPosition>().from = UI2DSprite_MapImage.transform.localPosition;
        UI2DSprite_MapImage.GetComponent<TweenPosition>().to = new Vector3(0, 100, 0);
        UI2DSprite_MapImage.GetComponent<TweenPosition>().PlayForward();
        UI2DSprite_MapImage.GetComponent<TweenPosition>().ResetToBeginning();


        UI2DSprite_MapImage.GetComponent<TweenScale>().PlayForward();
        UI2DSprite_MapImage.GetComponent<TweenScale>().ResetToBeginning();

        yield return new WaitForSeconds(0f);
     

       SelectMapUI.LoadMap();




    }


    private void DestroyImage()
    {

        // Destroy(s.gameObject);
        //UI2DSprite_MapImage.transform.SetParent(this.transform);
        //UI2DSprite_MapImage.transform.localScale = Vector3.one;
        //UI2DSprite_MapImage.transform.localPosition = new Vector3(0, 30, 0);

>>>>>>> 5d931418928b54d8a66623910e85834575dd5cf5
    }
=======
    //    SelectMapUI.LoadMap();
    //}
>>>>>>> parent of b44d83b... 1

}
