
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class WinCheckOneAnswer : MonoBehaviour
{
    public Slice169 slice169;
    public LineRenderer hintP1;
    public LineRenderer hintP2;

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
        answer.p1MatchedOfPoint1.z = -1;
        answer.p2MatchedOfPoint1.z = -1;
        answer.p1MatchedOfPoint2.z = -1;
        answer.p2MatchedOfPoint2.z = -1;

        hintP1.positionCount = 2;
        hintP1.SetPosition(0, answer.p1MatchedOfPoint1);
        hintP1.SetPosition(1, answer.p2MatchedOfPoint1);

        hintP2.positionCount = 2;
        hintP2.SetPosition(0, answer.p1MatchedOfPoint2);
        hintP2.SetPosition(1, answer.p2MatchedOfPoint2);
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
        Vector2[] testMatch = slice169.GetLineIntersection(answer.point1, answer.angle, slice169.peak3, slice169.peak2, slice169.peak0, slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        answer.p1MatchedOfPoint1 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        answer.p2MatchedOfPoint1 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        testMatch = GetComponent<Slice169>().GetLineIntersection(answer.point2, answer.angle, slice169.peak3, slice169.peak2, slice169.peak0, slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        answer.p1MatchedOfPoint2 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        answer.p2MatchedOfPoint2 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

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
        Debug.Log(1);
        isWin = true;
        StartCoroutine(PlayWinEffect2());

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

         if (Game.Instance != null)  Game.Instance.LevelCompleted();

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
    

    IEnumerator PlayWinEffect2()
    {
        if (!first)
        {
            first = true;

            Vector2 P1P2 = answer.point2 - answer.point1;
            answer.spriteHalfPoint1.gameObject.SetActive(true);
            answer.spriteHalfPoint2.gameObject.SetActive(true);

            TweenColor c = answer.spriteHalfPoint2.AddComponent<TweenColor>();
            c.duration = 1f;
            c.from = Color.black;
            c.to = Color.white;
            c.PlayForward();

            TweenColor c1 = answer.spriteHalfPoint1.AddComponent<TweenColor>();
            c1.duration = 1f;
            c1.from = Color.black;
            c1.to = Color.white;
            c1.PlayForward();

            //answer.spriteHalfPoint2.GetComponent<TweenAlpha>().duration =1f;
            //answer.spriteHalfPoint2.GetComponent<TweenAlpha>().from = 0f;
            //answer.spriteHalfPoint2.GetComponent<TweenAlpha>().to = 1f;
            //answer.spriteHalfPoint2.GetComponent<TweenAlpha>().PlayForward();

            

            //answer.spriteHalfPoint1.gameObject.SetActive(true);
            //answer.spriteHalfPoint1.GetComponent<TweenAlpha>().duration = 1f;
            //answer.spriteHalfPoint1.GetComponent<TweenAlpha>().from = 0f;
            //answer.spriteHalfPoint1.GetComponent<TweenAlpha>().to = 1f;
            //answer.spriteHalfPoint1.GetComponent<TweenAlpha>().PlayForward();

            if (answer.DisP1toLine <= answer.DisP2toLine)
            {
                answer.spriteHalfPoint1.transform.position += new Vector3( slice169.VectorMove.x,slice169.VectorMove.y,0);
                yield return new WaitForSeconds(1f);
                TweenPosition p = answer.spriteHalfPoint1.GetComponent<TweenPosition>();
                p.from = answer.spriteHalfPoint1.transform.localPosition;
                p.to = answer.spriteHalfPoint2.transform.localPosition;
                p.duration = 0.5f;
                p.PlayForward();


            }
            else if (answer.DisP1toLine > answer.DisP2toLine)
            {
                answer.spriteHalfPoint2.transform.position += new Vector3(slice169.VectorMove.x, slice169.VectorMove.y, 0);

                yield return new WaitForSeconds(1f);
                TweenPosition p = answer.spriteHalfPoint2.GetComponent<TweenPosition>();

                p.from = answer.spriteHalfPoint2.transform.localPosition;
                p.to = answer.spriteHalfPoint1.transform.localPosition;
                p.duration = 0.5f;
                p.PlayForward();
            }

           
        }

        yield return new WaitForSeconds(2f);

        if (Game.Instance != null) Game.Instance.LevelCompleted();

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
        if (answer.DisP1toLine <= 1f || answer.DisP2toLine <= 1f)
        {
            if (Vector2.Distance(answer.Point1, answer.Point2) <= 1f)
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

        if (answer.DisP1toLine <= answer.DisP2toLine)
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


    
    public void ShowHint()
    {
        hintP1.enabled = true;
        hintP2.enabled = true;
    }



    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(answer.p1MatchedOfPoint1, 0.5f);
        //Gizmos.DrawSphere(answer.p2MatchedOfPoint1, 0.5f);
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(answer.p1MatchedOfPoint2, 0.5f);
        //Gizmos.DrawSphere(answer.p2MatchedOfPoint2, 0.5f);
    }
}
