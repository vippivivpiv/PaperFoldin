using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMapUI : GameUI
{
    public MainMenuUI MainMenuUI;
    public InGameUI InGameUI;

    public UILabel CompletedMapCount;

    public Game game;
    public UIScrollView UIScrollView;
    public UIGrid grid;
    public SelectMapFrameUI SelectMapFrameUIPrefab;
    public Exhibit ExhibitPrefab;
    private int numberOfMap;
    private int currentPlayingMap;


    private void Start()
    {

        

    }
    public new void Show()
    {

        base.Show();
        BackgroundManager.instance.selectMap.SetActive(true);

        InstantiateSelectMap();
        CompletedMapCount.text = "Level: " + DataPlayer.GetTextOfCompletedMapCount();
    }


    public new void Hide()
    {
        base.Hide();
        BackgroundManager.instance.selectMap.SetActive(false);
    }
    public void InstantiateSelectMap()
    {
        for (int i = 1; i < 4; i++)
        {
            
            Exhibit e = Instantiate(ExhibitPrefab, UIScrollView.transform);
            
            e.gameObject.name = "Ex" + i.ToString();
               e.transform.localPosition = new Vector3(0, 2150 - 2150*i, 0);
            e.numberOfExh.text = "Exhibit No" + i.ToString();
            e.sttExh = i;
            e.SelectMapUI = this;
            
            e.InstantiateExh();
        }

    }
    public void DeleteSelectMap()
    {
        foreach (Transform child in UIScrollView.transform)
        {
           if (child!= null) Destroy(child.gameObject);
        }
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
