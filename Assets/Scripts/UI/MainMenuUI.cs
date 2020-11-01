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
        Game.Instance.currentMap = DataPlayer.CurrentPlayingMap;
        InGameUI.Show();
        Game.Instance.LoadMap();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();

        DataPlayer.CurrentPlayingMap = 9;
    }
    public void UnlockAllMap()
    {
        DataPlayer.CurrentPlayingMap = 30;
    }
    public void ClickShowAnswer()
    {
        DataPlayer.ShowAnswer = true;
        if (Game.Instance != null) Game.Instance.playingMap.gameObject.GetComponentInChildren<ImageProperties>().ShowOrHideAnswer();
    }

    public void ClickHideAnswer()
    {
        DataPlayer.ShowAnswer = false;
        if (Game.Instance != null) Game.Instance.playingMap.gameObject.GetComponentInChildren<ImageProperties>().ShowOrHideAnswer();
    }

}
