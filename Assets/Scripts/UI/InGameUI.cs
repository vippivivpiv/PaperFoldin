using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GameUI
{
    public SelectMapUI SelectStageUI;
    public BackgroundImageUI BackgroundImageUI;
    public LevelComplete LevelComplete;
    public UILabel currentLevel;

    public GameObject Btn_WatchAds;
    public GameObject WatchAds_SpriteFree;

    public UISprite Hint_BlackImage;
    public UI2DSprite Hint_BlackImage_2Dsprite;
    private float sinceLoadGame;
    private bool isShowBlackImage;

    private void Update()
    {
        sinceLoadGame += Time.deltaTime;

        if (sinceLoadGame > 10f)
        {
            if (!isShowBlackImage)
            {
                isShowBlackImage = true;
                ShowHint_BlackImage(true); 
            }

        }
    }
    public new void Show()
    {
        base.Show();
        BackgroundImageUI.Show();

        BackgroundManager.instance.Ingame.SetActive(true);

        sinceLoadGame = 0f;
        isShowBlackImage = false;
        //Panel.GetComponent<TweenAlpha>().from = 0;
        //Panel.GetComponent<TweenAlpha>().to = 1;
        //Panel.GetComponent<TweenAlpha>().ResetToBeginning();
        //Panel.GetComponent<TweenAlpha>().PlayForward();

        //BackgroundManager.instance.Ingame.GetComponent<TweenAlpha>().from = 0;
        //BackgroundManager.instance.Ingame.GetComponent<TweenAlpha>().to = 1;
        //BackgroundManager.instance.Ingame.GetComponent<TweenAlpha>().ResetToBeginning();
        //BackgroundManager.instance.Ingame.GetComponent<TweenAlpha>().PlayForward();

        //BackgroundImageUI.Panel.GetComponent<TweenAlpha>().from = 0;
        //BackgroundImageUI.Panel.GetComponent<TweenAlpha>().from = 0;
        //BackgroundImageUI.Panel.GetComponent<TweenAlpha>().to = 1;
        //BackgroundImageUI.Panel.GetComponent<TweenAlpha>().ResetToBeginning();
        //BackgroundImageUI.Panel.GetComponent<TweenAlpha>().PlayForward();


        if (Game.Instance.currentMap % 10 != 0)
        {
            currentLevel.text = "Level " + DataPlayer.Get10to9(Game.Instance.currentMap).ToString();
        }
        else
        {
            currentLevel.text = "Special Level " + DataPlayer.Get10to9(Game.Instance.currentMap).ToString();
        }

        WatchAds_SpriteFree.SetActive(DataPlayer.IsWatchedAdsMap(Game.Instance.currentMap));

        Hint_BlackImage_2Dsprite.sprite2D = Game.Instance.images[Game.Instance.currentMap-1].BackAnswer;
        Hint_BlackImage.spriteName = "Map" + Game.Instance.currentMap.ToString() + "_3";

        Hint_BlackImage_2Dsprite.gameObject.SetActive(false);

        ShowHint_BlackImage(false);

        Game.Instance.isShowBlackImage = false;


    }
    public new void Hide()
    {
        base.Hide();
        BackgroundManager.instance.Ingame.SetActive(false);
        BackgroundImageUI.Hide();
        if (Game.Instance.playingMap != null) Destroy(Game.Instance.playingMap.gameObject);
    }

    public void LoadMap()
    {
        Game.Instance.LoadMap();
    }

    public void LevelCompleted()
    {
        //Hide();
        LevelComplete.Show();

    }

    public void BackToSelectMap()
    {

        Hide();

        SelectStageUI.Show();

        Game.Instance.DestroyMap();

    }

    public void Replay()
    {
        Game.Instance.Replay();




    }


    public void DisplayWatchAds()
    {
        // Btn_WatchAds.SetActive(true);
        Btn_WatchAds.GetComponent<TweenColor>().enabled = true;
        Show();

    }
    public void WatchAds()
    {
        Game.Instance.ShowHint();

        DataPlayer.WatchedAdsMap(Game.Instance.currentMap);

        Show();
    }


    public void ShowHint_BlackImage(bool b)
    {
        if (!DataPlayer.IsShowHintBlackImage) return;
        Hint_BlackImage_2Dsprite.gameObject.SetActive(b);
        
        // Hint_BlackImage.gameObject.SetActive(b);
        Hint_BlackImage.GetComponent<TweenAlpha>().ResetToBeginning();

        Hint_BlackImage.GetComponent<TweenAlpha>().PlayForward();
    }


}
