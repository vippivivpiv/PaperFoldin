﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : GameUI
{
    public SettingUI SettingUI;
    public RemoveAdsUI RemoveAdsUI;
    public SelectMapUI SelectStageUI;
    public InGameUI InGameUI;
    public TweenScale btnSetting, btnRemoveAds, btnPlay;
    private void Start()
    {
        //PlayerPrefs.DeleteAll();

       // DataPlayer.IsPlayTutorial = true;
       // DataPlayer.CurrentPlayingMap = 45;
    }

    public new void Show()
    {
        base.Show();
        BackgroundManager.instance.mainmenu.SetActive(true);

    }
    public new void Hide()
    {
        base.Hide();
        BackgroundManager.instance.mainmenu.SetActive(false);
    }
    public void OpenSetting()

    {
        SettingUI.Show();
        Game.Instance.PlayFXButton();
    }

    public void OpenRemoveAds()
    {
        RemoveAdsUI.Show();
        Game.Instance.PlayFXButton();
    }

    public void ClickPlay()
    {

        Hide();
        Game.Instance.currentMap = DataPlayer.CurrentPlayingMap;
        InGameUI.Show();
        Game.Instance.LoadMap();
        Game.Instance.PlayFXButton();
    }


    //public void ClickShowAnswer()
    //{
    //    DataPlayer.ShowAnswer = true;
    //    if (Game.Instance != null) Game.Instance.playingMap.gameObject.GetComponentInChildren<ImageProperties>().ShowOrHideAnswer();
    //}

    //public void ClickHideAnswer()
    //{
    //    DataPlayer.ShowAnswer = false;
    //    if (Game.Instance != null) Game.Instance.playingMap.gameObject.GetComponentInChildren<ImageProperties>().ShowOrHideAnswer();
    //}





    public void  OnPressBtnSetting()
    {
        
        btnSetting.PlayForward();
    }

    public void OnReleaseBtnSetting()
    {

        btnSetting.PlayReverse();
       // OpenSetting();
    }

    public void OnPressBtnAds()
    {

        btnRemoveAds.PlayForward();
    }

    public void OnReleaseBtnAds()
    {

        btnRemoveAds.PlayReverse();
       // OpenRemoveAds();
    }

    public void OnPressBtnPlay()
    {

        btnPlay.PlayForward();
    }

    public void OnReleaseBtnPlay()
    {

        btnPlay.PlayReverse();
        // OpenSetting();
    }







    public void ResetData()
    {
         PlayerPrefs.DeleteAll();
        //DataPlayer.IsPlayTutorial = true;

    }
    public void JumpToMap9()
    {
        DataPlayer.IsPlayTutorial = true;
        DataPlayer.CurrentPlayingMap = 9;
    }
    public void UnlockAllMap()
    {
        DataPlayer.IsPlayTutorial = true;
        DataPlayer.CurrentPlayingMap = 45;
    }


    public void ShowHintBlackImage()
    {
        DataPlayer.IsShowHintBlackImage = true;
    }

    public void HideHintBlackImage()
    {
        DataPlayer.IsShowHintBlackImage = false;
    }

    public void DieuKhien2Diem()
    {
        DataPlayer.IsControllerPoint = true;
        DataPlayer.IsAutoMatch = false;

    }
    public void TuDongBatDuongCat()
    {
        DataPlayer.IsControllerPoint = false;
        DataPlayer.IsAutoMatch = true;
    }

}
