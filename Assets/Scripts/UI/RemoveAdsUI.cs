using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsUI : GameUI
{

    public TweenPosition tweenPosition;
    public new void Show()
    {
        base.Show();
        tweenPosition.from = new Vector3(0, 1600, 0);
        tweenPosition.to = Vector3.zero;
        tweenPosition.PlayForward();
        tweenPosition.ResetToBeginning();

    }
    public void CloseRemoveAds()
    {
        tweenPosition.from = new Vector3(0, -1600, 0);
        tweenPosition.to = Vector3.zero;
        tweenPosition.PlayReverse();
        tweenPosition.ResetToBeginning();

        Game.Instance.PlayFXButton();
        Invoke("Hide", 0.2f);
       // Hide();
    }


    public void ClickBuyRemoveAds()
    {
        Game.Instance.PlayFXButton();
        Debug.Log("BuyRemoveAds");
    }
}
