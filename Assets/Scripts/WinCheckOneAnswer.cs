
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class WinCheckOneAnswer : MonoBehaviour
{
    public Slice169 slice169;
    public bool isWin = false;
    [Header("Answer0")]
    public string nameOfAns0;
    public Vector2 point1OfAns0;
    public Vector2 point2OfAns0;
    public float angleOfAns0;
    public static bool isFindedAns0;
    public bool isNeedAutoMatch0;
    private Vector3 p1MatchedOfPoint1Ans0;
    private Vector3 p2MatchedOfPoint1Ans0;
    private Vector3 p1MatchedOfPoint2Ans0;
    private Vector3 p2MatchedOfPoint2Ans0;

    [Header("")]
    public Vector3 point1ImagePos;
    public Vector3 point2ImagePos;
    private float angleCheck;

    private float timerCount;

    public GameObject answerDisplay;
    public ImageProperties answerPrefab;
    public ImageProperties answer;

    //public static ImageProperties[] answer;
    private bool iscalculator;
    private bool first;
    private bool second;
    private bool third;

    private void Start()
    {
        //answer = GetComponent<ImageProperties>();

       // answer=Instantiate(answerPrefab, gameObject.transform);

        timerCount = 0;

        CalculatePointAutoMatch();
    }


    private void Update()
    {
        
        if (slice169.isSliced)
        {
            if (!iscalculator)
            {
                iscalculator = true;

                if (angleCheck == 180) angleCheck = 0;
                CalculateDistanceFromPointToLine();
                SelectPointMoveAndDisplayImage();
            }
            UpdatePointPos();
            if ( !slice169.isMoving) CheckAnswers();

        }

    }

    private void CalculatePointAutoMatch()
    {
        Vector2[] testMatch = GetComponent<Slice169>().GetLineIntersection(point1OfAns0, angleOfAns0, slice169.peak3, slice169.peak2, slice169.peak0, slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        p1MatchedOfPoint1Ans0 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        p2MatchedOfPoint1Ans0 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        testMatch = GetComponent<Slice169>().GetLineIntersection(point2OfAns0, angleOfAns0, slice169.peak3, slice169.peak2, slice169.peak0, slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        p1MatchedOfPoint2Ans0 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        p2MatchedOfPoint2Ans0 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

    }
    public void SelectPointMoveAndDisplayImage()
    {
        if (answer.DisP1toLine < answer.DisP2toLine)
        {
            answer.OldPosPointMove = answer.Point1;
            answer.DisplayImagePos = answer.Point2ImagePos;
        }
        else
        {
            answer.OldPosPointMove = answer.Point2;
            answer.DisplayImagePos = answer.Point1ImagePos;
        }


    }
    private void Win()
    {
        isWin = true;
        StartCoroutine(PlayWinEffect());

    }


    IEnumerator PlayWinEffect()
    {
        if ( !first)
        {
            first = true;
            answer.spriteDisplay.SetActive(true);

            Vector2 P1P2 = answer.point2 - answer.point1;


            if (answer.DisP1toLine <= answer.DisP2toLine)
            {
                Debug.Log(2);
                answer.spriteDisplay.transform.localPosition = answer.spritePoint2.transform.localPosition;
                //answer.spriteDisplay.transform.localPosition = new Vector3(answer.spritePoint2.transform.localPosition.x - P1P2.x/2, answer.spritePoint2.transform.localPosition.y - P1P2.y/4, 0);
            }
            else if (answer.DisP1toLine > answer.DisP2toLine)
            {
                Debug.Log(1);
                 answer.spriteDisplay.transform.localPosition = answer.spritePoint1.transform.localPosition;
                //answer.spriteDisplay.transform.localPosition = new Vector3(answer.spritePoint1.transform.localPosition.x + P1P2.x/2, answer.spritePoint1.transform.localPosition.y + P1P2.y/4, 0);
            }


            answer.spriteDisplay.GetComponent<TweenAlpha>().from = 0f;
            answer.spriteDisplay.GetComponent<TweenAlpha>().to = 0.5f;
            answer.spriteDisplay.GetComponent<TweenAlpha>().PlayForward();
        }

        yield return new WaitForSeconds(2f);

         if (Game.instance != null)  Game.instance.LevelCompleted();

        //if (!second)
        //{
        //    second = true;
        //   // answer.spriteDisplay.GetComponent<TweenPosition>().ResetToBeginning();
        //    Debug.Log(answer.spriteDisplay.transform.position);
        //    answer.spriteDisplay.GetComponent<TweenPosition>().from = answer.spriteDisplay.transform.localPosition;
        //    answer.spriteDisplay.GetComponent<TweenPosition>().to = Vector3.zero;
        //    answer.spriteDisplay.GetComponent<TweenPosition>().PlayForward();

        //    answer.spriteDisplay.GetComponent<TweenScale>().ResetToBeginning();
        //    answer.spriteDisplay.GetComponent<TweenScale>().PlayForward();
        //}

        //yield return new WaitForSeconds(2f);


    }

    public void CalculateDistanceFromPointToLine()
    {

        answer.DisP1toLine = DistancePointAndLine(answer.Point1, slice169.point1, slice169.point2);
        answer.DisP2toLine = DistancePointAndLine(answer.Point2, slice169.point1, slice169.point2);


    }

    private void CheckAnswers()
    {
        if (answer.DisP1toLine <= answer.distanceSlice || answer.DisP2toLine <= answer.distanceSlice)
        {
            if (Vector2.Distance(answer.Point1, answer.Point2) <= answer.distanceWin)
            {
                timerCount += Time.deltaTime;
             //   Debug.Log(timerCount);
              //  if ((timerCount) > 0.5f)
              if ( !isWin)
                {
                    Win();
                }

            }
        }
    }






    private void UpdatePointPos()
    {

        if (answer.DisP1toLine < answer.DisP2toLine)
        {
            answer.Point1 = answer.OldPosPointMove + slice169.VectorMove;
        }
        else
        {
            answer.Point2 = answer.OldPosPointMove + slice169.VectorMove;
        }


    }

    private float DistancePointAndLine(Vector2 point, Vector3 point1, Vector3 point2)
    {
        float distance;

        float x21 = point2.x - point1.x;
        float y21 = point2.y - point1.y;
        distance = Mathf.Abs(y21 * point.x - x21 * point.y - point1.x * y21 + point1.y * x21) / Mathf.Sqrt(y21 * y21 + x21 * x21);
        return distance;
    }
}
