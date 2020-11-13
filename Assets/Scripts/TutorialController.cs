using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : GameUI
{
    public InGameUI InGameUI;
    public UIPanel tutPanel;
    public Slice169 Slice169;
    public UILabel textTut;
    public TweenPosition hand1, hand2;
    public GameObject arrow1, arrow2;
    public TweenScale hand3;
    private bool first;

    public TweenPosition netDut;
    private void Start()
    {

    }


    public new void Show()
    {
        base.Show();
        InGameUI.Panel.SetActive(false);
    }

    public new void Hide()
    {
        base.Hide();

    }
    void Update()
    {
        // Slice to serate the image
        // Drag the seperated part to the right position
        // Release hand at the correct position

        if (Slice169 == null) return;

        if (!first)
        {
            first = true;
            tutPanel.gameObject.SetActive(true);
        }

        if ( Slice169.winCheck.isWin)
        {
            Hide();
        }

        if (!Slice169.isSliced)
        {

            PlayTut1();

        }
        else if (Vector2.Distance(Slice169.winCheck.answer.Point1, Slice169.winCheck.answer.Point2) <= 0.2f)
        {
            PlayTut3();
        }
        else if (Slice169.isSliced && !Slice169.winCheck.isWin)
        {

            PlayTut2();

        }




    }

    public void PlayTut1()
    {
        textTut.text = "Slice to seperate the image";
        hand1.gameObject.SetActive(true);
       // arrow1.gameObject.SetActive(true);
        netDut.gameObject.SetActive(true);
        hand2.gameObject.SetActive(false);
        arrow2.gameObject.SetActive(false);

    }

    public void PlayTut2()
    {
        textTut.text = "Drag the seperated part to the right position";
        hand1.gameObject.SetActive(false);
        arrow1.gameObject.SetActive(false);
        netDut.gameObject.SetActive(false);
        hand2.gameObject.SetActive(true);
        arrow2.gameObject.SetActive(true);
    }

    public void PlayTut3()
    {
        textTut.text = "Release hand at the correct position";
        hand1.gameObject.SetActive(false);
        arrow1.gameObject.SetActive(false);
        netDut.gameObject.SetActive(false);
        hand2.gameObject.SetActive(false);
        arrow2.gameObject.SetActive(false);
      //  hand3.gameObject.SetActive(true);
    }


}
