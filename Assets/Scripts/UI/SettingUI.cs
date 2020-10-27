using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : GameUI
{
    public MainMenuUI MainMenuUI;
    public Transform thanhTruotSound, thanhTruotFx;

    private void Start()
    {
        CheckSound();
        CheckFx();
    }
    public void CloseSetting()
    {
        Hide();
    }
    public void ClickChangeSound()
    {
        DataPlayer.Sound = !DataPlayer.Sound;
        CheckSound();
    }

    public void ClickChangeFx()
    {
        DataPlayer.Fx = !DataPlayer.Fx;

        CheckFx();
    }

    private void CheckSound()
    {
        if (DataPlayer.Sound) thanhTruotSound.localPosition = new Vector3(42, 0, 0);
        else thanhTruotSound.localPosition = new Vector3(-42, 0, 0);

        if (Game.instance != null) Game.instance.OnSoundChange();
    }

    private void CheckFx()
    {
        if (DataPlayer.Fx) thanhTruotFx.localPosition = new Vector3(42, 0, 0);
        else thanhTruotFx.localPosition = new Vector3(-42, 0, 0);
    }
    public void ClickRestorePurchase()
    {
        Debug.Log("RestorePurchase");
    }
}
