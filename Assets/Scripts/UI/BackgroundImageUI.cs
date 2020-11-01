using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageUI : GameUI
{
    public UILabel Hint;

    public new void Show()
    {
        base.Show();

        Hint.text = Game.Instance.images[Game.Instance.currentMap - 1].KeyWord;
    }

}
