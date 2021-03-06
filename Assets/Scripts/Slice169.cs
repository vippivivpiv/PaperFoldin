﻿
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Slice169 : MonoBehaviour
{
    #region Properties


    public Camera mainCamera;

    public WinCheckOneAnswer winCheck;

    public MeshRenderer meshRenderer;
    public LineRenderer lineSegment;

    public Transform lineSlicer;
    public Transform startPoint;


    public float widthRatio;
    public float heightRatio;

    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triagles;
    private Vector2[] uvs;
    private Vector3[] verticesOriginal;
    public Vector3[] edgeSliced;


    private Vector3 startPos;
    public Vector2 VectorMove;
    public Vector3 peak0, peak1, peak2, peak3;
    private Vector3 currentPos;
    private Vector3 oldPos;
    // private Vector3 currentPos;
    private Vector3 diffPos;
    private Vector3 diffBetweenCurandOldPos;
    private Vector3 moveDirection;
    private Vector3 mousePos;
    public Vector3 point1, point2;
    public Vector3 oldPoint1, oldPoint2;
    public Vector3 startMovePoint;
    private bool isHitPoint1;
    public Vector3 currentMovePoint;
    public Vector3 diffMovePoint;
    public Transform p1, p2;
    private float disP1, disP2;
    public LineRenderer GiayNhan;
    public LineRenderer EdgeOfSelected;

    private bool isYOver1;
    private bool isYOver2;
    private bool isYOver3;
    private bool isYOver4;

    private float MovedDiffP1;
    private float MovedDiffP2;
    private float MovedDiffP3;
    private float MovedDiffP4;

    private float Moved1;
    private float Moved2;
    private float Moved3;
    private float Moved4;

    private float magDiffP;
    private Vector3 diffP;

    public bool isConfirmSlice;
    public bool isSliced;
    public bool isMoving;
    public bool isClick;
    public bool isClickMovePoint;
    public bool isTutorial;

    private bool isCalculatorDiff;
    private bool isUpdateOldDiff;

    private bool isCheckPart;
    public bool isChoosePart;
    private bool isChooseSmallPart;
    private bool canMove;


    private eCasePos casePos;


    private List<Vector3> smallPartVertices, bigPartVerteces;
    private bool isMovePoint;
    private bool swap;

    #endregion
    private void Start()
    {

        //  lineSegment.gameObject.SetActive(true);

        lineSegment.positionCount = 2;
        lineSlicer.gameObject.SetActive(false);

        peak0 = new Vector3(-widthRatio, -heightRatio, 0);
        peak1 = new Vector3(-widthRatio, heightRatio, 0);
        peak2 = new Vector3(widthRatio, -heightRatio, 0);
        peak3 = new Vector3(widthRatio, heightRatio, 0);

        edgeSliced = new Vector3[0];
        mesh = GetComponent<MeshFilter>().mesh;

        diffBetweenCurandOldPos = new Vector3(0, 0, 0);

        mainCamera = Game.Instance.mainCam;

        isSliced = false;
        isClick = false;

        MakeOriginMeshData();
        CreateMesh();
    }

    private void Update()
    {


        if (!winCheck.isWin)
        {
            if (!isSliced)
            {
                Slice();
            }
            else
            {

                p1.position = point1 + new Vector3(0, 0, -1);

                p2.position = point2 + new Vector3(0, 0, -1);

                if (!isTutorial && DataPlayer.IsControllerPoint)
                {
                    if (!isMovePoint)
                    {
                        MovePoint();

                    }

                    if (isMovePoint)
                    {

                        Move();
                        CreateMesh();
                        UpdateVerOfEdgeSliced();
                    }
                }
                else
                {
                    Move();
                    CreateMesh();
                    UpdateVerOfEdgeSliced();
                }


            }
        }

    }
    public float GetMin(float[] f)
    {
        float min = f[0];
        for (int i = 0; i < f.Length; i++)
        {
            if (f[i] < min)
            {
                min = f[i];
            }
        }

        return min;
    }


    private void MovePoint()
    {
        lineSlicer.gameObject.SetActive(true);
        lineSegment.gameObject.SetActive(false);
        startPoint.gameObject.SetActive(false);
        p1.gameObject.SetActive(true);
        p2.gameObject.SetActive(true);

        EdgeOfSelected.enabled = false;


        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!isClickMovePoint)
            {
                // startMovePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    isClickMovePoint = true;

                    if (hit.collider.tag == "point1")
                    {
                        Debug.Log("Hit point 1");

                        isHitPoint1 = true;
                    }
                    else if (hit.collider.tag == "point2")
                    {
                        isHitPoint1 = false;
                        Debug.Log("Hit point 2");
                    }
                }
                else
                {
                    isClickMovePoint = false;
                    isMovePoint = true;
                }
            }
            else if (isClickMovePoint)
            {
                currentMovePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (currentMovePoint.x > widthRatio) currentMovePoint.x = widthRatio - 0.1f;
                if (currentMovePoint.x < -widthRatio) currentMovePoint.x = -widthRatio + 0.1f;
                if (currentMovePoint.y > heightRatio) currentMovePoint.y = heightRatio - 0.1f;
                if (currentMovePoint.y < -heightRatio) currentMovePoint.y = -heightRatio + 0.1f;


                if (isHitPoint1)
                {

                    if (Math.Abs(oldPoint1.x - widthRatio) < 0.01f || Math.Abs(oldPoint1.x + widthRatio) < 0.01f)
                    {
                        point1 = new Vector3(oldPoint1.x, currentMovePoint.y, 0);
                    }
                    else if (Math.Abs(oldPoint1.y - heightRatio) < 0.01f || Math.Abs(oldPoint1.y + heightRatio) < 0.01f)
                    {
                        point1 = new Vector3(currentMovePoint.x, oldPoint1.y, 0);
                    }
                }
                else
                {

                    if (Math.Abs(oldPoint2.x - widthRatio) < 0.01f || Math.Abs(oldPoint2.x + widthRatio) < 0.01f)
                    {
                        point2 = new Vector3(oldPoint2.x, currentMovePoint.y, 0);
                    }
                    else if (Math.Abs(oldPoint2.y - heightRatio) < 0.01f || Math.Abs(oldPoint2.y + heightRatio) < 0.01f)
                    {
                        point2 = new Vector3(currentMovePoint.x, oldPoint2.y, 0);
                    }
                }
                UpdateLineSlicer();
            }

        }


        if (Input.GetMouseButtonUp(0) & isClickMovePoint)
        {
            isClickMovePoint = false;

            UpdateLineSlicer();


            ClassSlicePosition();
            SortP1P2();
            CalculateDistance();
            UpdateMeshDataAfterSlice();
            UpdatePositionMoveVertercies();

            oldPoint1 = point1;
            oldPoint2 = point2;

            verticesOriginal = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                verticesOriginal[i] = vertices[i];
            }
        }



    }

    private void SortP1P2()
    {

        if (Vector3.SignedAngle(point2 - Vector3.zero, point1 - Vector3.zero, Vector3.forward) < 0f) swapP1P2();
    }

    //----------------------------------------Gameplay-----------------------------------------------------------------------------------------------------------------------------------------------
    #region Gameplay
    private void Slice()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        if (isTutorial)
        {
            if (Mathf.Abs(winCheck.answer.p1MatchedOfPoint2.y - mousePos.y) < 1f)
            {
                mousePos.y = winCheck.answer.p1MatchedOfPoint2.y;
            }
            else return;

        }

        if (Input.GetMouseButton(0))
        {
            if (!isClick)
            {
                if (Mathf.Abs(mousePos.x) > widthRatio || Mathf.Abs(mousePos.y) > heightRatio) return;
                Game.Instance.PlayFXSlice();

                isClick = true;

                startPoint.position = startPos = mousePos;
                startPoint.gameObject.SetActive(true);

                lineSegment.SetPosition(0, new Vector3(startPos.x, startPos.y, 0f));

            }
            else
            {
                currentPos = mousePos;


                lineSegment.SetPosition(1, new Vector3(currentPos.x, currentPos.y, 0f));
                lineSegment.gameObject.SetActive(true);

                diffPos = currentPos - startPos;

                Vector2[] lineIntersection = GetLineIntersection(startPos, currentPos, peak3, peak2, peak0, peak1, widthRatio, heightRatio);
                point1 = new Vector3(lineIntersection[0].x, lineIntersection[0].y, 0);
                point2 = new Vector3(lineIntersection[1].x, lineIntersection[1].y, 0);

                UpdateLineSlicer();
            }
        }
        if (Input.GetMouseButtonUp(0) && isClick)
        {
            if (diffPos.magnitude == 0) return;

            isClick = false;
            isSliced = true;

            lineSlicer.gameObject.SetActive(true);
            lineSegment.gameObject.SetActive(false);
            startPoint.gameObject.SetActive(false);
            p1.gameObject.SetActive(true);
            p2.gameObject.SetActive(true);

            if (DataPlayer.IsAutoMatch) CheckAutoMatch();



            ClassSlicePosition();
            SortP1P2();
            CalculateDistance();
            UpdateMeshDataAfterSlice();
            UpdatePositionMoveVertercies();

            oldPoint1 = point1;
            oldPoint2 = point2;

            verticesOriginal = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                verticesOriginal[i] = vertices[i];
            }
        }
    }

    private void CheckAutoMatch()
    {
        Debug.Log(point1);
        Vector3 point1Clone = point1;
        Vector3 point2Clone = point2;
        if (point1Clone.y > point2Clone.y)
        {
            Vector3 t = point1Clone;
            point1Clone = point2Clone;
            point2Clone = t;
        }
        float angle = -Vector3.SignedAngle(point2Clone - point1Clone, Vector3.right, Vector3.forward);
        winCheck.CalculateDistanceFromPointToLine();


        if (winCheck.answer.DisP1toLine < 0.5f)
        {
            if ((Math.Abs(angle - winCheck.answer.angle) < 5f) || (Math.Abs(angle - 180 - winCheck.answer.angle) < 5f) || (Math.Abs(angle + 180 - winCheck.answer.angle) < 5f))
            {

                point1 = winCheck.answer.p1MatchedOfPoint1;
                point2 = winCheck.answer.p2MatchedOfPoint1;

                UpdateLineSlicer();
            }

        }

        if (winCheck.answer.DisP2toLine < 0.5f)
        {
            if ((Math.Abs(angle - winCheck.answer.angle) < 5f) || (Math.Abs(angle - 180 - winCheck.answer.angle) < 5f) || (Math.Abs(angle + 180 - winCheck.answer.angle) < 5f))
            {

                point1 = winCheck.answer.p1MatchedOfPoint2;
                point2 = winCheck.answer.p2MatchedOfPoint2;

                UpdateLineSlicer();
            }
        }

        point1.z = 0;
        point2.z = 0;

    }

    private void UpdatePositionEdgeOfSelected()
    {

    }

    public void DisplayGiayNhan(Vector3 p1, Vector3 p2)
    {
        GiayNhan.gameObject.SetActive(true);
        GiayNhan.SetPosition(0, p1);
        GiayNhan.SetPosition(1, p2);
    }
    private void UpdatePositionMoveVertercies()
    {
        smallPartVertices = new List<Vector3>();
        smallPartVertices.Add(vertices[4]);
        smallPartVertices.Add(vertices[5]);
        bigPartVerteces = new List<Vector3>();
        bigPartVerteces.Add(vertices[6]);
        bigPartVerteces.Add(vertices[7]);
        switch (casePos)
        {
            case eCasePos.TH1:
                smallPartVertices.Add(vertices[0]);
                bigPartVerteces.Add(vertices[1]);
                bigPartVerteces.Add(vertices[3]);
                bigPartVerteces.Add(vertices[2]);
                break;
            case eCasePos.TH2:
                smallPartVertices.Add(vertices[1]);
                bigPartVerteces.Add(vertices[3]);
                bigPartVerteces.Add(vertices[2]);
                bigPartVerteces.Add(vertices[0]);
                break;
            case eCasePos.TH3:
                smallPartVertices.Add(vertices[3]);
                bigPartVerteces.Add(vertices[2]);
                bigPartVerteces.Add(vertices[0]);
                bigPartVerteces.Add(vertices[1]);
                break;
            case eCasePos.TH4:
                smallPartVertices.Add(vertices[2]);
                bigPartVerteces.Add(vertices[0]);
                bigPartVerteces.Add(vertices[1]);
                bigPartVerteces.Add(vertices[3]);
                break;
            case eCasePos.TH5:
                smallPartVertices.Add(vertices[1]);
                smallPartVertices.Add(vertices[0]);
                bigPartVerteces.Add(vertices[3]);
                bigPartVerteces.Add(vertices[2]);
                break;
            case eCasePos.TH6:
                smallPartVertices.Add(vertices[3]);
                smallPartVertices.Add(vertices[1]);
                bigPartVerteces.Add(vertices[2]);
                bigPartVerteces.Add(vertices[0]);
                break;
            case eCasePos.TH7:
                smallPartVertices.Add(vertices[2]);
                smallPartVertices.Add(vertices[3]);
                bigPartVerteces.Add(vertices[0]);
                bigPartVerteces.Add(vertices[1]);
                break;
            case eCasePos.TH8:
                smallPartVertices.Add(vertices[0]);
                smallPartVertices.Add(vertices[2]);
                bigPartVerteces.Add(vertices[1]);
                bigPartVerteces.Add(vertices[3]);
                break;
        }
    }
    bool CheckRelationshipOfPointAndSmallPolygons()
    {
        UpdatePositionMoveVertercies();
        float angle = 0;
        mousePos.z = 0;
        for (int i = 0; i < smallPartVertices.Count - 1; i++)
        {

            angle += Vector3.Angle(smallPartVertices[i + 1] - mousePos, smallPartVertices[i] - mousePos);

        }
        angle += Vector3.Angle(smallPartVertices[0] - mousePos, smallPartVertices[smallPartVertices.Count - 1] - mousePos);

        return (angle > 359 && angle < 361) ? true : false;
    }

    bool CheckRelationshiOfPointAndBIgPolygons()
    {
        UpdatePositionMoveVertercies();
        float angle = 0;
        mousePos.z = 0;
        for (int i = 0; i < bigPartVerteces.Count - 1; i++)
        {
            angle += Vector3.Angle(bigPartVerteces[i + 1] - mousePos, bigPartVerteces[i] - mousePos);
        }
        angle += Vector3.Angle(bigPartVerteces[0] - mousePos, bigPartVerteces[bigPartVerteces.Count - 1] - mousePos);

        return (angle > 359 && angle < 361) ? true : false;
    }

    private void DisplayLineSlicer(float angle)
    {
        if (angle == 12151255) return;

        float dist = Vector2.Distance(point1, point2);


        lineSlicer.GetChild(0).localEulerAngles = new Vector3(0, 0, angle);


        lineSlicer.GetChild(0).position = (point1 + point2) / 2;


        lineSlicer.GetChild(0).localScale = new Vector3(dist, 0.1f, dist);
        lineSlicer.GetChild(0).transform.position = new Vector3(lineSlicer.GetChild(0).transform.position.x, lineSlicer.GetChild(0).transform.position.y, -1.1f);
        lineSlicer.gameObject.SetActive(true);
    }

    public void UpdateLineSlicer()
    {

        float dist = Vector2.Distance(point1, point2);
        diffPos = point1 - point2;
        lineSlicer.GetChild(0).localEulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(diffPos, Vector2.right));
        lineSlicer.GetChild(0).position = (point1 + point2) / 2;
        lineSlicer.GetChild(0).localScale = new Vector3(dist, 0.1f, 0.1f);
        lineSlicer.GetChild(0).transform.position = new Vector3(lineSlicer.GetChild(0).transform.position.x, lineSlicer.GetChild(0).transform.position.y, 0f);
    }
    private void Move()
    {


        moveDirection = new Vector3(lineSlicer.GetChild(0).transform.up.x, lineSlicer.GetChild(0).transform.up.y, 0);
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (DataPlayer.IsControllerPoint) lineSlicer.gameObject.SetActive(false);

        //   lineSlicer.gameObject.SetActive(false);
        lineSegment.gameObject.SetActive(false);
        startPoint.gameObject.SetActive(false);
        p1.gameObject.SetActive(false);
        p2.gameObject.SetActive(false);


        if (Input.GetMouseButtonDown(0))
        {
            lineSlicer.gameObject.SetActive(false);
            EdgeOfSelected.enabled = true;

            if (!isChoosePart)
            {

                isChoosePart = true;
                if (CheckRelationshipOfPointAndSmallPolygons()) isChooseSmallPart = true;
                else if (CheckRelationshiOfPointAndBIgPolygons()) isChooseSmallPart = false;
                else
                {
                    isChoosePart = false;
                }

            }
        }
        Debug.Log(isChoosePart);
        if (!isChoosePart)
        {
            lineSlicer.gameObject.SetActive(true);
            EdgeOfSelected.enabled = false;
            return;
        }

        if (!isCheckPart && Input.GetMouseButtonDown(0))
        {
            isCheckPart = true;
            if (isChooseSmallPart && !CheckRelationshipOfPointAndSmallPolygons())
            {
                canMove = false;
            }
            else if (isChooseSmallPart && CheckRelationshipOfPointAndSmallPolygons())
            {
                canMove = true;
            }

            if (!isChooseSmallPart && !CheckRelationshiOfPointAndBIgPolygons())
            {
                canMove = false;
            }
            else if (!isChooseSmallPart && CheckRelationshiOfPointAndBIgPolygons())
            {
                canMove = true;
            }
        }

        if (Input.GetMouseButtonUp(0)) isCheckPart = false;


        if (!canMove) return;
        GiayNhan.gameObject.SetActive(false);

        if ( Input.GetMouseButtonDown(0)|| Input.GetMouseButtonUp(0))
        {
            Game.Instance.PlayFXMovePaper();
        }

        switch (casePos)
        {
            //--------------------------------------------------------TH1------------------------------------------------------------------------------------
            case eCasePos.TH1:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {

                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;

                            startPos = mousePos;
                            startPos.z = 0;

                        }
                        else
                        {

                            currentPos = mousePos;
                            currentPos.z = 0;


                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diffPos = currentPos - startPos;


                            magDiffP = diffPos.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diffPos)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));


                            if (!isUpdateOldDiff)
                            {
                                Debug.Log(1);
                                Moved1 = 2 * widthRatio * disP2 / disP1 - disP2; // y
                                MovedDiffP1 = 2 * (2 * widthRatio - disP1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * heightRatio * disP1 / disP2 - disP1; // x
                                MovedDiffP2 = 2 * (2 * heightRatio - disP2) * Mathf.Sin(angleA * Mathf.PI / 180);

                            }
                            isUpdateOldDiff = true;



                            if (isChooseSmallPart)
                            {
                                if (diffP.x < 0)
                                {

                                    startPos = currentPos;
                                    return;

                                }


                                if (IsPeakOver1(verticesOriginal[0]))
                                {



                                    return;
                                }



                                vertices[0] = verticesOriginal[0] + diffP;
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;


                                if (vertices[6].x > widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[2] = peak2;
                                        isYOver1 = true;
                                    }
                                    vertices[6] = peak2 + (magDiffP - MovedDiffP1) * moveDirection;
                                    vertices[2] = verticesOriginal[2] + new Vector3(0, moveY - Moved1, 0);

                                    if (vertices[2].y < -heightRatio)
                                    {
                                        vertices[2].y = -heightRatio;
                                    }

                                    if (vertices[6].y < -heightRatio)
                                    {
                                        vertices[6].y = -heightRatio;
                                    }

                                }
                                else
                                {

                                    if (isYOver1)
                                    {
                                        vertices[2] = new Vector3(peak2.x - 0.001f, peak2.y, 0);
                                        isYOver1 = false;
                                    }

                                    vertices[6] = verticesOriginal[6] + new Vector3(moveX, 0, 0);

                                    if (vertices[6].x > widthRatio) vertices[6].x = widthRatio + 0.1f;
                                }

                                if (vertices[7].y > heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[1] = peak1;
                                        isYOver2 = true;

                                    }
                                    vertices[7] = peak1 + (magDiffP - MovedDiffP2) * moveDirection;
                                    vertices[1] = verticesOriginal[1] + new Vector3(moveX - Moved2, 0, 0);


                                    if (vertices[1].x < -widthRatio)
                                    {
                                        vertices[1].x = -widthRatio;
                                    }

                                    if (vertices[7].x < -widthRatio)
                                    {
                                        vertices[7].x = -widthRatio;
                                    }
                                }
                                else
                                {

                                    if (isYOver2)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y - 0.001f, 0);
                                        isYOver2 = false;
                                    }

                                    vertices[7] = verticesOriginal[7] + new Vector3(0, moveY, 0);

                                    if (vertices[7].y > heightRatio) vertices[7].y = heightRatio + 0.1f;
                                }




                                UpdateVerticesBackSide();

                                // Update Mesh
                                if (vertices[7].y > heightRatio && vertices[6].x > widthRatio) UpdateMeshDataWhenOverPeak(3);
                                else if (vertices[7].y > heightRatio) UpdateMeshDataWhenOverPeak(2);
                                else if (vertices[6].x > widthRatio) UpdateMeshDataWhenOverPeak(1);
                                else UpdateMeshDataWhenOverPeak(0);


                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[1] = VerToUv(vertices[1]);
                                uvs[2] = VerToUv(vertices[2]);



                            }

                            else
                            {
                                if (diffP.x > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }

                                if (IsPeakOver1(verticesOriginal[3])) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[1] = verticesOriginal[1] + diffP;

                                vertices[2] = verticesOriginal[2] + diffP;
                                vertices[3] = verticesOriginal[3] + diffP;
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOriginal[4] + new Vector3(moveX, 0, 0);
                                vertices[9] = vertices[5] = verticesOriginal[5] + new Vector3(0, moveY, 0);
                                vertices[8] = vertices[4];
                                vertices[9] = vertices[5];
                                //uv
                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                            }

                        }
                    }
                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {

                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;
                    }
                    break;
                }
            //--------------------------------------------------------TH2------------------------------------------------------------------------------------
            case eCasePos.TH2:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;

                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            startPos.z = 0;

                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diffPos = currentPos - startPos;

                            magDiffP = diffPos.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diffPos)));
                            diffP = magDiffP * moveDirection;
                            float angleA = Vector2.SignedAngle(Vector2.down, lineSlicer.GetChild(0).transform.up); // change
                            float moveY = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // change
                            float moveX = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // change

                            if (!isUpdateOldDiff) // change
                            {

                                Moved1 = 2 * heightRatio * disP2 / disP1 - disP2; // x
                                MovedDiffP1 = 2 * (2 * heightRatio - disP1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * widthRatio * disP1 / disP2 - disP1; // y
                                MovedDiffP2 = 2 * (2 * widthRatio - disP2) * Mathf.Sin(angleA * Mathf.PI / 180);


                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart) // Phần Cắt bé
                            {
                                // kiểm tra điều kiện các góc ko cho kéo ngược hướng
                                if (diffP.x < 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                if (IsPeakOver1(verticesOriginal[1])) return;

                                vertices[1] = verticesOriginal[1] + diffP; // change
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;

                                // kiểm tra xem 6 có đi quá không : TH1
                                if (vertices[6].y < -heightRatio)
                                {

                                    if (!isYOver1)
                                    {

                                        vertices[0] = peak0; // change
                                        isYOver1 = true;

                                    }
                                    vertices[6] = peak0 + (magDiffP - MovedDiffP1) * moveDirection; // change
                                    vertices[0] = verticesOriginal[0] + new Vector3(moveX - Moved1, 0, 0); // change

                                    if (vertices[6].x < -widthRatio)
                                    {
                                        vertices[6].x = -widthRatio;
                                    }

                                    if (vertices[0].x < -widthRatio)
                                    {
                                        vertices[0].x = -widthRatio;
                                    }
                                }
                                else
                                {

                                    if (isYOver1)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y + 0.001f, 0); // change
                                        isYOver1 = false;
                                    }

                                    vertices[6] = verticesOriginal[6] - new Vector3(0, moveY, 0);  // change

                                    if (vertices[6].y < -heightRatio) vertices[6].y = -heightRatio - 0.1f;
                                }


                                // kiểm tra xem 7 có đi quá ko : TH2
                                if (vertices[7].x > widthRatio)
                                {
                                    if (!isYOver2)
                                    {

                                        vertices[3] = peak3; // change
                                        isYOver2 = true;

                                    }
                                    vertices[7] = peak3 + (magDiffP - MovedDiffP2) * moveDirection; // change
                                    vertices[3] = verticesOriginal[3] - new Vector3(0, moveY - Moved2, 0);

                                    if (vertices[3].y > heightRatio)
                                    {
                                        vertices[3].y = heightRatio;
                                    }

                                    if (vertices[7].y > heightRatio)
                                    {
                                        vertices[7].y = heightRatio;
                                    }



                                }

                                else
                                {

                                    if (isYOver2)
                                    {
                                        vertices[3] = new Vector3(peak3.x - 0.001f, peak3.y, 0); // change
                                        isYOver2 = false;
                                        //oldMoveY = moveY;
                                    }
                                    vertices[7] = verticesOriginal[7] + new Vector3(moveX, 0, 0); // change

                                    if (vertices[7].x > widthRatio) vertices[7].x = widthRatio + 0.1f;
                                }

                                // Update Mesh
                                if (vertices[7].x > widthRatio && vertices[6].y < -heightRatio) UpdateMeshDataWhenOverPeak(3);
                                // change
                                else if (vertices[7].x > widthRatio) UpdateMeshDataWhenOverPeak(2);
                                else if (vertices[6].y < -heightRatio) UpdateMeshDataWhenOverPeak(1);
                                else UpdateMeshDataWhenOverPeak(0);


                                // Update Mặt sau !!
                                UpdateVerticesBackSide();

                                ////uv khi Kéo dãn
                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[0] = VerToUv(vertices[0]); // change
                                uvs[3] = VerToUv(vertices[3]); // change


                            }

                            else // Phần Cắt To
                            {
                                // Test xem có kéo ngược hướng không
                                if (diffP.x > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                if (IsPeakOver1(verticesOriginal[2])) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[0] = verticesOriginal[0] + diffP;
                                vertices[2] = verticesOriginal[2] + diffP;
                                vertices[3] = verticesOriginal[3] + diffP;
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOriginal[4] - new Vector3(0, moveY, 0);
                                vertices[9] = vertices[5] = verticesOriginal[5] + new Vector3(moveX, 0, 0);
                                vertices[8] = vertices[4];
                                vertices[9] = vertices[5];
                                //uv
                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                            }

                        }
                    }

                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {
                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;
                    }
                    break;
                }
            //---------------------------------------------------------TH3-----------------------------------------------------------------------------------
            case eCasePos.TH3:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;

                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            startPos.z = 0;
                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;
                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            Vector3 diff = currentPos - startPos;

                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = 180 + Vector2.SignedAngle(Vector2.left, lineSlicer.GetChild(0).transform.up); // change
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // change
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // change

                            if (!isUpdateOldDiff) // change
                            {

                                Moved1 = 2 * widthRatio * disP2 / disP1 - disP2; // y
                                MovedDiffP1 = 2 * (2 * widthRatio - disP1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * heightRatio * disP1 / disP2 - disP1; // x
                                MovedDiffP2 = 2 * (2 * heightRatio - disP2) * Mathf.Sin(angleA * Mathf.PI / 180);



                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart) // Phần Cắt bé
                            {
                                // kiểm tra điều kiện các góc ko cho kéo ngược hướng
                                if (diffP.x > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                if (IsPeakOver1(verticesOriginal[3])) return;

                                vertices[3] = verticesOriginal[3] + diffP; // change
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;

                                // kiểm tra xem 6 có đi quá không : TH1
                                if (vertices[6].x < -widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[1] = peak1; // change
                                        isYOver1 = true;

                                    }
                                    vertices[6] = peak1 + (magDiffP + MovedDiffP1) * moveDirection; // change
                                    vertices[1] = verticesOriginal[1] + new Vector3(0, moveY + Moved1, 0); // change


                                    if (vertices[1].y > heightRatio)
                                    {
                                        vertices[1].y = heightRatio;
                                    }

                                    if (vertices[6].y > heightRatio)
                                    {
                                        vertices[6].y = heightRatio;
                                    }

                                }
                                else
                                {

                                    if (isYOver1)
                                    {
                                        vertices[1] = new Vector3(peak1.x + 0.001f, peak1.y, 0); // change
                                        isYOver1 = false;
                                    }

                                    vertices[6] = verticesOriginal[6] + new Vector3(moveX, 0, 0);  // change
                                    if (vertices[6].x < -widthRatio) vertices[6].x = -widthRatio - 0.1f;
                                }


                                // kiểm tra xem 7 có đi quá ko : TH2
                                if (vertices[7].y < -heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[2] = peak2; // change
                                        isYOver2 = true;

                                    }
                                    vertices[7] = peak2 + (magDiffP + MovedDiffP2) * moveDirection; // change
                                    vertices[2] = verticesOriginal[2] + new Vector3(moveX + Moved2, 0, 0);


                                    if (vertices[2].x > widthRatio)
                                    {
                                        vertices[2].x = widthRatio;
                                    }

                                    if (vertices[7].x > widthRatio)
                                    {
                                        vertices[7].x = widthRatio;
                                    }
                                }

                                else
                                {

                                    if (isYOver2)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y + 0.001f, 0); // change
                                        isYOver2 = false;
                                        //oldMoveY = moveY;
                                    }
                                    vertices[7] = verticesOriginal[7] + new Vector3(0, moveY, 0); // change

                                    if (vertices[7].y > heightRatio) vertices[7].y = heightRatio + 0.1f;

                                }

                                // Update Mesh
                                if (vertices[7].y < -heightRatio && vertices[6].x < -widthRatio) UpdateMeshDataWhenOverPeak(3); // change
                                else if (vertices[7].y < -heightRatio) UpdateMeshDataWhenOverPeak(2);
                                else if (vertices[6].x < -widthRatio) UpdateMeshDataWhenOverPeak(1);
                                else UpdateMeshDataWhenOverPeak(0);


                                // Update Mặt sau !!
                                UpdateVerticesBackSide();

                                ////uv khi Kéo dãn
                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[1] = VerToUv(vertices[1]);
                                uvs[2] = VerToUv(vertices[2]);


                            }

                            else // Phần Cắt To
                            {
                                // Test xem có kéo ngược hướng không
                                if (diffP.x < 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                if (IsPeakOver1(verticesOriginal[0])) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[0] = verticesOriginal[0] + diffP;
                                vertices[2] = verticesOriginal[2] + diffP;
                                vertices[1] = verticesOriginal[1] + diffP;
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOriginal[4] + new Vector3(moveX, 0, 0);
                                vertices[9] = vertices[5] = verticesOriginal[5] + new Vector3(0, moveY, 0);
                                vertices[8] = vertices[4];
                                vertices[9] = vertices[5];
                                //uv
                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                            }

                        }
                    }

                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {
                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;
                    }
                    break;
                }
            //---------------------------------------------------------TH4-----------------------------------------------------------------------------------
            case eCasePos.TH4:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;

                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            startPos.z = 0;
                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;
                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            Vector3 diff = currentPos - startPos;

                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = 180 - Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.up); // change
                            float moveY = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // change
                            float moveX = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // change

                            if (!isUpdateOldDiff) // change
                            {

                                Moved1 = 2 * heightRatio * disP2 / disP1 - disP2; // x
                                MovedDiffP1 = 2 * (2 * heightRatio - disP1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * widthRatio * disP1 / disP2 - disP1; // y
                                MovedDiffP2 = 2 * (2 * widthRatio - disP2) * Mathf.Sin(angleA * Mathf.PI / 180);


                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart) // Phần Cắt bé
                            {
                                // kiểm tra điều kiện các góc ko cho kéo ngược hướng
                                if (diffP.x > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                if (IsPeakOver1(verticesOriginal[2])) return;

                                vertices[2] = verticesOriginal[2] + diffP; // change
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;

                                // kiểm tra xem 6 có đi quá không : TH1
                                if (vertices[6].y > heightRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[3] = peak3; // change
                                        isYOver1 = true;

                                    }
                                    vertices[6] = peak3 + (magDiffP + MovedDiffP1) * moveDirection; // change
                                    vertices[3] = verticesOriginal[3] + new Vector3(moveX + Moved1, 0, 0); // change


                                    if (vertices[6].x > widthRatio)
                                    {
                                        vertices[6].x = widthRatio;
                                    }

                                    if (vertices[3].x > widthRatio)
                                    {
                                        vertices[3].x = widthRatio;
                                    }
                                }
                                else
                                {

                                    if (isYOver1)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y - 0.001f, 0); // change
                                        isYOver1 = false;
                                    }

                                    vertices[6] = verticesOriginal[6] - new Vector3(0, moveY, 0);  // change
                                    if (vertices[6].y > heightRatio) vertices[6].y = heightRatio + 0.1f;

                                }


                                // kiểm tra xem 7 có đi quá ko : TH2
                                if (vertices[7].x < -widthRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[0] = peak0; // change
                                        isYOver2 = true;

                                    }
                                    vertices[7] = peak0 + (magDiffP + MovedDiffP2) * moveDirection; // change
                                    vertices[0] = verticesOriginal[0] - new Vector3(0, moveY + Moved2, 0);

                                    if (vertices[0].y < -heightRatio)
                                    {
                                        vertices[0].y = -heightRatio;
                                    }

                                    if (vertices[7].y < -heightRatio)
                                    {
                                        vertices[7].y = -heightRatio;
                                    }
                                }

                                else
                                {
                                    if (isYOver2)
                                    {
                                        vertices[0] = new Vector3(peak0.x + 0.001f, peak0.y, 0); // change
                                        isYOver2 = false;
                                        //oldMoveY = moveY;
                                    }
                                    vertices[7] = verticesOriginal[7] + new Vector3(moveX, 0, 0); // change

                                    if (vertices[7].x < -widthRatio) vertices[7].x = -widthRatio - 0.1f;
                                }

                                // Update Mesh
                                if (vertices[7].x < -widthRatio && vertices[6].y > heightRatio) UpdateMeshDataWhenOverPeak(3); // change
                                else if (vertices[7].x < -widthRatio) UpdateMeshDataWhenOverPeak(2);
                                else if (vertices[6].y > heightRatio) UpdateMeshDataWhenOverPeak(1);
                                else UpdateMeshDataWhenOverPeak(0);


                                // Update Mặt sau !!
                                UpdateVerticesBackSide();

                                ////uv khi Kéo dãn
                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[0] = VerToUv(vertices[0]); // change
                                uvs[3] = VerToUv(vertices[3]); // change

                            }

                            else // Phần Cắt To
                            {
                                // Test xem có kéo ngược hướng không
                                if (diffP.x < 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                if (IsPeakOver1(verticesOriginal[1])) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[0] = verticesOriginal[0] + diffP;
                                vertices[1] = verticesOriginal[1] + diffP;
                                vertices[3] = verticesOriginal[3] + diffP;
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOriginal[4] - new Vector3(0, moveY, 0);
                                vertices[9] = vertices[5] = verticesOriginal[5] + new Vector3(moveX, 0, 0);
                                vertices[8] = vertices[4];
                                vertices[9] = vertices[5];
                                //uv
                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                            }

                        }
                    }

                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {
                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;
                    }
                    break;
                }
            //---------------------------------------------------------TH5-----------------------------------------------------------------------------------
            case eCasePos.TH5:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;


                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            startPos.z = 0;
                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;
                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;

                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diffPos = currentPos - startPos;


                            magDiffP = diffPos.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diffPos)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));


                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * widthRatio - disP2) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP2 = 2 * (2 * widthRatio - disP1) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP3 = 2 * (disP1) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP4 = 2 * (disP2) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y


                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart)
                            {
                                UpdateMeshDataWhenClickSmallerPart();
                                if (diffP.x < 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }

                                //       if (IsPeakOver1(verticesOld[1], true,false)) return; // change
                                //       if (IsPeakOver1(verticesOld[0], true, false)) return; // change
                                Vector3 newPos6 = verticesOriginal[6] + new Vector3(moveX, 0, 0);
                                Vector3 newPos7 = verticesOriginal[7] + new Vector3(moveX, 0, 0);
                                // if (newPos6.x > widthRatio || newPos7.x > widthRatio) return;
                                if (newPos7.x > widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[3] = peak3;
                                        isYOver1 = true;
                                    }
                                    Vector3 test = verticesOriginal[3] + new Vector3(0, moveY - Moved1, 0);
                                    if (test.y < -heightRatio) return;
                                    if (newPos6.x > widthRatio) return;
                                    vertices[6] = verticesOriginal[6] + new Vector3(moveX, 0, 0);
                                    vertices[7] = peak3 + (magDiffP - MovedDiffP1) * moveDirection;
                                    vertices[3] = verticesOriginal[3] + new Vector3(0, moveY - Moved1, 0);

                                }
                                else if (newPos6.x > widthRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[2] = peak2;
                                        isYOver2 = true;
                                    }
                                    Vector3 new2 = verticesOriginal[2] + new Vector3(0, moveY - Moved2, 0);
                                    if (new2.y > heightRatio)
                                    {

                                        return;
                                    }
                                    if (newPos7.x > widthRatio)
                                    {

                                        return;

                                    }
                                    vertices[7] = verticesOriginal[7] + new Vector3(moveX, 0, 0);

                                    vertices[6] = peak2 + (magDiffP - MovedDiffP2) * moveDirection;
                                    vertices[2] = verticesOriginal[2] + new Vector3(0, moveY - Moved2, 0);
                                }

                                else
                                {

                                    if (isYOver1)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y - 0.001f, 0);
                                        isYOver1 = false;
                                    }

                                    if (isYOver2)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y - 0.001f, 0);
                                        isYOver2 = false;
                                    }
                                    vertices[7] = verticesOriginal[7] + new Vector3(moveX, 0, 0);
                                    vertices[6] = verticesOriginal[6] + new Vector3(moveX, 0, 0);
                                }

                                vertices[0] = verticesOriginal[0] + diffP; // change
                                vertices[1] = verticesOriginal[1] + diffP; // change
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;


                                UpdateVerticesBackSide();

                                // Update Mesh
                                if (vertices[7].x > widthRatio) { UpdateMeshDataWhenOverPeak(1); }
                                else if (vertices[6].x > widthRatio) { UpdateMeshDataWhenOverPeak(2); }
                                else { UpdateMeshDataWhenClickSmallerPart(); }

                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[3] = VerToUv(vertices[3]);
                                uvs[2] = VerToUv(vertices[2]);

                            }
                            else
                            {
                                UpdateMeshDataWhenChooseBiggerPart();
                                if (diffP.x > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                //  if (IsPeakOver1(verticesOld[3], true, false)) return; // change
                                //  if (IsPeakOver1(verticesOld[2], true, false)) return; // change
                                Vector3 newPos4 = verticesOriginal[4] + new Vector3(moveX, 0, 0);
                                Vector3 newPos5 = verticesOriginal[5] + new Vector3(moveX, 0, 0);
                                //if (newPos4.x < -widthRatio || newPos5.x < -widthRatio) return;


                                if (newPos4.x < -widthRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[0] = peak0;
                                        isYOver3 = true;
                                    }

                                    Vector3 new0 = verticesOriginal[0] + new Vector3(0, moveY + Moved3, 0);
                                    if (new0.y > heightRatio) return;
                                    if (newPos5.x < -widthRatio) return;
                                    vertices[0] = verticesOriginal[0] + new Vector3(0, moveY + Moved3, 0);


                                    vertices[5] = verticesOriginal[5] + new Vector3(moveX, 0, 0);

                                    vertices[4] = peak0 + (magDiffP + MovedDiffP3) * moveDirection;

                                }
                                else if (newPos5.x < -widthRatio)
                                {
                                    if (!isYOver4)
                                    {
                                        vertices[1] = peak1;
                                        isYOver4 = true;
                                    }
                                    Vector3 new1 = verticesOriginal[1] + new Vector3(0, moveY + Moved4, 0);
                                    if (new1.y < -heightRatio) return;
                                    if (newPos4.x < -widthRatio) return;

                                    vertices[4] = verticesOriginal[4] + new Vector3(moveX, 0, 0);

                                    vertices[5] = peak1 + (magDiffP + MovedDiffP4) * moveDirection;
                                    vertices[1] = verticesOriginal[1] + new Vector3(0, moveY + Moved4, 0);

                                }
                                else
                                {

                                    if (isYOver3)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }
                                    if (isYOver4)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }
                                    vertices[4] = verticesOriginal[4] + new Vector3(moveX, 0, 0);
                                    vertices[5] = verticesOriginal[5] + new Vector3(moveX, 0, 0);
                                }


                                vertices[2] = verticesOriginal[2] + diffP; // change
                                vertices[3] = verticesOriginal[3] + diffP; // change
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;


                                UpdateVerticesBackSide();




                                if (vertices[4].x < -widthRatio) UpdateMeshDataWhenOverPeak(3);
                                else if (vertices[5].x < -widthRatio) UpdateMeshDataWhenOverPeak(4);
                                else UpdateMeshDataWhenChooseBiggerPart();

                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                                uvs[1] = VerToUv(vertices[1]);
                                uvs[0] = VerToUv(vertices[0]);

                                //uv

                            }

                        }
                    }
                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {
                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;
                    }
                    break;
                }
            //---------------------------------------------------------TH6-----------------------------------------------------------------------------------
            case eCasePos.TH6:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;

                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            startPos.z = 0;
                            //   startPos = mousePos;


                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;

                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diffPos = currentPos - startPos;



                            magDiffP = diffPos.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diffPos)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));

                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * heightRatio - disP2) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP2 = 2 * (2 * heightRatio - disP1) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP3 = 2 * (disP1) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP4 = 2 * (disP2) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x


                            }
                            isUpdateOldDiff = true;

                            if (isChooseSmallPart)
                            {
                                if (diffP.y > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                UpdateMeshDataWhenClickSmallerPart();
                                //  if (IsPeakOver1(verticesOld[1], false,true)) break; // change
                                //  if (IsPeakOver1(verticesOld[3], false, true)) break; // change

                                Vector3 newPos6 = verticesOriginal[6] + new Vector3(0, moveY, 0);
                                Vector3 newPos7 = verticesOriginal[7] + new Vector3(0, moveY, 0);
                                //if (newPos6.y < -heightRatio || newPos7.y < -heightRatio) return;

                                if (newPos7.y < -heightRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[2] = peak2;
                                        isYOver1 = true;
                                    }
                                    Vector3 new2 = verticesOriginal[2] + new Vector3(moveX + Moved1, 0, 0);
                                    if (new2.x < -widthRatio) return;
                                    if (newPos6.y < -heightRatio) return;
                                    vertices[6] = verticesOriginal[6] + new Vector3(0, moveY, 0);
                                    vertices[7] = peak2 + (magDiffP + MovedDiffP1) * moveDirection;
                                    vertices[2] = verticesOriginal[2] + new Vector3(moveX + Moved1, 0, 0);

                                }
                                else if (newPos6.y < -heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[0] = peak0;
                                        isYOver2 = true;
                                    }
                                    Vector3 new0 = verticesOriginal[0] + new Vector3(moveX + Moved2, 0, 0);
                                    if (new0.x > widthRatio) return;
                                    if (newPos7.y < -heightRatio) return;
                                    vertices[7] = verticesOriginal[7] + new Vector3(0, moveY, 0);

                                    vertices[6] = peak0 + (magDiffP + MovedDiffP2) * moveDirection;
                                    vertices[0] = verticesOriginal[0] + new Vector3(moveX + Moved2, 0, 0);

                                }
                                else
                                {
                                    if (isYOver1)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y /*- 0.001f*/, 0);
                                        isYOver1 = false;
                                    }
                                    if (isYOver2)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver2 = false;
                                    }

                                    vertices[7] = verticesOriginal[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = verticesOriginal[6] + new Vector3(0, moveY, 0);
                                }


                                vertices[1] = verticesOriginal[1] + diffP; // change
                                vertices[3] = verticesOriginal[3] + diffP; // change
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;
                                UpdateVerticesBackSide();

                                if (vertices[7].y < -heightRatio) UpdateMeshDataWhenOverPeak(1);
                                else if (vertices[6].y < -heightRatio) UpdateMeshDataWhenOverPeak(2);
                                else UpdateMeshDataWhenClickSmallerPart();
                                //uv
                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[2] = VerToUv(vertices[2]);
                                uvs[0] = VerToUv(vertices[0]);
                            }
                            else
                            {
                                UpdateMeshDataWhenChooseBiggerPart();
                                if (diffP.y < 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                //  if (IsPeakOver1(verticesOld[0], false, true)) break; // change
                                //  if (IsPeakOver1(verticesOld[2], false, true)) break; // change

                                Vector3 newPos4 = verticesOriginal[4] + new Vector3(0, moveY, 0);
                                Vector3 newPos5 = verticesOriginal[5] + new Vector3(0, moveY, 0);
                                //if (newPos4.y > heightRatio || newPos5.y > heightRatio) return;


                                if (newPos4.y > heightRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[1] = peak1;
                                        isYOver3 = true;
                                    }
                                    Vector3 new1 = verticesOriginal[1] + new Vector3(moveX - Moved3, 0, 0);
                                    if (new1.x > widthRatio) return;
                                    if (newPos5.y > heightRatio) return;
                                    vertices[5] = verticesOriginal[5] + new Vector3(0, moveY, 0);

                                    vertices[4] = peak1 + (magDiffP - MovedDiffP3) * moveDirection;
                                    vertices[1] = verticesOriginal[1] + new Vector3(moveX - Moved3, 0, 0);

                                }
                                else if (newPos5.y > heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[3] = peak3;
                                        isYOver4 = true;
                                    }
                                    Vector3 new3 = verticesOriginal[3] + new Vector3(moveX - Moved4, 0, 0);
                                    if (new3.x < -widthRatio) return;
                                    if (newPos4.y > heightRatio) return;
                                    vertices[4] = verticesOriginal[4] + new Vector3(0, moveY, 0);

                                    vertices[5] = peak3 + (magDiffP - MovedDiffP4) * moveDirection;
                                    vertices[3] = verticesOriginal[3] + new Vector3(moveX - Moved4, 0, 0);

                                }
                                else
                                {
                                    if (isYOver3)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }
                                    if (isYOver4)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }

                                    vertices[4] = verticesOriginal[4] + new Vector3(0, moveY, 0);
                                    vertices[5] = verticesOriginal[5] + new Vector3(0, moveY, 0);

                                }



                                vertices[2] = verticesOriginal[2] + diffP; // change
                                vertices[0] = verticesOriginal[0] + diffP; // change
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;
                                UpdateVerticesBackSide();

                                if (vertices[4].y > heightRatio) UpdateMeshDataWhenOverPeak(3);
                                else if (vertices[5].y > heightRatio) UpdateMeshDataWhenOverPeak(4);
                                else UpdateMeshDataWhenChooseBiggerPart();
                                //uv
                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                                uvs[1] = VerToUv(vertices[1]);
                                uvs[3] = VerToUv(vertices[3]);
                            }

                        }
                    }
                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {
                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;

                    }
                    break;
                }
            //---------------------------------------------------------TH7-----------------------------------------------------------------------------------
            case eCasePos.TH7:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;

                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            startPos.z = 0;
                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diffPos = currentPos - startPos;

                            magDiffP = diffPos.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diffPos)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));

                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * widthRatio - disP1) * Mathf.Cos(angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP2 = 2 * (2 * widthRatio - disP2) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP3 = 2 * (disP2) * Mathf.Cos(angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP4 = 2 * (disP1) * Mathf.Cos(angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y


                            }
                            isUpdateOldDiff = true;

                            if (isChooseSmallPart)
                            {
                                UpdateMeshDataWhenClickSmallerPart();
                                if (diffP.x > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                // if (IsPeakOver1(verticesOld[2], true,false)) return; // change
                                // if (IsPeakOver1(verticesOld[3], true, false)) return; // change


                                Vector3 newPos6 = verticesOriginal[6] + new Vector3(moveX, 0, 0);
                                Vector3 newPos7 = verticesOriginal[7] + new Vector3(moveX, 0, 0);
                                //if (newPos6.x < -widthRatio || newPos7.x < -widthRatio) return;

                                if (newPos6.x < -widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[1] = peak1;
                                        isYOver1 = true;
                                    }
                                    if (newPos7.x < -widthRatio) return;
                                    vertices[7] = verticesOriginal[7] + new Vector3(moveX, 0, 0);

                                    vertices[6] = peak1 + (magDiffP + MovedDiffP1) * moveDirection;
                                    vertices[1] = verticesOriginal[1] + new Vector3(0, moveY + Moved1, 0);

                                }
                                else if (newPos7.x < -widthRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[0] = peak0;
                                        isYOver2 = true;
                                    }
                                    if (newPos6.x < -widthRatio) return;
                                    vertices[6] = verticesOriginal[6] + new Vector3(moveX, 0, 0);

                                    vertices[7] = peak0 + (magDiffP + MovedDiffP2) * moveDirection;
                                    vertices[0] = verticesOriginal[0] + new Vector3(0, moveY + Moved2, 0);

                                }
                                else
                                {
                                    if (isYOver2)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver2 = false;
                                    }
                                    if (isYOver1)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver1 = false;
                                    }

                                    vertices[7] = verticesOriginal[7] + new Vector3(moveX, 0, 0);
                                    vertices[6] = verticesOriginal[6] + new Vector3(moveX, 0, 0);
                                }


                                vertices[2] = verticesOriginal[2] + diffP; // change
                                vertices[3] = verticesOriginal[3] + diffP; // change
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;

                                UpdateVerticesBackSide();

                                // Update Mesh
                                if (vertices[6].x < -widthRatio) UpdateMeshDataWhenOverPeak(1);
                                else if (vertices[7].x < -widthRatio) UpdateMeshDataWhenOverPeak(2);
                                else UpdateMeshDataWhenClickSmallerPart();

                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[0] = VerToUv(vertices[0]);
                                uvs[1] = VerToUv(vertices[1]);

                            }
                            else
                            {
                                UpdateMeshDataWhenChooseBiggerPart();
                                if (diffP.x < 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                // if (IsPeakOver1(verticesOld[0], true, false)) break; // change
                                //  if (IsPeakOver1(verticesOld[1], true, false)) break; // change

                                Vector3 newPos4 = verticesOriginal[4] + new Vector3(moveX, 0, 0);
                                Vector3 newPos5 = verticesOriginal[5] + new Vector3(moveX, 0, 0);
                                //    if (newPos4.x > widthRatio || newPos5.x > widthRatio) return;


                                if (newPos5.x > widthRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[2] = peak2;
                                        isYOver3 = true;
                                    }
                                    if (newPos4.x > widthRatio) return;
                                    vertices[4] = verticesOriginal[4] + new Vector3(moveX, 0, 0);

                                    vertices[5] = peak2 + (magDiffP - MovedDiffP3) * moveDirection;
                                    vertices[2] = verticesOriginal[2] + new Vector3(0, moveY - Moved3, 0);

                                }
                                else if (newPos4.x > widthRatio)
                                {
                                    if (!isYOver4)
                                    {
                                        vertices[3] = peak3;
                                        isYOver4 = true;
                                    }
                                    if (newPos5.x > widthRatio) return;
                                    vertices[5] = verticesOriginal[5] + new Vector3(moveX, 0, 0);

                                    vertices[4] = peak3 + (magDiffP - MovedDiffP4) * moveDirection;
                                    vertices[3] = verticesOriginal[3] + new Vector3(0, moveY - Moved4, 0);

                                }
                                else
                                {

                                    if (isYOver3)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }
                                    if (isYOver4)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }

                                    vertices[4] = verticesOriginal[4] + new Vector3(moveX, 0, 0);
                                    vertices[5] = verticesOriginal[5] + new Vector3(moveX, 0, 0);
                                }



                                vertices[0] = verticesOriginal[0] + diffP; // change
                                vertices[1] = verticesOriginal[1] + diffP; // change
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;

                                UpdateVerticesBackSide();

                                // Update Mesh
                                if (vertices[5].x > widthRatio) UpdateMeshDataWhenOverPeak(3);
                                else if (vertices[4].x > widthRatio) UpdateMeshDataWhenOverPeak(4);
                                else UpdateMeshDataWhenChooseBiggerPart();

                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                                uvs[2] = VerToUv(vertices[2]);
                                uvs[3] = VerToUv(vertices[3]);
                            }

                        }
                    }
                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {
                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;

                    }
                    break;
                }
            //---------------------------------------------------------TH8-----------------------------------------------------------------------------------
            case eCasePos.TH8:
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;

                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            startPos.z = 0;
                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diffPos = currentPos - startPos;

                            magDiffP = diffPos.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diffPos)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));

                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * heightRatio - disP1) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP2 = 2 * (2 * heightRatio - disP2) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP3 = 2 * (disP2) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP4 = 2 * (disP1) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // 


                            }
                            isUpdateOldDiff = true;
                            if (isChooseSmallPart)
                            {
                                UpdateMeshDataWhenClickSmallerPart();
                                if (diffP.y < 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                //    if (IsPeakOver1(verticesOld[0], false,true)) return; // change
                                //   if (IsPeakOver1(verticesOld[2], false, true)) return; // change

                                Vector3 newPos6 = verticesOriginal[6] + new Vector3(0, moveY, 0);
                                Vector3 newPos7 = verticesOriginal[7] + new Vector3(0, moveY, 0);
                                //if (newPos6.y > heightRatio || newPos7.y > heightRatio) return;


                                if (newPos6.y > heightRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[3] = peak3;
                                        isYOver1 = true;
                                    }
                                    if (newPos7.y > heightRatio) return;
                                    vertices[7] = verticesOriginal[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = peak3 + (magDiffP + MovedDiffP1) * moveDirection;
                                    vertices[3] = verticesOriginal[3] + new Vector3(moveX + Moved1, 0, 0);

                                }

                                else if (newPos7.y > heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[1] = peak1;
                                        isYOver2 = true;
                                    }
                                    if (newPos6.y > heightRatio) return;
                                    vertices[6] = verticesOriginal[6] + new Vector3(0, moveY, 0);

                                    vertices[7] = peak1 + (magDiffP + MovedDiffP2) * moveDirection;
                                    vertices[1] = verticesOriginal[1] + new Vector3(moveX + Moved2, 0, 0);

                                }
                                else
                                {
                                    if (isYOver1)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y /*- 0.001f*/, 0);
                                        isYOver1 = false;
                                    }
                                    if (isYOver2)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver2 = false;
                                    }
                                    vertices[7] = verticesOriginal[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = verticesOriginal[6] + new Vector3(0, moveY, 0);
                                }

                                vertices[0] = verticesOriginal[0] + diffP; // change
                                vertices[2] = verticesOriginal[2] + diffP; // change
                                vertices[4] = verticesOriginal[4] + diffP;
                                vertices[5] = verticesOriginal[5] + diffP;
                                UpdateVerticesBackSide();

                                if (vertices[6].y > heightRatio) UpdateMeshDataWhenOverPeak(1);
                                else if (vertices[7].y > heightRatio) UpdateMeshDataWhenOverPeak(2);
                                else UpdateMeshDataWhenClickSmallerPart();
                                //uv
                                uvs[1] = VerToUv(vertices[1]);
                                uvs[3] = VerToUv(vertices[3]);
                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);

                            }
                            else
                            {
                                UpdateMeshDataWhenChooseBiggerPart();
                                if (diffP.y > 0)
                                {
                                    startPos = currentPos;
                                    return;

                                }
                                //    if (IsPeakOver1(verticesOld[1], false, true)) return; // change
                                //    if (IsPeakOver1(verticesOld[3], false, true)) return; // change

                                Vector3 newPos4 = verticesOriginal[4] + new Vector3(0, moveY, 0);
                                Vector3 newPos5 = verticesOriginal[5] + new Vector3(0, moveY, 0);
                                // if (newPos4.y < -heightRatio || newPos5.y < -heightRatio) return;

                                if (newPos5.y < -heightRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[0] = peak0;
                                        isYOver3 = true;
                                    }
                                    if (newPos4.y < -heightRatio) return;
                                    vertices[4] = verticesOriginal[4] + new Vector3(0, moveY, 0);

                                    vertices[5] = peak0 + (magDiffP - MovedDiffP3) * moveDirection;
                                    vertices[0] = verticesOriginal[0] + new Vector3(moveX - Moved3, 0, 0);

                                }
                                else if (newPos4.y < -heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[2] = peak2;
                                        isYOver4 = true;
                                    }
                                    if (newPos5.y < -heightRatio) return;
                                    vertices[5] = verticesOriginal[5] + new Vector3(0, moveY, 0);

                                    vertices[4] = peak2 + (magDiffP - MovedDiffP4) * moveDirection;
                                    vertices[2] = verticesOriginal[2] + new Vector3(moveX - Moved4, 0, 0);

                                }
                                else
                                {
                                    if (isYOver3)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }
                                    if (isYOver4)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }
                                    vertices[4] = verticesOriginal[4] + new Vector3(0, moveY, 0);
                                    vertices[5] = verticesOriginal[5] + new Vector3(0, moveY, 0);
                                }

                                vertices[1] = verticesOriginal[1] + diffP; // change
                                vertices[3] = verticesOriginal[3] + diffP; // change
                                vertices[6] = verticesOriginal[6] + diffP;
                                vertices[7] = verticesOriginal[7] + diffP;

                                UpdateVerticesBackSide();

                                if (vertices[5].y < -heightRatio) UpdateMeshDataWhenOverPeak(3);
                                else if (vertices[4].y < -heightRatio) UpdateMeshDataWhenOverPeak(4);
                                else UpdateMeshDataWhenChooseBiggerPart();
                                //uv
                                uvs[0] = VerToUv(vertices[0]);
                                uvs[2] = VerToUv(vertices[2]);
                                uvs[4] = VerToUv(vertices[4]);
                                uvs[5] = VerToUv(vertices[5]);
                            }

                        }
                    }
                    // Nhả chuột
                    if (Input.GetMouseButtonUp(0))
                    {
                        oldPos = currentPos;
                        isCalculatorDiff = false;
                        isMoving = false;

                    }
                    break;

                }
            default: break;


        }



        if (diffP.magnitude < 0.3f && Input.GetMouseButtonUp(0))
        {
            startPos = currentPos;
            for (int i = 0; i < 16; i++)
            {
                vertices[i] = verticesOriginal[i];
            }
            for (int i = 0; i < 8; i++)
            {
                uvs[i] = VerToUv(vertices[i]);
            }
            isChoosePart = false;
            isMovePoint = false;
            isUpdateOldDiff = false;
        }

        VectorMove = v3tov2(diffP);
    }
    #endregion

    //----------------------------------------Mesh----------------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Mesh
    private void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triagles;
        mesh.uv = uvs;

        //mesh.colors = new Color[] { natural, natural, natural, natural };
    }
    private void MakeOriginMeshData()
    {
        vertices = new Vector3[]
        {
            // 4 cạnh
            peak0,
            peak1,
            peak2,
            peak3,

        };


        uvs = new Vector2[vertices.Length];

        {
            uvs[0] = VerToUv(vertices[0]);
            uvs[1] = VerToUv(vertices[1]);
            uvs[2] = VerToUv(vertices[2]);
            uvs[3] = VerToUv(vertices[3]);

        }




        triagles = new int[]
        {   0,1,2,
            2,1,3,
        };

    }
    private void UpdateMeshDataWhenClickSmallerPart()
    {
        switch (casePos)
        {
            case eCasePos.TH1:
                {
                    triagles = new int[]
                     {
                         6,1,2,
                         2,1,3,

                         10,11,8,
                         8,11,9,
                         6,7,1,

                         0,5,4,


                     };
                    break;
                }
            case eCasePos.TH2:
                {
                    triagles = new int[]
                     {
                         6,7,0,
                         0,7,3,
                         0,3,2,

                         10,11,8,
                         8,11,9,

                         1,5,4,

                     };
                    break;
                }
            case eCasePos.TH3:
                {
                    triagles = new int[]
                     {
                         6,7,2,
                         6,2,1,
                         0,1,2,

                         10,11,8,
                         8,11,9,

                         3,5,4,


                     };
                    break;
                }
            case eCasePos.TH4:
                {
                    triagles = new int[]
                     {
                         6,7,3,
                         3,7,0,
                         0,1,3,

                         10,11,8,
                         8,11,9,

                         2,5,4,


                     };
                    break;
                }
            case eCasePos.TH5:
                {
                    triagles = new int[]
                     {
                        6,7,2,
                        2,7,3,

                        10,11,9,
                        10,9,8,

                        0,1,4,
                        4,1,5,


                     };
                    break;
                }
            case eCasePos.TH6:
                {
                    triagles = new int[]
                     {
                        6,7,0,
                        0,7,2,


                        10,11,9,
                        10,9,8,

                        1,3,4,
                        4,3,5,


                     };
                    break;
                }
            case eCasePos.TH7:
                {
                    triagles = new int[]
                     {
                         7,1,6,
                        7,0,1,


                        10,11,9,
                        10,9,8,

                        2,4,3,
                        2,5,4,

                     };
                    break;
                }
            case eCasePos.TH8:
                {
                    triagles = new int[]
                     {
                        7,3,6,
                        7,1,3,

                        10,11,9,
                        10,9,8,

                        2,0,4,
                        0,5,4,


                     };
                    break;
                }


            default: break;
        }
    }
    private void UpdateMeshDataWhenChooseBiggerPart()
    {
        switch (casePos)
        {
            case eCasePos.TH1:
                {
                    triagles = new int[]
                     {
                         0,5,4,

                         10,11,8,
                         8,11,9,
                         6,7,1,

                         6,1,2,
                         2,1,3,

                     };
                    break;
                }
            case eCasePos.TH2:
                {
                    triagles = new int[]
                     {
                         1,5,4,

                         10,11,8,
                         8,11,9,

                         6,7,0,
                         0,7,3,
                         0,3,2,

                     };
                    break;
                }
            case eCasePos.TH3:
                {
                    triagles = new int[]
                     {
                         3,5,4,

                         10,11,8,
                         8,11,9,

                         6,7,2,
                         6,2,1,
                         0,1,2,

                     };
                    break;
                }
            case eCasePos.TH4:
                {
                    triagles = new int[]
                     {

                         2,5,4,

                         10,11,8,
                         8,11,9,

                         6,7,3,
                         3,7,0,
                         0,1,3,

                     };
                    break;
                }
            case eCasePos.TH5:
                {
                    triagles = new int[]
                     {
                        0,1,4,
                        4,1,5,

                        10,11,9,
                        10,9,8,

                        6,7,2,
                        2,7,3,

                     };
                    break;
                }
            case eCasePos.TH6:
                {
                    triagles = new int[]
                     {
                        1,3,4,
                        4,3,5,

                        10,11,9,
                        10,9,8,

                        6,7,0,
                        0,7,2,

                     };
                    break;
                }
            case eCasePos.TH7:
                {
                    triagles = new int[]
                     {
                        2,4,3,
                        2,5,4,

                        10,11,9,
                        10,9,8,

                        7,1,6,
                        7,0,1,

                     };
                    break;
                }
            case eCasePos.TH8:
                {
                    triagles = new int[]
                     {
                        2,0,4,
                        0,5,4,

                        10,11,9,
                        10,9,8,

                        7,3,6,
                        7,1,3,


                     };
                    break;
                }


            default: break;
        }
    }
    private bool IsPeakOver(Vector3 peak, int n)
    {
        switch (n)
        {
            case 1:
                if (peak.y < -heightRatio) return true;
                break;
            case 2:
                if (peak.x < -widthRatio) return true;
                break;
            case 3:
                if (peak.x > widthRatio) return true;
                break;
            case 4:
                if (peak.y > heightRatio) return true;
                break;

        }
        if ((peak.x <= -widthRatio && peak.y <= -heightRatio) || (peak.x <= -widthRatio && peak.y >= heightRatio) || (peak.x >= widthRatio && peak.y <= -heightRatio) || (peak.x >= widthRatio && peak.y >= heightRatio))
        // if ( ( x && ( test.x < -widthRatio || (test.x > widthRatio))) || ( y &&( (test.y < -heightRatio)  || ( test.y > heightRatio))))
        {

            return true;
        }
        else return false;
    }

    private bool IsPeakOver2(Vector3 peak, bool x = true, bool y = true)
    {

        // Vector3 test = peak + diffu * new Vector3(lineSlicer.GetChild(0).transform.up.x, lineSlicer.GetChild(0).transform.up.y, 0); // change
        Vector3 test = peak + diffP; // change


        if ((test.x <= -widthRatio && test.y <= -heightRatio) || (test.x <= -widthRatio && test.y >= heightRatio) || (test.x >= widthRatio && test.y <= -heightRatio) || (test.x >= widthRatio && test.y >= heightRatio))
        // if ( ( x && ( test.x < -widthRatio || (test.x > widthRatio))) || ( y &&( (test.y < -heightRatio)  || ( test.y > heightRatio))))
        {

            return true;
        }
        else return false;
    }

    private bool IsPeakOver1(Vector3 peak, bool x = true, bool y = true)
    {

        // Vector3 test = peak + diffu * new Vector3(lineSlicer.GetChild(0).transform.up.x, lineSlicer.GetChild(0).transform.up.y, 0); // change
        Vector3 test = peak + diffP; // change


        // if ((test.x <= -widthRatio && test.y <= -heightRatio) || (test.x <= -widthRatio && test.y >= heightRatio) || (test.x >= widthRatio && test.y <= -heightRatio) || (test.x >= widthRatio && test.y >= heightRatio))
        if ((x && (test.x < -widthRatio || (test.x > widthRatio))) || (y && ((test.y < -heightRatio) || (test.y > heightRatio))))
        {

            return true;
        }
        else return false;
    }
    private void UpdateMeshDataWhenOverPeak(int movePos)
    {
        switch (casePos)
        {
            case eCasePos.TH1:
                {
                    if (movePos == (int)eMovePos.normal)
                    {
                        triagles = new int[]
                     {

                         6,7,1,
                         6,1,2,
                         2,1,3,


                         10,11,8,
                         8,11,9,

                         0,5,4,
                     };


                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {

                         2,7,1,
                         2,1,3,

                         14,11,10,
                         10,11,9,
                         10,9,8,

                         0,5,4,
                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        6,1,2,
                        2,1,3,

                        8,10,9,
                        9,10,11,
                        11,10,13,

                        0,5,4,

                    };

                    }

                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        2,1,3,

                        10,9,8,
                        14,9,10,
                        14,11,9,
                        14,13,11,

                        0,5,4,

                    };

                    }




                    break;
                }
            case eCasePos.TH2:
                {
                    if (movePos == (int)eMovePos.normal)
                    {

                        triagles = new int[]
                     {

                         6,7,0,
                         0,7,3,
                         0,3,2,


                         10,11,8,
                         8,11,9,

                         1,5,4,
                     };

                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {

                         0,7,3,
                         0,3,2,

                         12,11,10,
                         10,11,8,
                         8,11,9,

                         1,5,4,
                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        6,3,0,
                        0,3,2,

                        10,15,11,
                        10,11,8,
                        8,11,9,

                        1,5,4,

                    };

                    }

                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        0,3,2,

                        12,15,10,
                        10,15,11,
                        10,11,8,
                        8,11,9,

                        1,5,4,

                    };

                    }

                    break;
                }
            case eCasePos.TH3:
                {
                    if (movePos == (int)eMovePos.normal)
                    {

                        triagles = new int[]
                     {

                         6,7,2,
                         6,2,1,
                         0,1,2,

                         10,11,8,
                         8,11,9,

                         3,5,4,
                     };

                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {


                         1,7,2,
                         1,2,0,

                         13,11,10,
                         10,11,9,
                         10,9,8,

                         3,5,4,
                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        6,2,1,
                        1,2,0,

                        10,14,11,
                        10,11,9,
                        10,9,8,

                        3,5,4,

                    };

                    }

                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        1,2,0,

                        13,14,10,
                        10,14,11,
                        10,11,8,
                        8,11,9,


                        3,5,4,

                    };

                    }

                    break;
                }
            case eCasePos.TH4:
                {
                    if (movePos == (int)eMovePos.normal)
                    {

                        triagles = new int[]
                     {

                         6,7,3,
                         3,7,0,
                         0,1,3,

                         10,11,8,
                         8,11,9,

                         2,5,4,
                     };

                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {


                         7,0,3,
                         0,1,3,

                         15,11,9,
                         15,9,10,
                         10,9,8,

                         2,5,4,
                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        6,0,3,
                        0,1,3,


                        10,12,8,
                        8,12,11,
                        8,11,9,
                        8,9,14,

                        2,5,4,

                    };

                    }

                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        0,1,3,

                        10,15,12,
                        10,12,11,
                        10,11,8,
                        8,11,9,


                        2,5,4,

                    };

                    }

                    break;
                }
            case eCasePos.TH5:
                {
                    if (movePos == (int)eMovePos.normal)
                    {

                        triagles = new int[]
                     {

                         2,6,3,
                         3,6,7,

                         8,10,9,
                         9,10,11,

                         4,0,5,
                         5,0,1
                     };

                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {


                         2,6,3,

                         9,8,10,
                         9,10,11,
                         11,10,15,

                         4,0,5,
                         5,0,1
                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        3,2,7,

                        10,14,11,
                        10,11,8,
                        8,11,9,

                        4,0,1,
                        4,1,5

                    };

                    }
                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        0,1,5,

                        12,8,9,
                        9,8,10,
                        9,10,11,

                        2,6,7,
                        2,7,3

                    };

                    }

                    if (movePos == (int)eMovePos.over4)
                    {


                        triagles = new int[]
                    {

                        0,1,4,

                        13,8,9,
                        9,8,11,
                        11,8,10,

                        2,6,7,
                        2,7,3

                    };


                    }
                    break;


                }
            case eCasePos.TH6:
                {
                    if (movePos == (int)eMovePos.normal)
                    {

                        triagles = new int[]
                     {


                     };

                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {


                         0,6,2,

                         10,14,11,
                         10,11,8,
                         8,11,9,

                         5,4,1,
                         5,1,3,

                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        2,0,7,

                        9,8,10,
                        9,10,12,
                        9,12,11,

                        5,4,3,
                        3,4,1,

                    };

                    }
                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        5,1,3,

                        10,11,8,
                        8,11,9,
                        8,9,13,

                        0,6,2,
                        2,6,7,


                    };

                    }

                    if (movePos == (int)eMovePos.over4)
                    {


                        triagles = new int[]
                    {

                        4,1,3,

                        10,11,8,
                        8,11,9,
                        8,9,15,

                        2,0,7,
                        7,0,6

                    };


                    }
                    break;


                }
            case eCasePos.TH7:
                {
                    if (movePos == (int)eMovePos.normal)
                    {

                        triagles = new int[]
                     {

                     };

                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {


                         0,1,7,

                         9,8,11,
                         11,8,10,
                         11,10,13,

                         2,5,4,
                         2,4,3,



                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        0,1,6,

                        11,10,12,
                        10,11,9,
                        10,9,8,

                        2,5,3,
                        3,5,4,

                    };

                    }
                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        2,4,3,

                        10,11,9,
                        10,9,8,
                        8,9,14,

                        0,1,7,
                        7,1,6,


                    };

                    }

                    if (movePos == (int)eMovePos.over4)
                    {


                        triagles = new int[]
                    {

                        2,5,3,

                        11,9,10,
                        10,9,8,
                        8,9,15,

                        7,0,6,
                        6,0,1,

                    };


                    }
                    break;


                }
            case eCasePos.TH8:
                {
                    if (movePos == (int)eMovePos.normal)
                    {

                        triagles = new int[]
                     {

                     };

                    }
                    if (movePos == (int)eMovePos.over1)
                    {

                        triagles = new int[]
                     {


                         7,1,3,

                         8,11,9,
                         11,8,10,
                         11,10,15,


                         2,0,4,
                         4,0,5,



                     };

                    }

                    if (movePos == (int)eMovePos.over2)
                    {


                        triagles = new int[]
                    {

                        6,1,3,

                        8,10,9,
                        9,10,11,
                        11,10,13,


                        2,0,4,
                        4,0,5,

                    };

                    }
                    if (movePos == (int)eMovePos.over3)
                    {


                        triagles = new int[]
                    {

                        2,0,4,

                        9,12,8,
                        9,8,11,
                        11,8,10,


                        6,7,3,
                        3,7,1,


                    };

                    }

                    if (movePos == (int)eMovePos.over4)
                    {


                        triagles = new int[]
                    {

                        2,0,5,

                        9,14,8,
                        9,8,10,
                        9,10,11,

                        6,7,3,
                        3,7,1,

                    };


                    }
                    break;


                }
            default: break;
        }



    }
    // Sắp xếp P1 P2 theo chiều kim đồng hồ: P1 trước, P2 sau
    private void CalculateDistance()
    {
        switch (casePos)
        {
            case eCasePos.TH1:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak0));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak0));

                    break;
                }
            case eCasePos.TH2:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak1));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak1));

                    break;
                }
            case eCasePos.TH3:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak3));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak3));

                    break;
                }
            case eCasePos.TH4:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak2));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak2));



                    break;

                }
            case eCasePos.TH5:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak0));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak1));



                    break;
                }
            case eCasePos.TH6:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak1));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak3));




                    break;
                }
            case eCasePos.TH7:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak3));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak2));



                    break;
                }
            case eCasePos.TH8:
                {

                    disP1 = Vector2.Distance(v3tov2(point1), v3tov2(peak2));
                    disP2 = Vector2.Distance(v3tov2(point2), v3tov2(peak0));


                    break;
                }
            default: break;
        }
        swap = true;


    }
    private void UpdateMeshDataAfterSlice()
    {

        vertices = new Vector3[]
        {
            // 4 cạnh
            peak0,
            peak1,
            peak2,
            peak3,
            point1,
            point2,
            point1,
            point2,

            point1,
            point2,
            point1,
            point2,
            peak0,
            peak1,
            peak2,
            peak3,

        };
        uvs = new Vector2[vertices.Length];

        for (int i = 0; i < 8; i++)
        {
            uvs[i] = VerToUv(vertices[i]);
        }

        //float x = UnityEngine.Random.Range(0, 1000f);
        //float y = UnityEngine.Random.Range(0, 1000f);

        for (int i = 8; i < vertices.Length; i++)
        {
            //float x = UnityEngine.Random.Range(0, 1000f);
            //float y = UnityEngine.Random.Range(0, 1000f);
            uvs[i] = new Vector2(0, 0);
        }
    }
    private Vector2 VerToUv(Vector3 vector3)
    {
        return (new Vector2((vector3.x + widthRatio) / (2 * widthRatio), (vector3.y + heightRatio) / (2 * heightRatio)));
    }
    private void UpdateVerticesBackSide()
    {
        vertices[8] = vertices[4];
        vertices[9] = vertices[5];
        vertices[10] = vertices[6];
        vertices[11] = vertices[7];
        vertices[12] = vertices[0];
        vertices[13] = vertices[1];
        vertices[14] = vertices[2];
        vertices[15] = vertices[3];
    }
    private void UpdateVerOfEdgeSliced()
    {
        switch (casePos)
        {
            case eCasePos.TH1:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[4];

                        edgeSliced[0] = vertices[0];
                        edgeSliced[1] = vertices[4];
                        edgeSliced[2] = vertices[5];
                        edgeSliced[3] = vertices[0];
                    }
                    else
                    {
                        edgeSliced = new Vector3[6];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[1];
                        edgeSliced[3] = vertices[3];
                        edgeSliced[4] = vertices[2];
                        edgeSliced[5] = vertices[6];
                    }
                    break;
                }
            case eCasePos.TH2:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[4];

                        edgeSliced[0] = vertices[1];
                        edgeSliced[1] = vertices[4];
                        edgeSliced[2] = vertices[5];
                        edgeSliced[3] = vertices[1];
                    }
                    else
                    {
                        edgeSliced = new Vector3[6];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[3];
                        edgeSliced[3] = vertices[2];
                        edgeSliced[4] = vertices[0];
                        edgeSliced[5] = vertices[6];
                    }
                    break;
                }
            case eCasePos.TH3:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[4];

                        edgeSliced[0] = vertices[3];
                        edgeSliced[1] = vertices[4];
                        edgeSliced[2] = vertices[5];
                        edgeSliced[3] = vertices[3];
                    }
                    else
                    {
                        edgeSliced = new Vector3[6];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[2];
                        edgeSliced[3] = vertices[0];
                        edgeSliced[4] = vertices[1];
                        edgeSliced[5] = vertices[6];
                    }
                    break;
                }
            case eCasePos.TH4:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[4];

                        edgeSliced[0] = vertices[2];
                        edgeSliced[1] = vertices[4];
                        edgeSliced[2] = vertices[5];
                        edgeSliced[3] = vertices[2];
                    }
                    else
                    {
                        edgeSliced = new Vector3[6];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[0];
                        edgeSliced[3] = vertices[1];
                        edgeSliced[4] = vertices[3];
                        edgeSliced[5] = vertices[6];
                    }
                    break;
                }
            case eCasePos.TH5:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[0];
                        edgeSliced[1] = vertices[1];
                        edgeSliced[2] = vertices[5];
                        edgeSliced[3] = vertices[4];
                        edgeSliced[4] = vertices[0];
                    }
                    else
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[3];
                        edgeSliced[3] = vertices[2];
                        edgeSliced[4] = vertices[6];
                    }
                    break;
                }
            case eCasePos.TH6:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[1];
                        edgeSliced[1] = vertices[3];
                        edgeSliced[2] = vertices[5];
                        edgeSliced[3] = vertices[4];
                        edgeSliced[4] = vertices[1];
                    }
                    else
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[2];
                        edgeSliced[3] = vertices[0];
                        edgeSliced[4] = vertices[6];
                    }
                    break;
                }
            case eCasePos.TH7:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[2];
                        edgeSliced[1] = vertices[3];
                        edgeSliced[2] = vertices[4];
                        edgeSliced[3] = vertices[5];
                        edgeSliced[4] = vertices[2];
                    }
                    else
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[0];
                        edgeSliced[3] = vertices[1];
                        edgeSliced[4] = vertices[6];
                    }
                    break;
                }
            case eCasePos.TH8:
                {
                    if (isChooseSmallPart)
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[2];
                        edgeSliced[1] = vertices[0];
                        edgeSliced[2] = vertices[5];
                        edgeSliced[3] = vertices[4];
                        edgeSliced[4] = vertices[2];
                    }
                    else
                    {
                        edgeSliced = new Vector3[5];

                        edgeSliced[0] = vertices[6];
                        edgeSliced[1] = vertices[7];
                        edgeSliced[2] = vertices[1];
                        edgeSliced[3] = vertices[3];
                        edgeSliced[4] = vertices[6];
                    }
                    break;
                }
        }
    }
    #endregion Mesh

    // ---------------------------------------Vector - Point - Line----------------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Vector - Point - Line
    // chuyển vector3 sang vector2 ( z=0);
    private Vector2 v3tov2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }
    // khoảng cách của point với đường thẳng tạo bởi point1 và point2
    private float RelationPointAndLine(Vector3 point, Vector3 point1, Vector3 point2)
    {
        float x21 = point2.x - point1.x;
        float y21 = point2.y - point1.y;
        return y21 * point.x - x21 * point.y - point1.x * y21 + point1.y * x21;
    }
    // Giao điểm của đường thẳng tạo bới AB và hình chữ nhât tạo bởi 4 góc

    private Vector2[] GetLineIntersection(Vector2 pointA, Vector2 pointB, Vector3 peak3, Vector3 peak2, Vector3 peak0, Vector3 peak1, float width, float height)
    {


        Vector2[] lineIntersection = new Vector2[2];
        int count = 0;
        int temp = 0;
        Vector2[] edge = new Vector2[] { peak3, peak2, peak0, peak1 };
        Vector2 a = new Vector2();
        while (count < 2 && temp < 5)
        {
            a = LineIntersection(pointA, pointB, edge[temp % 4], edge[(temp + 1) % 4]);

            if (a != new Vector2(float.MaxValue, float.MaxValue) && Mathf.Abs(a.x) <= (width + 0.00001f) && Mathf.Abs(a.y) <= (height + 0.00001f))
            {
                lineIntersection[count] = a;


                count++;
            }
            temp++;
        }
        return lineIntersection;
    }
    public Vector2[] GetLineIntersection(Vector2 pointA, float angle, Vector3 peak3, Vector3 peak2, Vector3 peak0, Vector3 peak1, float width, float height)
    {
        Vector2[] lineIntersection = new Vector2[2];
        int count = 0;
        int temp = 0;
        Vector2[] edge = new Vector2[] { peak3, peak2, peak0, peak1 };
        Vector2 a = new Vector2();
        while (count < 2 && temp < 5)
        {
            a = LineIntersection(pointA, angle, edge[temp % 4], edge[(temp + 1) % 4]);

            if (a != new Vector2(float.MaxValue, float.MaxValue) && Mathf.Abs(a.x) <= (width + 0.00001f) && Mathf.Abs(a.y) <= (height + 0.00001f))
            {
                lineIntersection[count] = a;
                count++;
            }
            temp++;
        }
        return lineIntersection;
    }
    private Vector2 LineIntersection(Vector2 pointA, float angle, Vector2 pointC, Vector2 pointD)
    {
        // Line AB represented as a1x + b1y = c1  
        float a1 = Mathf.Tan(angle * Mathf.PI / 180);
        float b1 = -1f;
        float c1 = a1 * pointA.x - pointA.y;

        // Line CD represented as a2x + b2y = c2  
        float a2 = pointD.y - pointC.y;
        float b2 = pointC.x - pointD.x;
        float c2 = a2 * (pointC.x) + b2 * (pointC.y);

        float determinant = a1 * b2 - a2 * b1;

        if (determinant == 0)
        {
            // The lines are parallel. This is simplified  
            // by returning a pair of FLT_MAX  
            return new Vector2(float.MaxValue, float.MaxValue); ;
        }
        else
        {
            float x = (b2 * c1 - b1 * c2) / determinant;
            float y = (a1 * c2 - a2 * c1) / determinant;

            //  Debug.Log(x+ " " + y);
            return new Vector2(x, y);
        }
    }
    private Vector2 LineIntersection(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
    {
        // Line AB represented as a1x + b1y = c1  
        float a1 = pointB.y - pointA.y;
        float b1 = pointA.x - pointB.x;
        float c1 = a1 * (pointA.x) + b1 * (pointA.y);

        // Line CD represented as a2x + b2y = c2  
        float a2 = pointD.y - pointC.y;
        float b2 = pointC.x - pointD.x;
        float c2 = a2 * (pointC.x) + b2 * (pointC.y);

        float determinant = a1 * b2 - a2 * b1;

        if (determinant == 0)
        {
            // The lines are parallel. This is simplified  
            // by returning a pair of FLT_MAX  
            return new Vector2(float.MaxValue, float.MaxValue); ;
        }
        else
        {
            float x = (b2 * c1 - b1 * c2) / determinant;
            float y = (a1 * c2 - a2 * c1) / determinant;
            return new Vector2(x, y);
        }
    }
    private void swapP1P2()
    {

        Vector3 p3 = point1;
        point1 = point2;
        point2 = p3;
    }
    // xét vị trí cắt ( 8 trường hợp)
    private void ClassSlicePosition()
    {
        float insteadEdge0, insteadEdge1, insteadEdge2, insteadEdge3, insteadCentre;

        insteadEdge0 = RelationPointAndLine(peak0, point1, point2);
        insteadEdge1 = RelationPointAndLine(peak1, point1, point2);
        insteadEdge2 = RelationPointAndLine(peak2, point1, point2);
        insteadEdge3 = RelationPointAndLine(peak3, point1, point2);
        insteadCentre = RelationPointAndLine(new Vector3(0, 0, 0), point1, point2);

        if (insteadEdge0 * insteadEdge3 < 0)
        {
            if (insteadEdge0 * insteadCentre < 0 && insteadEdge3 * insteadCentre > 0)
            {
                if (insteadEdge1 * insteadCentre < 0 && insteadEdge2 * insteadCentre > 0)
                {
                    Debug.Log("TH5"); casePos = eCasePos.TH5;
                }
                else if (insteadEdge2 * insteadCentre < 0 && insteadEdge1 * insteadCentre > 0)
                {
                    Debug.Log("TH8"); casePos = eCasePos.TH8;
                }
                else if (insteadEdge1 * insteadCentre > 0 && insteadEdge2 * insteadCentre > 0)
                {
                    Debug.Log("TH1"); casePos = eCasePos.TH1;
                }
                else if (insteadEdge0 * insteadEdge1 > 0)
                {
                    Debug.Log("TH5"); casePos = eCasePos.TH5;
                }
                else if (insteadEdge0 * insteadEdge1 < 0)
                {
                    Debug.Log("TH6"); casePos = eCasePos.TH6;
                }
                else Debug.Log(" Deo hieu kieu gi");
            }

            else if (insteadEdge0 * insteadCentre > 0 && insteadEdge3 * insteadCentre < 0)
            {
                if (insteadEdge1 * insteadCentre < 0 && insteadEdge2 * insteadCentre > 0)
                {
                    Debug.Log("TH6"); casePos = eCasePos.TH6;
                }
                else if (insteadEdge2 * insteadCentre < 0 && insteadEdge1 * insteadCentre > 0)
                {
                    Debug.Log("TH7"); casePos = eCasePos.TH7;
                }
                else if (insteadEdge1 * insteadCentre > 0 && insteadEdge2 * insteadCentre > 0)
                {
                    Debug.Log("TH3"); casePos = eCasePos.TH3;
                }
                else Debug.Log(" Deo hieu kieu gi");
            }
            else Debug.Log(" Deo hieu kieu gi");
        }
        else if (insteadEdge0 * insteadEdge3 > 0)
        {
            if (insteadEdge1 * insteadCentre > 0 && insteadEdge2 * insteadCentre < 0)
            {
                Debug.Log("TH4"); casePos = eCasePos.TH4;
            }
            else if (insteadEdge1 * insteadCentre < 0 && insteadEdge2 * insteadCentre > 0)
            {
                Debug.Log("TH2"); casePos = eCasePos.TH2;
            }
            else Debug.Log(" Deo hieu kieu gi");
        }
        else Debug.Log(" Deo hieu kieu gi");
    }
    public float AngleSlice()
    {

        Vector2 p1Check = point1;
        Vector2 p2Check = point2;

        if (point1.y > point2.y)
        {
            Vector2 p3Check = p1Check;
            p1Check = p2Check;
            p2Check = p3Check;
        }

        return Vector2.SignedAngle(Vector2.right, new Vector2(p2Check.x - p1Check.x, p2Check.y - p1Check.y));
    }
    #endregion Vector - Point - LineVector - Point - Line

    //----------------------------------------Enum --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Enum
    enum eCasePos
    {
        TH1,
        TH2,
        TH3,
        TH4,
        TH5,
        TH6,
        TH7,
        TH8
    }
    enum eMovePos
    {
        normal,
        over1,
        over2,
        over3,
        over4
    }

    #endregion

    //----------------------------------------Btn------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }


    private void OnDrawGizmos()
    {
        //if (isSliced)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(point1, 1f);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawSphere(point2, 1f);
        //    Gizmos.color = Color.red;
        //    Gizmos.color = Color.red;
        //    for ( int i = 0; i < 8; i++)
        //    {
        //        Gizmos.DrawSphere(vertices[i], 0.5f);
        //    }

        //}

        //if ( isSliced)
        //{

        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(smallPartVertices[0], 0.5f);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawSphere(smallPartVertices[1], 0.5f);
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(smallPartVertices[2], 0.5f);
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawSphere(smallPartVertices[3], 0.5f);
        //}

    }
}
