﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public InGameUI InGameUI;
    public static Game instance;
    public ImageProperties[] images;
    public Slice169 mapPrefab;
    public Slice169[] maps;
    public Slice169 playingMap;

    public AudioSource AudioSource;


    public int currentMap;




    private void Start()
    {
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
        AudioSource.mute = !DataPlayer.Sound;
    }
    public void LoadMap(bool LoadFromMainmenu)
    {
        if (LoadFromMainmenu) currentMap = currentMap = DataPlayer.CurrentPlayingMap;
        playingMap = Instantiate(mapPrefab, transform);
        playingMap.winCheck.answer = Instantiate(images[currentMap - 1], playingMap.transform);
        playingMap.meshRenderer.material = playingMap.winCheck.answer.OriginalMaterial;
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

    public void DestroyMap()
    {
        Destroy(playingMap.gameObject);
    }
}
