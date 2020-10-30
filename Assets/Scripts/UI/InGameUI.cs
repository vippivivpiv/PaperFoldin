using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GameUI
{
    public SelectMapUI SelectStageUI;
    public BackgroundImageUI BackgroundImageUI;
    public LevelComplete LevelComplete;
    public UILabel currentLevel;

    public GameObject Btn_WatchAds;

    
    public new void Show(bool LoadFromMainmenu)
    {
        base.Show();
        BackgroundManager.instance.Ingame.SetActive(true);
        LoadMap(LoadFromMainmenu);
        if (Game.instance.currentMap %10 !=0)
        {
            currentLevel.text = "Level " + DataPlayer.Get10to9(Game.instance.currentMap).ToString() ;
        }
        else
        {
            currentLevel.text = "Special Level " + DataPlayer.Get10to9(Game.instance.currentMap).ToString();
        }
        
        BackgroundImageUI.Show();
        // Btn_WatchAds.SetActive(false);

        Btn_WatchAds.GetComponent<UISprite>().color = Color.white;
        Btn_WatchAds.GetComponent<TweenColor>().enabled = false;
    }
    public new void Hide()
    {
        base.Hide();
        BackgroundManager.instance.Ingame.SetActive(false);
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

    public void BackToSelectMap()
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


    public void DisplayWatchAds()
    {
        // Btn_WatchAds.SetActive(true);
        Btn_WatchAds.GetComponent<TweenColor>().enabled = true;

    }
    public void WatchAds()
    {
        Game.instance.ShowHint();

        Debug.Log("WatchAds");
    }

   
}
