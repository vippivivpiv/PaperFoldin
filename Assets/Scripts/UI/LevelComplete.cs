using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class LevelComplete : GameUI
{
    public InGameUI InGameUI;
    public SelectMapUI SelectMapUI;
    public UISprite blackImage;
    public SpecialLevelUI SpecialLevelUI;
    public UILabel CompletedMapCount;
    public TweenScale tweenScale;

    public new void Show()
    {
        base.Show();

        tweenScale.PlayForward();

        BackgroundManager.instance.selectMap.SetActive(true);
        blackImage.spriteName = "Map" + Game.Instance.playingMap.GetComponentInChildren<ImageProperties>().stt + "_3";
        CompletedMapCount.text = DataPlayer.GetTextOfCompletedMapCount();
        Debug.Log(Game.Instance.currentMap);

        if (DataPlayer.Get10to9(Game.Instance.currentMap) % 9 == 0)
        {

            if (!DataPlayer.IsShowedSpecialLevel(Game.Instance.currentMap))
            {
                SpecialLevelUI.Show();
            }

        }
        //else
        //{
        //    Game.instance.currentMap += 1;
        //    InGameUI.Show(false);
        //}


    }

    public new void Hide()
    {
        base.Hide();
        BackgroundManager.instance.selectMap.SetActive(false);
    }
    public void ClickClose()
    {
        Hide();
        SelectMapUI.Show();

    }
    public void ClickNext()
    {
        Hide();
        if (Game.Instance.currentMap == 30)
        {
            ClickClose();
            return;
        }

        Game.Instance.currentMap += 1;

        InGameUI.Show();


        Game.Instance.LoadMap();


    }




}
