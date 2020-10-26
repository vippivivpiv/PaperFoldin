using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WinCheck : MonoBehaviour
{
    public Slice169 slice169;
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
    [Header("Answer1")]
    public string nameOfAns1;
    public Vector2 point1OfAns1;
    public Vector2 point2OfAns1;
    public float angleOfAns1;
    public static bool isFindedAns1;
    public bool isNeedAutoMatch1;
    private Vector3 p1MatchedOfPoint1Ans1;
    private Vector3 p2MatchedOfPoint1Ans1;
    private Vector3 p1MatchedOfPoint2Ans1;
    private Vector3 p2MatchedOfPoint2Ans1;
    [Header("Answer2")]
    public string nameOfAns2;
    public Vector2 point1OfAns2;
    public Vector2 point2OfAns2;
    public float angleOfAns2;
    public static bool isFindedAns2;
    public bool isNeedAutoMatch2;
    private Vector3 p1MatchedOfPoint1Ans2;
    private Vector3 p2MatchedOfPoint1Ans2;
    private Vector3 p1MatchedOfPoint2Ans2;
    private Vector3 p2MatchedOfPoint2Ans2;
    [Header("Answer3")]
    public string nameOfAns3;
    public Vector2 point1OfAns3;
    public Vector2 point2OfAns3;
    public float angleOfAns3;
    public static bool isFindedAns3;
    public bool isNeedAutoMatch3;
    private Vector3 p1MatchedOfPoint1Ans3;
    private Vector3 p2MatchedOfPoint1Ans3;
    private Vector3 p1MatchedOfPoint2Ans3;
    private Vector3 p2MatchedOfPoint2Ans3;
    [Header("Win conditions")]
    public float distanceSlice;
    public float angleMatch;
    public float distanceWin;
    public float holdTime;
    [Header("")]
    public Vector3 point1ImagePos;
    public Vector3 point2ImagePos;
    private float angleCheck;

    private float[] timerCount;

    public GameObject answerDisplay;
    private static ImageProperties[] answer;

    //public static ImageProperties[] answer;
    private bool iscalculator;


    private void Start()
    {
        answer = new ImageProperties[4];
        {
            answer[0] = new ImageProperties(nameOfAns0, point1OfAns0, point2OfAns0);
            answer[1] = new ImageProperties(nameOfAns1, point1OfAns1, point2OfAns1);
            answer[2] = new ImageProperties(nameOfAns2, point1OfAns2, point2OfAns2);
            answer[3] = new ImageProperties(nameOfAns3, point1OfAns3, point2OfAns3);
            //
        };
        timerCount = new float[4]
        {
            0,0,0,0
        };

        CalculatePointAutoMatch();

        //Debug.Log("p1MatchedOfPoint1Ans0: " + p1MatchedOfPoint1Ans0);
        //Debug.Log("p2MatchedOfPoint1Ans0: " + p2MatchedOfPoint1Ans0);
        //Debug.Log("p1MatchedOfPoint1Ans0: " + p1MatchedOfPoint2Ans0);
        //Debug.Log("p2MatchedOfPoint1Ans0: " + p2MatchedOfPoint2Ans0);

        //Debug.Log("p1MatchedOfPoint1Ans1: " + p1MatchedOfPoint1Ans1);
        //Debug.Log("p2MatchedOfPoint1Ans1: " + p2MatchedOfPoint1Ans1);
        //Debug.Log("p1MatchedOfPoint1Ans1: " + p1MatchedOfPoint2Ans1);
        //Debug.Log("p2MatchedOfPoint1Ans1: " + p2MatchedOfPoint2Ans1);

        //Debug.Log("p1MatchedOfPoint1Ans2: " + p1MatchedOfPoint1Ans2);
        //Debug.Log("p2MatchedOfPoint1Ans2: " + p2MatchedOfPoint1Ans2);
        //Debug.Log("p1MatchedOfPoint1Ans2: " + p1MatchedOfPoint2Ans2);
        //Debug.Log("p2MatchedOfPoint1Ans2: " + p2MatchedOfPoint2Ans2);

        //Debug.Log("p1MatchedOfPoint1Ans3: " + p1MatchedOfPoint1Ans3);
        //Debug.Log("p2MatchedOfPoint1Ans3: " + p2MatchedOfPoint1Ans3);
        //Debug.Log("p1MatchedOfPoint1Ans3: " + p1MatchedOfPoint2Ans3);
        //Debug.Log("p2MatchedOfPoint1Ans3: " + p2MatchedOfPoint2Ans3);

    }


    private void Update()
    {
        //Debug.Log("Slice169.VectorMove: " + Slice169.VectorMove);
        if (slice169.isSliced)
        {
            if (!iscalculator)
            {
                iscalculator = true;

                if (angleCheck == 180) angleCheck = 0;

                //CalculateDistanceFromPointToLine();

                //AutoMatchLine();

                SelectPointMoveAndDisplayImage();
            }
            UpdatePointPos();
            CheckAnswers();
        }

        if (isFindedAns0 && isFindedAns1 && isFindedAns2 && isFindedAns3)
        {
            Win();
        }
        //isFindedAns0 = answer[0].IsFinded;
        //isFindedAns1 = answer[1].IsFinded;
        //isFindedAns2 = answer[2].IsFinded;
        //Debug.Log(answer[0].IsFinded);
        //Win();

    }

    public float AutoMatchLine()
    {
        angleCheck = gameObject.GetComponent<Slice169>().AngleSlice();
        if (isNeedAutoMatch0 && (Mathf.Abs(angleCheck - angleOfAns0) < angleMatch || Mathf.Abs(180 - angleCheck - angleOfAns0) < angleMatch))
        {
            Debug.Log("0,1");
            if (answer[0].DisP1toLine < distanceSlice)
            {
                Debug.Log("0,1");
                slice169.point1 = p1MatchedOfPoint1Ans0;
                slice169.point2 = p2MatchedOfPoint1Ans0;
                return angleOfAns0;
            }

            if (answer[0].DisP2toLine < distanceSlice)
            {
                Debug.Log("0,2");
                slice169.point1 = p1MatchedOfPoint2Ans0;
                slice169.point2 = p2MatchedOfPoint2Ans0;
                return angleOfAns0;
            }
        }
        if (isNeedAutoMatch1 && (Mathf.Abs(angleCheck - angleOfAns1) < angleMatch || Mathf.Abs(180 - angleCheck - angleOfAns1) < angleMatch))
        {
            if (answer[1].DisP1toLine < distanceSlice)
            {
                Debug.Log("1,1");
                slice169.point1 = p1MatchedOfPoint1Ans1;
                slice169.point2 = p2MatchedOfPoint1Ans1;
                return angleOfAns1;
            }

            if (answer[1].DisP2toLine < distanceSlice)
            {
                Debug.Log("1,2");
                slice169.point1 = p1MatchedOfPoint2Ans1;
                slice169.point2 = p2MatchedOfPoint2Ans1;
                return angleOfAns1;
            }
        }
        if (isNeedAutoMatch2 && (Mathf.Abs(angleCheck - angleOfAns2) < angleMatch || Mathf.Abs(180 - angleCheck - angleOfAns2) < angleMatch))
        {
            if (answer[2].DisP1toLine < distanceSlice)
            {
                Debug.Log("2,1");
                slice169.point1 = p1MatchedOfPoint1Ans2;
                slice169.point2 = p2MatchedOfPoint1Ans2;
                return angleOfAns2;
            }

            if (answer[2].DisP2toLine < distanceSlice)
            {
                Debug.Log("2,2");
                slice169.point1 = p1MatchedOfPoint2Ans2;
                slice169.point2 = p2MatchedOfPoint2Ans2;
                return angleOfAns2;
            }
        }
        if (isNeedAutoMatch3 && (Mathf.Abs(angleCheck - angleOfAns3) < angleMatch || Mathf.Abs(180 - angleCheck - angleOfAns3) < angleMatch))
        {
            if (answer[3].DisP1toLine < distanceSlice)
            {
                Debug.Log("3,1");
                slice169.point1 = p1MatchedOfPoint1Ans3;
                slice169.point2 = p2MatchedOfPoint1Ans3;
                return angleOfAns3;
            }

            if (answer[3].DisP2toLine < distanceSlice)
            {
                Debug.Log("3,2");
                slice169.point1 = p1MatchedOfPoint2Ans3;
                slice169.point2 = p2MatchedOfPoint2Ans3;
                return angleOfAns3;
            }
        }
        return 12151255;
    }

    private void CalculatePointAutoMatch()
    {
        //Vector2[] testMatch = GetComponent<Slice169>().GetLineIntersection(point1OfAns0, angleOfAns0, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint1Ans0 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint1Ans0 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        //testMatch = GetComponent<Slice169>().GetLineIntersection(point2OfAns0, angleOfAns0, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint2Ans0 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint2Ans0 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        //testMatch = GetComponent<Slice169>().GetLineIntersection(point1OfAns1, angleOfAns1, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint1Ans1 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint1Ans1 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        //testMatch = GetComponent<Slice169>().GetLineIntersection(point2OfAns1, angleOfAns1, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint2Ans1 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint2Ans1 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        //testMatch = GetComponent<Slice169>().GetLineIntersection(point1OfAns2, angleOfAns2, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint1Ans2 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint1Ans2 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        //testMatch = GetComponent<Slice169>().GetLineIntersection(point2OfAns2, angleOfAns2, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint2Ans2 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint2Ans2 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        //testMatch = GetComponent<Slice169>().GetLineIntersection(point1OfAns3, angleOfAns3, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint1Ans3 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint1Ans3 = new Vector3(testMatch[1].x, testMatch[1].y, 0);

        //testMatch = GetComponent<Slice169>().GetLineIntersection(point2OfAns3, angleOfAns3, Slice169.peak3, Slice169.peak2, Slice169.peak0, Slice169.peak1, GetComponent<Slice169>().widthRatio, GetComponent<Slice169>().heightRatio);
        //p1MatchedOfPoint2Ans3 = new Vector3(testMatch[0].x, testMatch[0].y, 0);
        //p2MatchedOfPoint2Ans3 = new Vector3(testMatch[1].x, testMatch[1].y, 0);
    }
    public void SelectPointMoveAndDisplayImage()
    {
        foreach (ImageProperties ans in answer)
        {
            if (ans.DisP1toLine < ans.DisP2toLine)
            {
                ans.OldPosPointMove = ans.Point1;
                ans.DisplayImagePos = ans.Point2ImagePos;
            }
            else
            {
                ans.OldPosPointMove = ans.Point2;
                ans.DisplayImagePos = ans.Point1ImagePos;
            }
        }

    }
    private void Win()
    {
        Debug.Log("Win");
        answerController.isFindAns0 = answerController.isFindAns1 = answerController.isFindAns2 = answerController.isFindAns3 = false;
        isFindedAns0 = isFindedAns1 = isFindedAns2 = isFindedAns3 = false;
        gameObject.GetComponentInParent<GamePlayProcess>().FindedAnswer();
        gameObject.GetComponentInParent<GamePlayProcess>().ClickNextStage();
        //gameObject.GetComponent<Slice169>().enabled = false;
        //gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

    public void CalculateDistanceFromPointToLine()
    {
        for (int i = 0; i < answer.Length; i++)
        {
            answer[i].DisP1toLine = DistancePointAndLine(answer[i].Point1, slice169.point1, slice169.point2);
            answer[i].DisP2toLine = DistancePointAndLine(answer[i].Point2, slice169.point1, slice169.point2);
        }

    }

    private void CheckAnswers()
    {
        for (int i = 0; i < answer.Length; i++)
        {
            //if (Mathf.Abs(angleCheck - answer[i].Angle) <= angleWin)
            {
                if (answer[i].DisP1toLine <= distanceSlice || answer[i].DisP2toLine <= distanceSlice)
                {
                    if (Vector2.Distance(answer[i].Point1, answer[i].Point2) <= distanceWin)
                    {
                        timerCount[0] += Time.deltaTime;
                        Debug.Log(timerCount[0]);
                        if ((timerCount[0]) > holdTime)
                        {
                            if (i == 0)
                            {
                                Debug.Log(">>");
                                isFindedAns0 = true;
                                answerController.isFindAns0 = true;


                            }
                            if (i == 1)
                            {
                                isFindedAns1 = true;
                                answerController.isFindAns1 = true;
                            }
                            if (i == 2)
                            {
                                isFindedAns2 = true;
                                answerController.isFindAns2 = true;
                            }
                            if (i == 3)
                            {
                                isFindedAns0 = true;
                                answerController.isFindAns3 = true;
                            }
                            gameObject.GetComponentInParent<GamePlayProcess>().FindedAnswer();
                            gameObject.GetComponentInParent<GamePlayProcess>().LoadStage();
                        }


                    }

                }
            }
        }




    }

    private void UpdatePointPos()
    {
        foreach (ImageProperties ans in answer)
        {
            if (ans.DisP1toLine < ans.DisP2toLine)
            {
                ans.Point1 = ans.OldPosPointMove + slice169.VectorMove;
            }
            else
            {
                ans.Point2 = ans.OldPosPointMove + slice169.VectorMove;
            }
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
