using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class LevelComplete : GameUI
{
    public InGameUI InGameUI;
    public SelectMapUI SelectMapUI;
    public SpecialLevelUI SpecialLevelUI;

    public UILabel CompletedMapCount;
    public TweenPosition TweenPosition;

    public UISprite blackImage;



    public UI2DSprite imageNextLevel;
    public UILabel labelLevelComplete, labelNextLevel;
    public UISprite spriteBlackAnswer;
    public UI2DSprite spriteNextLevel;
    public UISprite btnNext;

    Coroutine levelComplete, playNextMap;


    public new void Show()
    {
        base.Show();
        BackgroundManager.instance.Ingame.SetActive(true);

      //  StopAllCoroutines();

        //Panel.GetComponent<UIPanel>().alpha = 1;
        //blackImage.alpha = 1;
        //imageNextLevel.alpha = 0;

        //labelLevelComplete.alpha = 1;
        //labelNextLevel.alpha = 0;
        //btnNext.alpha = 0;

 
        TweenPosition.from = new Vector3(0, 2000, 0);
        TweenPosition.to = Vector3.zero;

        TweenPosition.ResetToBeginning();
        TweenPosition.PlayForward();

        blackImage.spriteName = "Map" + Game.Instance.playingMap.GetComponentInChildren<ImageProperties>().stt + "_3";
        imageNextLevel.sprite2D = Game.Instance.images[Game.Instance.currentMap].Image;




        CompletedMapCount.text = DataPlayer.GetTextOfCompletedMapCount();
        Debug.Log(Game.Instance.currentMap);

        if (DataPlayer.Get10to9(Game.Instance.currentMap) % 9 == 0)
        {
            if (!DataPlayer.IsShowedSpecialLevel(Game.Instance.currentMap))
            {
                SpecialLevelUI.Show();
            }
        }

        levelComplete= StartCoroutine(OnLevelComplete());




        //else
        //{
        //    Game.instance.currentMap += 1;
        //    InGameUI.Show(false);
        //}


    }
    public new void Hide()
    {

        StartCoroutine(OnHide());

        //  StopAllCoroutines();
    }

    IEnumerator OnHide()
    {

        TweenPosition.from = Vector3.zero;
        TweenPosition.to = new Vector3(0, -2000, 0);

        TweenPosition.ResetToBeginning();
        TweenPosition.PlayForward();

        yield return new WaitForSeconds(0.3f);

        base.Hide();
    }
    public void ClickClose()
    {
        TweenPosition.from = Vector3.zero;
        TweenPosition.to =  new Vector3(0, -2000, 0);

        TweenPosition.ResetToBeginning();
        TweenPosition.PlayForward();

        InGameUI.Hide();
        SelectMapUI.Show();

        Hide();


    }
    public void ClickNext()
    {
        Game.Instance.currentMap += 1;

        InGameUI.Show();


        Game.Instance.LoadMap();
      
       
        Hide();
       // playNextMap = StartCoroutine( OnPlayNextMap());

    }


    IEnumerator OnLevelComplete()
    {
        yield return new WaitForSeconds(2f);
        labelLevelComplete.GetComponent<TweenAlpha>().ResetToBeginning();
        labelNextLevel.GetComponent<TweenAlpha>().ResetToBeginning();
        spriteBlackAnswer.GetComponent<TweenAlpha>().ResetToBeginning();
        spriteNextLevel.GetComponent<TweenAlpha>().ResetToBeginning();
        btnNext.GetComponent<TweenAlpha>().ResetToBeginning();

        labelLevelComplete.GetComponent<TweenAlpha>().PlayForward();
        labelNextLevel.GetComponent<TweenAlpha>().PlayForward();
        spriteBlackAnswer.GetComponent<TweenAlpha>().PlayForward();
        spriteNextLevel.GetComponent<TweenAlpha>().PlayForward();
        btnNext.GetComponent<TweenAlpha>().PlayForward();
        btnNext.gameObject.SetActive(true);
    }


    IEnumerator OnPlayNextMap()
    {
        UI2DSprite i = Instantiate(imageNextLevel, SelectMapUI.tweenImage.transform);
        i.enabled = false;
        i.enabled = true;

        //Panel.GetComponent<TweenAlpha>().from = 1;
        //Panel.GetComponent<TweenAlpha>().to = 0;
        //Panel.GetComponent<TweenAlpha>().ResetToBeginning();
        //Panel.GetComponent<TweenAlpha>().PlayForward();


        i.GetComponent<TweenPosition>().from = i.transform.localPosition;
        i.GetComponent<TweenPosition>().to = new Vector3(0, 100, 0);
        i.GetComponent<TweenPosition>().ResetToBeginning();
        i.GetComponent<TweenPosition>().PlayForward();


        i.GetComponent<TweenScale>().from = Vector3.one;
        i.GetComponent<TweenScale>().to = new Vector3(1.82f, 1.82f, 0);
        i.GetComponent<TweenScale>().ResetToBeginning();
        i.GetComponent<TweenScale>().PlayForward();


        yield return new WaitForSeconds(0.2f);

        Panel.GetComponent<TweenAlpha>().from = 1;
        Panel.GetComponent<TweenAlpha>().to = 0;
        Panel.GetComponent<TweenAlpha>().ResetToBeginning();
        Panel.GetComponent<TweenAlpha>().PlayForward();


        yield return new WaitForSeconds(0.2f);

        //TweenPosition.from =  Vector3.zero;
        //TweenPosition.to = new Vector3(0, -2000, 0);

        //TweenPosition.ResetToBeginning();
        //TweenPosition.PlayForward();



        //if (Game.Instance.currentMap == 30)
        //{
        //    ClickClose();
        //    return;
        //}

        Game.Instance.currentMap += 1;

        InGameUI.Show();


        Game.Instance.LoadMap();
        yield return new WaitForSeconds(0.1f);
        Destroy(i.gameObject);
        Hide();
    }


    //IEnumerator OnClose()
    //{

    //}
}
