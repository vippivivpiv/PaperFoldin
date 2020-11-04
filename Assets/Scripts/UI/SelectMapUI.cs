using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMapUI : GameUI
{

    public static List<SelectMapFrameUI> selectMapFrameUIs;
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

<<<<<<< HEAD
    public UIPanel LabelTweenImage;
=======
    public UIPanel tweenImage;

    public bool canClick;

>>>>>>> 5d931418928b54d8a66623910e85834575dd5cf5
    private void Start()
    {

        selectMapFrameUIs = new List<SelectMapFrameUI>();
        Invoke("InstantiateSelectMap", 0.1f);

    }
    public new void Show()
    {

        base.Show();

        BackgroundManager.instance.selectMap.SetActive(true);


        //Panel.GetComponent<TweenAlpha>().from = 0;
        //Panel.GetComponent<TweenAlpha>().to = 1;
        //Panel.GetComponent<TweenAlpha>().ResetToBeginning();
        //Panel.GetComponent<TweenAlpha>().PlayForward();

        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().from = 0;
        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().to = 1;
        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().ResetToBeginning();
        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().PlayForward();


        UpdateStateOfSelectMap();

        CompletedMapCount.text = "Level: " + DataPlayer.GetTextOfCompletedMapCount();

        canClick = true;
    }


    public new void Hide()
    {
        base.Hide();


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


    public void UpdateStateOfSelectMap()
    {
        foreach ( SelectMapFrameUI s in selectMapFrameUIs)
        {
            s.UpdateStateOfMap();
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

        InGameUI.Show();
        Hide();
        Game.Instance.LoadMap();
        //Panel.GetComponent<TweenAlpha>().from = 1;
        //Panel.GetComponent<TweenAlpha>().to = 0;
        //Panel.GetComponent<TweenAlpha>().ResetToBeginning();
        //Panel.GetComponent<TweenAlpha>().PlayForward();

        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().from = 1;
        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().to = 0;
        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().ResetToBeginning();
        //BackgroundManager.instance.selectMap.GetComponent<TweenAlpha>().PlayForward();


        //StartCoroutine(OnLoadMap());
    }

    IEnumerator OnLoadMap()
    {

        yield return new WaitForSeconds(0.5f);

        Game.Instance.LoadMap();

    }

}
