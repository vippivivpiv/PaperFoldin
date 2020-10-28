﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class LevelComplete : GameUI
{
    public InGameUI InGameUI;
    public UISprite blackImage;
    public SpecialLevelUI SpecialLevelUI;
    public UILabel CompletedMapCount;

    public new void Show()
    {
        base.Show();
        blackImage.spriteName = "Map" + Game.instance.playingMap.GetComponentInChildren<ImageProperties>().stt + "_3";
        CompletedMapCount.text = DataPlayer.GetTextOfCompletedMapCount();
    }
    public void ClickNext()
    {
        Hide();
        Game.instance.currentMap += 1;
        InGameUI.Show(false);
    }




}
