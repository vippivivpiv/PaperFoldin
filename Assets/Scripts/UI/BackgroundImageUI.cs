using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageUI : GameUI
{
    public UILabel Hint;

    public new void Show()
    {
        base.Show();

        Hint.text = Game.instance.images[Game.instance.currentMap - 1].KeyWord;
    }

}
