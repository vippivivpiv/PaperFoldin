using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GameUI
{
    public SelectMapUI SelectStageUI;
    public BackgroundImageUI BackgroundImageUI;
    public LevelComplete LevelComplete;
    public UILabel currentLevel;

    
    public new void Show(bool LoadFromMainmenu)
    {
        base.Show();
        BackgroundImageUI.Show();
        LoadMap(LoadFromMainmenu);
        currentLevel.text = "Level " + Game.instance.currentMap.ToString();
    }
    public new void Hide()
    {
        base.Hide();
        BackgroundImageUI.Hide();
    }
    private void Update()
    {
          
        //Debug.Log(Panel.GetComponent<UIPanel>().sortingLayerName);
        //Panel.GetComponent<UIPanel>().sortingLayerName = "1";
        //Debug.Log(Panel.GetComponent<UIPanel>().sortingOrder);
        //Panel.GetComponent<UIPanel>().sortingOrder = 1;
    }
    public void LoadMap(bool LoadFromMainmenu)
    {
        Game.instance.LoadMap(LoadFromMainmenu);
    }

    public void LevelCompleted()
    {
        Hide();
        LevelComplete.Show();
       
    }

    public void BackToMainMenu()
    {
        Hide();
        SelectStageUI.Show();
        Game.instance.DestroyMap();
    }

    public void Replay()
    {
        Destroy(Game.instance.playingMap.gameObject);
        Game.instance.LoadMap(false);
    }

    public void WatchAds()
    {
        Debug.Log("WatchAds");
    }

   
}
