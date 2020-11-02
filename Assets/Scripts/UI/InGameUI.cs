using JetBrains.Annotations;
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

    
    public new void Show()
    {
        base.Show();
        BackgroundImageUI.Show();
        BackgroundManager.instance.Ingame.SetActive(true);

        if (Game.Instance.currentMap %10 !=0)
        {
            currentLevel.text = "Level " + DataPlayer.Get10to9(Game.Instance.currentMap).ToString() ;
        }
        else
        {
            currentLevel.text = "Special Level " + DataPlayer.Get10to9(Game.Instance.currentMap).ToString();
        }
        
        Btn_WatchAds.GetComponent<UISprite>().color = Color.white;
        Btn_WatchAds.GetComponent<TweenColor>().enabled = false;
    }
    public new void Hide()
    {
        base.Hide();
        BackgroundManager.instance.Ingame.SetActive(false);
        BackgroundImageUI.Hide();
    }

    public void LoadMap()
    {
        Game.Instance.LoadMap();
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

        Game.Instance.DestroyMap();
 
    }

    public void Replay()
    {
        Game.Instance.Replay();
    }


    public void DisplayWatchAds()
    {
        // Btn_WatchAds.SetActive(true);
        Btn_WatchAds.GetComponent<TweenColor>().enabled = true;

    }
    public void WatchAds()
    {
        Game.Instance.ShowHint();

        Debug.Log("WatchAds");
    }

   
}
