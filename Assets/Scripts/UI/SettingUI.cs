﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : GameUI
{
    public MainMenuUI MainMenuUI;
    public Transform thanhTruotSound, thanhTruotFx;

    public TweenPosition tweenPosition;


    public new void Show()
    {
        base.Show();

        tweenPosition.from = new Vector3(0, 1600, 0);
        tweenPosition.to = Vector3.zero;    
        tweenPosition.PlayForward();
        tweenPosition.ResetToBeginning();

    }


    private void Start()
    {
        CheckSound();
        CheckFx();
    }
    public void CloseSetting()
    {
        tweenPosition.from = new Vector3(0, -1600, 0);
        tweenPosition.to = Vector3.zero;

        tweenPosition.PlayReverse();
        tweenPosition.ResetToBeginning();
        Game.Instance.PlayFXButton();
        Invoke("Hide", 0.2f);
       // Hide();
    }
    public void ClickChangeSound()
    {
        DataPlayer.Sound = !DataPlayer.Sound;
        CheckSound();
        Game.Instance.PlayFXButton();
    }

    public void ClickChangeFx()
    {
        DataPlayer.Fx = !DataPlayer.Fx;

        CheckFx();
        Game.Instance.PlayFXButton();
    }

    private void CheckSound()
    {
        if (DataPlayer.Sound) thanhTruotSound.localPosition = new Vector3(90, 0, 0);
        else thanhTruotSound.localPosition = new Vector3(-90, 0, 0);

        if (Game.Instance != null)
        {
            Game.Instance.OnOffSound();

        }
    }

    private void CheckFx()
    {
        if (DataPlayer.Fx) thanhTruotFx.localPosition = new Vector3(90, 0, 0);
        else thanhTruotFx.localPosition = new Vector3(-90, 0, 0);
    }
    public void ClickRestorePurchase()
    {
        Game.Instance.PlayFXButton();
        Debug.Log("RestorePurchase");
    }
}
