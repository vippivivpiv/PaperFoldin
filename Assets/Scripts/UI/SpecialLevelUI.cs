using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialLevelUI : GameUI
{
    public InGameUI InGameUI;
    public LevelComplete LevelComplete;
    public SelectMapUI SelectMapUI;

    public UI2DSprite specialImageMap;
    public TweenPosition tweenPosition;
    public new void Show()
    {
        base.Show();

        specialImageMap.sprite2D = Game.Instance.images[Game.Instance.currentMap].Image;


        tweenPosition.from = new Vector3(0, 2000, 0);
        tweenPosition.to = Vector3.zero;
        tweenPosition.ResetToBeginning();
        tweenPosition.PlayForward();
    }
    

    public void ClickPlay()
    {

        DataPlayer.ShowedSpecialLevel(DataPlayer.Get10to9(Game.Instance.currentMap));

        Hide();
        LevelComplete.Hide();

        Game.Instance.currentMap += 1;
        DataPlayer.CurrentPlayingMap = Game.Instance.currentMap;

        InGameUI.Show();
        Game.Instance.LoadMap();

        Game.Instance.PlayFXButton();

    }


    public void ClickNoThanks()
    {

        tweenPosition.from = Vector3.zero;
        tweenPosition.to = new Vector3(0, -2000, 0);
        tweenPosition.ResetToBeginning();
        tweenPosition.PlayForward();

        Invoke("Hide",0.3f);
        LevelComplete.Invoke("Hide", 0.3f);
       // Hide();
       // LevelComplete.Hide();

        DataPlayer.ShowedSpecialLevel(DataPlayer.Get10to9(Game.Instance.currentMap));
        Game.Instance.currentMap += 2;
        DataPlayer.CurrentPlayingMap += 1;

        InGameUI.Show();
        Game.Instance.LoadMap();

        Game.Instance.PlayFXButton();

    }
}
