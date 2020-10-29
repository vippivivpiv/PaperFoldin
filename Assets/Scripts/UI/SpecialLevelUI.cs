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

        specialImageMap.sprite2D = Game.instance.images[Game.instance.currentMap].Image;

    }
    

    public void ClickPlay()
    {
        Debug.Log(Game.instance.currentMap);
        DataPlayer.ShowedSpecialLevel(DataPlayer.Get10to9(Game.instance.currentMap));

        Debug.Log(DataPlayer.IsShowedSpecialLevel(Game.instance.currentMap));
        Hide();
        LevelComplete.Hide();

        Game.instance.currentMap += 1;
        DataPlayer.CurrentPlayingMap = Game.instance.currentMap;

        InGameUI.Show(false);

        

    }


    public void ClickNoThanks()
    {
        DataPlayer.ShowedSpecialLevel(DataPlayer.Get10to9(Game.instance.currentMap));
        LevelComplete.Hide();
        Hide();
        Game.instance.currentMap += 2;
        DataPlayer.CurrentPlayingMap += 1;
        InGameUI.Show(false);


        
    }
}
