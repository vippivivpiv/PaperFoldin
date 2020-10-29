using System.Collections;
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
        Debug.Log(DataPlayer.IsShowedSpecialLevel(9));
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
