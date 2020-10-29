using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : GameUI
{
    public SettingUI SettingUI;
    public RemoveAdsUI RemoveAdsUI;
    public SelectMapUI SelectStageUI;
    public InGameUI InGameUI;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
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
    }

    public void OpenRemoveAds()
    {
        RemoveAdsUI.Show();
    }

    public void ClickPlay()
    {
        Hide();
        InGameUI.Show(true);
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();

      //  DataPlayer.CurrentPlayingMap = 19;
    }
    public void UnlockAllMap()
    {
        DataPlayer.CurrentPlayingMap = 30;
    }
    public void ClickShowAnswer()
    {
        DataPlayer.ShowAnswer = true;
        if (Game.instance != null) Game.instance.playingMap.gameObject.GetComponentInChildren<ImageProperties>().ShowOrHideAnswer();
    }

    public void ClickHideAnswer()
    {
        DataPlayer.ShowAnswer = false;
        if (Game.instance != null) Game.instance.playingMap.gameObject.GetComponentInChildren<ImageProperties>().ShowOrHideAnswer();
    }

}
