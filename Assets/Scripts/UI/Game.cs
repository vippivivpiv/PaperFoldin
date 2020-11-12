﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;

    public static Game Instance;

    public TutorialController TutorialController;

    public Camera mainCam;
    public InGameUI InGameUI;

    public ImageProperties[] images;
    public Slice169 mapPrefab;
    public Slice169[] maps;
    public Slice169 playingMap;

    public AudioSource AudioSource;

    public bool isTutorial;

    public int currentMap = 1;



    private bool first;


    public bool isShowBlackImage;
    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;

        Application.targetFrameRate = 60;
    }
    private void Update()
    {

        if (playingMap!= null && playingMap.winCheck.isWin) return;



    }
    private void Start()
    {

        if ((float)Screen.height / (float)Screen.width <= 16f / 9f) mainCam.orthographicSize = 19.224f;
        else if ((float)Screen.height / (float)Screen.width > 16f / 9f)
        {
            mainCam.orthographicSize = 19.224f * (((float)Screen.height / (float)Screen.width) / (16f / 9f));
        }


        for (int i = 1; i <= images.Length; i++)
        {
            if (i % 10 == 0) images[i - 1].isSpecialLevel = true;
        }
    }

    public void OnSoundChange()
    {
       // AudioSource.mute = !DataPlayer.Sound;
    }
    public void LoadMap()
    {
        // if (LoadFromMainmenu) currentMap = DataPlayer.CurrentPlayingMap;

        if (playingMap != null) Destroy(playingMap.gameObject);

        playingMap = Instantiate(mapPrefab, transform);

        if (!DataPlayer.IsPlayTutorial)
        {
            TutorialController.gameObject.SetActive(true);
            TutorialController.Show();

            TutorialController.Slice169 = playingMap;
            playingMap.isTutorial = true;
            InGameUI.currentLevel.text = "";
           // BackgroundManager.instance.Ingame.gameObject.SetActive(false);
        }

        playingMap.winCheck.answer = Instantiate(images[currentMap - 1], playingMap.transform);

        first = false;
    }
    public void Replay()
    {
        Vector3 p1 = playingMap.point1;
        Vector3 p2 = playingMap.point2;

        Destroy(playingMap.gameObject);

        LoadMap();
        playingMap.DisplayGiayNhan(p1, p2);
    }
    public void LevelCompleted()
    {
        if (!DataPlayer.IsPlayTutorial)
        {
            TutorialController.gameObject.SetActive(false);
            TutorialController.Hide();
            DataPlayer.IsPlayTutorial = true;
        }
        if (DataPlayer.CurrentPlayingMap == currentMap)
        {
            DataPlayer.CompletedMapCount += 1;
            DataPlayer.CurrentPlayingMap += 1;
        }
        DataPlayer.CompletedMap(currentMap);

        InGameUI.LevelCompleted();

      //  DestroyMap();
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
