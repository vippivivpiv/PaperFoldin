using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialLevelUI : GameUI
{
    public InGameUI InGameUI;
    public LevelComplete LevelComplete;
    public SelectMapUI SelectMapUI;

    public UI2DSprite specialImageMap;

    public new void Show()
    {
        base.Show();

        specialImageMap.sprite2D = Game.Instance.images[Game.Instance.currentMap].Image;

    }
    

    public void ClickPlay()
    {
        Debug.Log(Game.Instance.currentMap);
        DataPlayer.ShowedSpecialLevel(DataPlayer.Get10to9(Game.Instance.currentMap));

        Debug.Log(DataPlayer.IsShowedSpecialLevel(Game.Instance.currentMap));
        Hide();
        LevelComplete.Hide();

        Game.Instance.currentMap += 1;
        DataPlayer.CurrentPlayingMap = Game.Instance.currentMap;

        InGameUI.Show();
        Game.Instance.LoadMap();

        

    }


    public void ClickNoThanks()
    {
        Hide();
        LevelComplete.Hide();

        DataPlayer.ShowedSpecialLevel(DataPlayer.Get10to9(Game.Instance.currentMap));
        Game.Instance.currentMap += 2;
        DataPlayer.CurrentPlayingMap += 1;

        InGameUI.Show();
        Game.Instance.LoadMap();


        
    }
}
