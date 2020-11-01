﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;

    public static Game Instance
    {
        get
        {
            if (instance == null) instance = new Game();
            return instance;
        }
    }
    public Camera mainCam;
    public InGameUI InGameUI;

    public ImageProperties[] images;
    public Slice169 mapPrefab;
    public Slice169[] maps;
    public Slice169 playingMap;

    public AudioSource AudioSource;


    public int currentMap = 1;

    public float sinceLoadGame;
    private bool first;

    private void Update()
    {
        sinceLoadGame += Time.deltaTime;
        if (sinceLoadGame > 5f)
        {
            if ( !first)
            {
                first = true;
                InGameUI.DisplayWatchAds();
            }
            
        }
    }
    private void Start()
    {

        mainCam.orthographicSize = 19.224f * (((float)Screen.height/(float)Screen.width) / (16f/9f));


        if (instance != null)
        {
            Destroy(instance);
            instance = this;
        }
        else instance = this;

        OnSoundChange();
    }

    public void OnSoundChange()
    {
       // AudioSource.mute = !DataPlayer.Sound;
    }
    public void LoadMap(bool LoadFromMainmenu)
    {
        if (LoadFromMainmenu) currentMap = DataPlayer.CurrentPlayingMap;

        playingMap = Instantiate(mapPrefab, transform);
        
        playingMap.winCheck.answer = Instantiate(images[currentMap - 1], playingMap.transform);
        sinceLoadGame = 0f;
        first = false;
    }

    public void LevelCompleted()
    {
        if (DataPlayer.CurrentPlayingMap == currentMap)
        {
            DataPlayer.CompletedMapCount += 1;
            DataPlayer.CurrentPlayingMap += 1;
        }
        DataPlayer.CompletedMap(currentMap);

        InGameUI.LevelCompleted();

        DestroyMap();
    }
    public void ShowHint()
    {
        playingMap.winCheck.ShowHint();
    }
    public void DestroyMap()
    {
        if (playingMap!= null) Destroy(playingMap.gameObject);
    }
}
