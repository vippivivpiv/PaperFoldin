using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMapUI : GameUI
{
    public MainMenuUI MainMenuUI;
    public InGameUI InGameUI;

    public UILabel CompletedMapCount;

    public Game game;
    public UIGrid grid;
    public SelectMapFrameUI SelectMapFrameUIPrefab;
    private int numberOfMap;
    private int currentPlayingMap;


    private void Start()
    {
        InstantiateSelectMap();
    }
    public void InstantiateSelectMap()
    {
        numberOfMap = DataPlayer.NumberOfMap;
        currentPlayingMap = DataPlayer.CurrentPlayingMap;
        for (int i = 1; i < 9; i++)
        {
            SelectMapFrameUI s = Instantiate(SelectMapFrameUIPrefab, grid.transform);
            s.gameObject.name = "Map" + (i).ToString();
            s.indexOfMap = i;
            s.SelectMapUI = this;
            s.UpdateStateOfMap();
        }

        grid.enabled = false;
        grid.enabled = true;
    }

    public new void Show()
    {
        base.Show();
        CompletedMapCount.text = DataPlayer.GetTextOfCompletedMapCount();
    }

    public void BackToMainMenu()
    {
        Hide();
        MainMenuUI.Show();
    }

    public void LoadMap()
    {
        Hide();
        InGameUI.Show(false);
    }
}
