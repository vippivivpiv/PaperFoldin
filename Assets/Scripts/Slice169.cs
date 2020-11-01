
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
    public WinCheckOneAnswer winCheck;


    public Camera mainCamera;
    public MeshRenderer meshRenderer;


    public Transform lineSlicer;
    public GameObject confirmPanel;
    public Transform startPoint;


    public float widthRatio;
    public float heightRatio;

    private Mesh mesh;
    public Vector3 point1, point2;
    public Vector3[] vertices;
    public int[] triagles;
    public Vector2[] uvs;
    private Vector3[] verticesOld;
    public Vector3[] edgeSliced;

    private Vector3 startPos;

    public  Vector2 VectorMove;
    public  Vector3 peak0, peak1, peak2, peak3;
    private Vector3 currentPos;
    private Vector3 oldPos;
    private Vector3 curPos;
    private Vector3 diff;
    private Vector3 diffBetweenCurandOldPos;
    private Vector3 moveDirection;
    private Vector3 mousePos;
    private Vector3 point1Matched;
    private Vector3 point2Matched;

    private float Moved2;

    public float MovedDiffP3 { get; private set; }
    public float Moved3 { get; private set; }
    public float MovedDiffP4 { get; private set; }
    public float Moved4 { get; private set; }

    private float MovedDiffP2;
    private float MovedDiffP1;
    private float Moved1;

    private float disp1, disp2;
    private float magDiffP;
    private Vector3 diffP;
    public bool isConfirmSlice;
    public bool isSliced;
    public bool isMoving;
    public bool isClick;
    private bool isCalculatorDiff;
    private bool isUpdateOldDiff;
    private bool isYOver1;
    private bool isYOver2;
    private bool isChoosePart;
    private bool isChooseSmallPart;
    private bool isDisplayPanelConfirmSlice;
    public bool isWin;

    private float TimerCount = 0;
    private float DelayTime = 1;

    private eCasePos casePos;
    private bool isYOver3;
    private bool isYOver4;

    private List<Vector3> smallPartVertices, bigPartVerteces;
    #endregion
    private void Start()
    {


        //widthRatio = 16;
        //heightRatio = 9;
        // mainCamera.orthographicSize = 12;
        lineSlicer.gameObject.SetActive(false);
        peak0 = new Vector3(-widthRatio, -heightRatio, 0);
        peak1 = new Vector3(-widthRatio, heightRatio, 0);
        peak2 = new Vector3(widthRatio, -heightRatio, 0);
        peak3 = new Vector3(widthRatio, heightRatio, 0);

        edgeSliced = new Vector3[0];
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "Default";

        meshRenderer.sortingOrder = 0;
        MakeOriginMeshData();
        CreateMesh();
        diffBetweenCurandOldPos = new Vector3(0, 0, 0);

        mainCamera = Camera.main;

        //  GetComponent<WinCheck>().enabled = true;


    }


    private void OnEnable()
    {
        //lineSlicer = GameObject.Instantiate(prefabLineSlicer).transform;
        //lineSlicer.gameObject.SetActive(false);

        isSliced = false;
        //isCalculatorDiff = false;
        //isChoosePart = false;
        //isChooseSmallPart = false;
        isClick = false;
        isConfirmSlice = false;
        //isUpdateOldDiff = false;

        MakeOriginMeshData();
    }

    private void Update()
    {

        if (winCheck.isWin) return;

        {
            if (!isSliced)
            {
                Slice();
            }
            else
            {

                Move();

                CreateMesh();

                UpdateVerOfEdgeSliced();
            }


        }



    }

    //----------------------------------------Gameplay-----------------------------------------------------------------------------------------------------------------------------------------------
    #region Gameplay
    private void Slice()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log(mousePos);

        if (Input.GetMouseButton(0))
        {
            if (!isClick)
            {
                if (Mathf.Abs(mousePos.x) > widthRatio || Mathf.Abs(mousePos.y) > heightRatio) return;

                startPoint.gameObject.SetActive(true);
                isClick = true;

                startPos = mousePos;


                startPoint.position = new Vector3(startPos.x, startPos.y, -0.1f);

                //lineSlicer.position = new Vector3(startPos.x, startPos.y, 0);

            }
            else
            {
                curPos = mousePos;
                diff = curPos - startPos;



                Vector2[] lineIntersection = GetLineIntersection(startPos, curPos, peak3, peak2, peak0, peak1, widthRatio, heightRatio);
                point1 = new Vector3(lineIntersection[0].x, lineIntersection[0].y, 0);
                point2 = new Vector3(lineIntersection[1].x, lineIntersection[1].y, 0);


                float dist = Vector2.Distance(point1, point2);


                lineSlicer.GetChild(0).localEulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(diff, Vector2.right));


                lineSlicer.GetChild(0).position = (point1 + point2) / 2;


                lineSlicer.GetChild(0).localScale = new Vector3(dist, 0.1f, 0.1f);
                lineSlicer.GetChild(0).transform.position = new Vector3(lineSlicer.GetChild(0).transform.position.x, lineSlicer.GetChild(0).transform.position.y, -1.1f);
                lineSlicer.gameObject.SetActive(true);


            }
        }
        if (Input.GetMouseButtonUp(0) && isClick)
        {
            if (diff.magnitude == 0) return;

            isClick = false;
            isSliced = true;

            // DisplayLineSlicer(winCheck.AutoMatchLine());

            ClassSlicePosition();
            SortP1P2Position();
            UpdateMeshDataAfterSlice();
            UpdatePositionMoveVertercies();


            verticesOld = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                verticesOld[i] = vertices[i];
            }
        }
    }

    private void UpdatePositionMoveVertercies()
    {
        smallPartVertices = new List<Vector3>();
        smallPartVertices.Add(vertices[4]);
        smallPartVertices.Add(vertices[5]);
        bigPartVerteces = new List<Vector3>();
        bigPartVerteces.Add(vertices[6]);
        bigPartVerteces.Add(vertices[7]);
       switch ( casePos)
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
    bool CheckTotalAngleOfPointAndSmallPolygons()
    {
        UpdatePositionMoveVertercies();
        float angle = 0;
        mousePos.z = 0;
        for (int i = 0; i < smallPartVertices.Count-1; i++)
        {
            
            angle += Vector3.Angle(smallPartVertices[i + 1] - mousePos, smallPartVertices[i] - mousePos);

        }
        angle += Vector3.Angle(smallPartVertices[0] - mousePos, smallPartVertices[smallPartVertices.Count - 1] - mousePos);
        Debug.Log(angle);
        return (angle>359 && angle< 361) ? true : false;
    }

    bool CheckTotalAngleOfPointAndBIgPolygons()
    {
        UpdatePositionMoveVertercies();
        float angle = 0;
        mousePos.z = 0;
        for (int i = 0; i < bigPartVerteces.Count - 1; i++)
        {
            angle += Vector3.Angle(bigPartVerteces[i + 1] - mousePos, bigPartVerteces[i] - mousePos);
        }
        angle += Vector3.Angle(bigPartVerteces[0] - mousePos, bigPartVerteces[bigPartVerteces.Count - 1] - mousePos);
        Debug.Log(angle);
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
    private void Move()
    {
        moveDirection = new Vector3(lineSlicer.GetChild(0).transform.up.x, lineSlicer.GetChild(0).transform.up.y, 0);
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //if (Mathf.Abs(mousePos.x) > widthRatio || Mathf.Abs(mousePos.y) > heightRatio) return;
        Debug.Log(isChoosePart);
        if (Input.GetMouseButtonDown(0))
        {
            if (!isChoosePart)
            {

                isChoosePart = true;
                if (CheckTotalAngleOfPointAndSmallPolygons()) isChooseSmallPart = true;
                else if (CheckTotalAngleOfPointAndBIgPolygons()) isChooseSmallPart = false;
                else
                {
                    isChoosePart = false;
                }


            }
        }
            
        if (!isChoosePart) return;

        //if (isChooseSmallPart && !CheckTotalAngleOfPointAndSmallPolygons()) return;
        //if (!isChooseSmallPart && !CheckTotalAngleOfPointAndBIgPolygons()) return;

        Debug.Log("choose");
        switch (casePos)
        {
            //--------------------------------------------------------TH1------------------------------------------------------------------------------------
            case eCasePos.TH1:
                {
                    if (Input.GetMouseButton(0)) // Kiểm tra xem Lần click đầu Click vào phần To hay phần BÉ
                    {
                        if (!isClick)
                        {
                            isMoving = true;
                            isClick = true;
                            isCalculatorDiff = true;
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);

                            startPos = mousePos;
                            Debug.Log(startPos);

                        }
                        else
                        {
                            currentPos = mousePos;
                            currentPos.z = 0;
                            Debug.Log(currentPos);

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diff = currentPos - startPos;

                            Debug.Log(diff);

                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));

                            // giá trị đã đi được khi các cạnh over
                            if (!isUpdateOldDiff)
                            {
                                Moved1 = 2 * widthRatio * disp2 / disp1 - disp2; // y
                                MovedDiffP1 = 2 * (2 * widthRatio - disp1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * heightRatio * disp1 / disp2 - disp1; // x
                                MovedDiffP2 = 2 * (2 * heightRatio - disp2) * Mathf.Sin(angleA * Mathf.PI / 180);

                            }
                            isUpdateOldDiff = true;

                            if (isChooseSmallPart) // Phần Cắt bé
                            {
                                if (IsPeakOver(verticesOld[0], magDiffP)) return;

                                vertices[0] = verticesOld[0] + diffP;
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;

                                // kiểm tra over1
                                if (vertices[6].x > widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[2] = peak2;
                                        isYOver1 = true;
                                    }
                                    vertices[6] = peak2 + (magDiffP - MovedDiffP1) * moveDirection;
                                    vertices[2] = verticesOld[2] + new Vector3(0, moveY - Moved1, 0);

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

                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);

                                    if (vertices[6].x > widthRatio) vertices[6].x = widthRatio + 0.1f;
                                }

                                // kiểm tra over2
                                if (vertices[7].y > heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[1] = peak1;
                                        isYOver2 = true;

                                    }
                                    vertices[7] = peak1 + (magDiffP - MovedDiffP2) * moveDirection;
                                    vertices[1] = verticesOld[1] + new Vector3(moveX - Moved2, 0, 0);


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

                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0);

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

                            else // Phần Cắt To
                            {
                                // Test xem có kéo ngược hướng không
                                if (IsPeakOver(verticesOld[3], magDiffP)) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[1] = verticesOld[1] + diffP;

                                vertices[2] = verticesOld[2] + diffP;
                                vertices[3] = verticesOld[3] + diffP;
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);
                                vertices[9] = vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);
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
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);
                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                        }
                        else
                        {
                            currentPos = mousePos;

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            Vector3 diff = currentPos - startPos;

                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = Vector2.SignedAngle(Vector2.down, lineSlicer.GetChild(0).transform.up); // change
                            float moveY = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // change
                            float moveX = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // change

                            if (!isUpdateOldDiff) // change
                            {

                                Moved1 = 2 * heightRatio * disp2 / disp1 - disp2; // x
                                MovedDiffP1 = 2 * (2 * heightRatio - disp1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * widthRatio * disp1 / disp2 - disp1; // y
                                MovedDiffP2 = 2 * (2 * widthRatio - disp2) * Mathf.Sin(angleA * Mathf.PI / 180);


                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart) // Phần Cắt bé
                            {
                                // kiểm tra điều kiện các góc ko cho kéo ngược hướng
                                if (IsPeakOver(verticesOld[1], magDiffP)) return;

                                vertices[1] = verticesOld[1] + diffP; // change
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;

                                // kiểm tra xem 6 có đi quá không : TH1
                                if (vertices[6].y < -heightRatio)
                                {

                                    if (!isYOver1)
                                    {

                                        vertices[0] = peak0; // change
                                        isYOver1 = true;

                                    }
                                    vertices[6] = peak0 + (magDiffP - MovedDiffP1) * moveDirection; // change
                                    vertices[0] = verticesOld[0] + new Vector3(moveX - Moved1, 0, 0); // change

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

                                    vertices[6] = verticesOld[6] - new Vector3(0, moveY, 0);  // change

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
                                    vertices[3] = verticesOld[3] - new Vector3(0, moveY - Moved2, 0);

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
                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0); // change

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
                                if (IsPeakOver(verticesOld[2], magDiffP)) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[0] = verticesOld[0] + diffP;
                                vertices[2] = verticesOld[2] + diffP;
                                vertices[3] = verticesOld[3] + diffP;
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOld[4] - new Vector3(0, moveY, 0);
                                vertices[9] = vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);
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
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);
                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                        }
                        else
                        {
                            currentPos = mousePos;

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

                                Moved1 = 2 * widthRatio * disp2 / disp1 - disp2; // y
                                MovedDiffP1 = 2 * (2 * widthRatio - disp1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * heightRatio * disp1 / disp2 - disp1; // x
                                MovedDiffP2 = 2 * (2 * heightRatio - disp2) * Mathf.Sin(angleA * Mathf.PI / 180);



                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart) // Phần Cắt bé
                            {
                                // kiểm tra điều kiện các góc ko cho kéo ngược hướng
                                if (IsPeakOver(verticesOld[3], magDiffP)) return;

                                vertices[3] = verticesOld[3] + diffP; // change
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;

                                // kiểm tra xem 6 có đi quá không : TH1
                                if (vertices[6].x < -widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[1] = peak1; // change
                                        isYOver1 = true;

                                    }
                                    vertices[6] = peak1 + (magDiffP + MovedDiffP1) * moveDirection; // change
                                    vertices[1] = verticesOld[1] + new Vector3(0, moveY + Moved1, 0); // change


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

                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);  // change
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
                                    vertices[2] = verticesOld[2] + new Vector3(moveX + Moved2, 0, 0);


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
                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0); // change

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
                                if (IsPeakOver(verticesOld[0], magDiffP)) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[0] = verticesOld[0] + diffP;
                                vertices[2] = verticesOld[2] + diffP;
                                vertices[1] = verticesOld[1] + diffP;
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);
                                vertices[9] = vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);
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
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);
                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                        }
                        else
                        {
                            currentPos = mousePos;

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

                                Moved1 = 2 * heightRatio * disp2 / disp1 - disp2; // x
                                MovedDiffP1 = 2 * (2 * heightRatio - disp1) * Mathf.Cos(angleA * Mathf.PI / 180);

                                Moved2 = 2 * widthRatio * disp1 / disp2 - disp1; // y
                                MovedDiffP2 = 2 * (2 * widthRatio - disp2) * Mathf.Sin(angleA * Mathf.PI / 180);


                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart) // Phần Cắt bé
                            {
                                // kiểm tra điều kiện các góc ko cho kéo ngược hướng
                                if (IsPeakOver(verticesOld[2], magDiffP)) return;

                                vertices[2] = verticesOld[2] + diffP; // change
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;

                                // kiểm tra xem 6 có đi quá không : TH1
                                if (vertices[6].y > heightRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[3] = peak3; // change
                                        isYOver1 = true;

                                    }
                                    vertices[6] = peak3 + (magDiffP + MovedDiffP1) * moveDirection; // change
                                    vertices[3] = verticesOld[3] + new Vector3(moveX + Moved1, 0, 0); // change


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

                                    vertices[6] = verticesOld[6] - new Vector3(0, moveY, 0);  // change
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
                                    vertices[0] = verticesOld[0] - new Vector3(0, moveY + Moved2, 0);

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
                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0); // change

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
                                if (IsPeakOver(verticesOld[1], magDiffP)) return;

                                UpdateMeshDataWhenChooseBiggerPart();

                                vertices[0] = verticesOld[0] + diffP;
                                vertices[1] = verticesOld[1] + diffP;
                                vertices[3] = verticesOld[3] + diffP;
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;
                                vertices[10] = vertices[6];
                                vertices[11] = vertices[7];

                                vertices[8] = vertices[4] = verticesOld[4] - new Vector3(0, moveY, 0);
                                vertices[9] = vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);
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
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);

                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                        }
                        else
                        {
                            currentPos = mousePos;
                            Debug.Log(startPos);
                            Debug.Log(currentPos);

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                                Debug.Log(diffBetweenCurandOldPos);
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diff = currentPos - startPos;


                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));


                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * widthRatio - disp2) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y
           
                                MovedDiffP2 = 2 * (2 * widthRatio - disp1) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP3 = 2 * (disp1) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y
                    
                                MovedDiffP4 = 2 * (disp2) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y


                            }
                            isUpdateOldDiff = true;


                            if (isChooseSmallPart)
                            {
                                UpdateMeshDataWhenClickSmallerPart();
                                if (IsPeakOver(verticesOld[1], magDiffP)) return; // change
                                if (IsPeakOver(verticesOld[0], magDiffP)) return; // change
                                Vector3 newPos6 = verticesOld[6] + new Vector3(moveX, 0, 0);
                                Vector3 newPos7 = verticesOld[7] + new Vector3(moveX, 0, 0);
                                // if (newPos6.x > widthRatio || newPos7.x > widthRatio) return;

                                vertices[0] = verticesOld[0] + diffP; // change
                                vertices[1] = verticesOld[1] + diffP; // change
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;

                                if (newPos7.x > widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[3] = peak3;
                                        isYOver1 = true;
                                    }
                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);
                                    vertices[7] = peak3 + (magDiffP - MovedDiffP1) * moveDirection;
                                    vertices[3] = verticesOld[3] + new Vector3(0, moveY - Moved1, 0);

                                }

                                else if (!isYOver2)
                                {
                                    if (isYOver1)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y - 0.001f, 0);
                                        isYOver1 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);
                                }


                                if (newPos6.x > widthRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[2] = peak2;
                                        isYOver2 = true;
                                    }
                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0);

                                    vertices[6] = peak2 + (magDiffP - MovedDiffP2) * moveDirection;
                                    vertices[2] = verticesOld[2] + new Vector3(0, moveY - Moved2, 0);

                                }

                                else if (!isYOver1)
                                {
                                    if (isYOver2)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y - 0.001f, 0);
                                        isYOver2 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);
                                }


                                UpdateVerticesBackSide();

                                // Update Mesh
                                if (vertices[7].x > widthRatio) UpdateMeshDataWhenOverPeak(1);
                                else if (vertices[6].x > widthRatio) UpdateMeshDataWhenOverPeak(2);
                                else UpdateMeshDataWhenClickSmallerPart();

                                uvs[6] = VerToUv(vertices[6]);
                                uvs[7] = VerToUv(vertices[7]);
                                uvs[3] = VerToUv(vertices[3]);
                                uvs[2] = VerToUv(vertices[2]);

                            }
                            else
                            {
                                UpdateMeshDataWhenChooseBiggerPart();
                                if (IsPeakOver(verticesOld[3], magDiffP)) return; // change
                                if (IsPeakOver(verticesOld[2], magDiffP)) return; // change
                                Vector3 newPos4 = verticesOld[4] + new Vector3(moveX, 0, 0);
                                Vector3 newPos5 = verticesOld[5] + new Vector3(moveX, 0, 0);
                                //if (newPos4.x < -widthRatio || newPos5.x < -widthRatio) return;

                                vertices[2] = verticesOld[2] + diffP; // change
                                vertices[3] = verticesOld[3] + diffP; // change
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;

                                if (newPos4.x < -widthRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[3] = peak3;
                                        isYOver3 = true;
                                    }
                                    vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);

                                    vertices[4] = peak0 + (magDiffP + MovedDiffP3) * moveDirection;
                                    vertices[0] = verticesOld[0] + new Vector3(0, moveY + Moved3, 0);

                                }

                                else if (!isYOver4)
                                {
                                    if (isYOver3)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);
                                }
                       

                                if (newPos5.x < -widthRatio)
                                {
                                    if (!isYOver4)
                                    {
                                        vertices[1] = peak1;
                                        isYOver4 = true;
                                    }
                                    vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);

                                    vertices[5] = peak1 + (magDiffP + MovedDiffP4) * moveDirection;
                                    vertices[1] = verticesOld[1] + new Vector3(0, moveY + Moved4, 0);

                                }

                                else if (!isYOver3)
                                {
                                    if (isYOver4)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);
                                }


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
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);
                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                         //   startPos = mousePos;
                            Debug.Log(startPos);
                   
                        }
                        else
                        {
                            currentPos = mousePos;

                            Debug.Log(startPos);
                            Debug.Log(currentPos);

                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                                Debug.Log(diffBetweenCurandOldPos);
                            }
                            currentPos -= diffBetweenCurandOldPos;
                            Debug.Log(currentPos);
                            diff = currentPos - startPos;

                            Debug.Log(diff);

                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));
                   
                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * heightRatio - disp2) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP2 = 2 * (2 * heightRatio - disp1) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP3 = 2 * (disp1) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP4 = 2 * (disp2) * Mathf.Sin(angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x


                            }
                            isUpdateOldDiff = true;

                            if (isChooseSmallPart)
                            {
                                UpdateMeshDataWhenClickSmallerPart();
                                if (IsPeakOver(verticesOld[1], magDiffP)) break; // change
                                if (IsPeakOver(verticesOld[3], magDiffP)) break; // change

                                Vector3 newPos6 = verticesOld[6] + new Vector3(0, moveY, 0);
                                Vector3 newPos7 = verticesOld[7] + new Vector3(0, moveY, 0);
                                //if (newPos6.y < -heightRatio || newPos7.y < -heightRatio) return;
                                vertices[1] = verticesOld[1] + diffP; // change
                                vertices[3] = verticesOld[3] + diffP; // change
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;


                                if (newPos7.y < -heightRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[2] = peak2;
                                        isYOver1 = true;
                                    }
                                    vertices[6] = verticesOld[6] + new Vector3(0, moveY, 0);
                                    vertices[7] = peak2 + (magDiffP + MovedDiffP1) * moveDirection;
                                    vertices[2] = verticesOld[2] + new Vector3(moveX + Moved1, 0, 0);

                                }

                                else if (!isYOver2)
                                {
                                    if (isYOver1)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y /*- 0.001f*/, 0);
                                        isYOver1 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(0, moveY, 0);
                                }



                                if (newPos6.y < -heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[0] = peak0;
                                        isYOver2 = true;
                                    }
                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0);

                                    vertices[6] = peak0 + (magDiffP + MovedDiffP2) * moveDirection;
                                    vertices[0] = verticesOld[0] + new Vector3(moveX + Moved2, 0, 0);

                                }

                                else if (!isYOver1)
                                {
                                    if (isYOver2)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver2 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(0, moveY, 0);
                                }

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
                                if (IsPeakOver(verticesOld[0], magDiffP)) break; // change
                                if (IsPeakOver(verticesOld[2], magDiffP)) break; // change

                                Vector3 newPos4 = verticesOld[4] + new Vector3(0, moveY, 0);
                                Vector3 newPos5 = verticesOld[5] + new Vector3(0, moveY, 0);
                                //if (newPos4.y > heightRatio || newPos5.y > heightRatio) return;


                                vertices[2] = verticesOld[2] + diffP; // change
                                vertices[0] = verticesOld[0] + diffP; // change
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;

                 
                                if (newPos4.y > heightRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[1] = peak1;
                                        isYOver3 = true;
                                    }
                                    vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);

                                    vertices[4] = peak1 + (magDiffP - MovedDiffP3) * moveDirection;
                                    vertices[1] = verticesOld[1] + new Vector3(moveX - Moved3, 0, 0);

                                }

                                else if (!isYOver4)
                                {
                                    if (isYOver3)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(0, moveY, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);
                                }



                                if (newPos5.y > heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[3] = peak3;
                                        isYOver4 = true;
                                    }
                                    vertices[4] = verticesOld[4] + new Vector3(0, moveY, 0);

                                    vertices[5] = peak3 + (magDiffP - MovedDiffP4) * moveDirection;
                                    vertices[3] = verticesOld[3] + new Vector3(moveX - Moved4, 0, 0);

                                }

                                else if (!isYOver3)
                                {
                                    if (isYOver4)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(0, moveY, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);
                                }

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
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);
                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                        }
                        else
                        {
                            currentPos = mousePos;


                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diff = currentPos - startPos;

                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));

                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * widthRatio - disp1) * Mathf.Cos(angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP2 = 2 * (2 * widthRatio - disp2) * Mathf.Cos(-angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP3 = 2 * (disp2) * Mathf.Cos(angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y

                                MovedDiffP4 = 2 * (disp1) * Mathf.Cos(angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Sin(angleA * Mathf.PI / 180)); // y


                            }
                            isUpdateOldDiff = true;

                            if (isChooseSmallPart)
                            {
                                UpdateMeshDataWhenClickSmallerPart();
                                if (IsPeakOver(verticesOld[2], magDiffP)) return; // change
                                if (IsPeakOver(verticesOld[3], magDiffP)) return; // change

                                vertices[2] = verticesOld[2] + diffP; // change
                                vertices[3] = verticesOld[3] + diffP; // change
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;

                                Vector3 newPos6 = verticesOld[6] + new Vector3(moveX, 0, 0);
                                Vector3 newPos7 = verticesOld[7] + new Vector3(moveX, 0, 0);
                                //if (newPos6.x < -widthRatio || newPos7.x < -widthRatio) return;

                                if (newPos6.x < -widthRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[1] = peak1;
                                        isYOver1 = true;
                                    }
                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0);

                                    vertices[6] = peak1 + (magDiffP + MovedDiffP1) * moveDirection;
                                    vertices[1] = verticesOld[1] + new Vector3(0, moveY + Moved1, 0);

                                }

                                else if (!isYOver2)
                                {
                                    if (isYOver1)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver1 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);
                                }


                                if (newPos7.x < -widthRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[0] = peak0;
                                        isYOver2 = true;
                                    }
                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);

                                    vertices[7] = peak0 + (magDiffP + MovedDiffP2) * moveDirection;
                                    vertices[0] = verticesOld[0] + new Vector3(0, moveY + Moved2, 0);

                                }

                                else if (!isYOver1)
                                {
                                    if (isYOver2)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver2 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(moveX, 0, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(moveX, 0, 0);
                                }


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
                                if (IsPeakOver(verticesOld[0], magDiffP)) break; // change
                                if (IsPeakOver(verticesOld[1], magDiffP)) break; // change

                                Vector3 newPos4 = verticesOld[4] + new Vector3(moveX, 0, 0);
                                Vector3 newPos5 = verticesOld[5] + new Vector3(moveX, 0, 0);
                            //    if (newPos4.x > widthRatio || newPos5.x > widthRatio) return;

                                vertices[0] = verticesOld[0] + diffP; // change
                                vertices[1] = verticesOld[1] + diffP; // change
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;

                                if (newPos5.x > widthRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[2] = peak2;
                                        isYOver3 = true;
                                    }
                                    vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);

                                    vertices[5] = peak2 + (magDiffP - MovedDiffP3) * moveDirection;
                                    vertices[2] = verticesOld[2] + new Vector3(0, moveY - Moved3, 0);

                                }

                                else if (!isYOver4)
                                {
                                    if (isYOver3)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);
                                }


                                if (newPos4.x > widthRatio)
                                {
                                    if (!isYOver4)
                                    {
                                        vertices[3] = peak3;
                                        isYOver4 = true;
                                    }
                                    vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);

                                    vertices[4] = peak3 + (magDiffP - MovedDiffP4) * moveDirection;
                                    vertices[3] = verticesOld[3] + new Vector3(0, moveY - Moved4, 0);

                                }

                                else if (!isYOver3)
                                {
                                    
                                    if (isYOver4)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(moveX, 0, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(moveX, 0, 0);
                                }


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
                            lineSlicer.gameObject.SetActive(false);
                            startPoint.gameObject.SetActive(false);
                            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                        }
                        else
                        {
                            currentPos = mousePos;


                            if (!isCalculatorDiff)
                            {
                                isCalculatorDiff = true;
                                diffBetweenCurandOldPos = currentPos - oldPos;
                            }
                            currentPos -= diffBetweenCurandOldPos;

                            diff = currentPos - startPos;

                            magDiffP = diff.magnitude * Mathf.Cos((Mathf.PI / 180) * (Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, diff)));
                            diffP = magDiffP * moveDirection;
                            float angleA = -Vector2.SignedAngle(lineSlicer.GetChild(0).transform.up, Vector2.right);
                            float moveX = magDiffP / (2 * Mathf.Cos(angleA * Mathf.PI / 180));
                            float moveY = magDiffP / (2 * Mathf.Sin(angleA * Mathf.PI / 180));

                            if (!isUpdateOldDiff)
                            {
                                MovedDiffP1 = 2 * (2 * heightRatio - disp1) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved1 = MovedDiffP1 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP2 = 2 * (2 * heightRatio - disp2) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved2 = MovedDiffP2 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP3 = 2 * (disp2) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved3 = MovedDiffP3 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // x

                                MovedDiffP4 = 2 * (disp1) * Mathf.Sin(-angleA * Mathf.PI / 180);
                                Moved4 = MovedDiffP4 / (2 * Mathf.Cos(angleA * Mathf.PI / 180)); // 


                            }
                            isUpdateOldDiff = true;
                            if (isChooseSmallPart)
                            {
                                UpdateMeshDataWhenClickSmallerPart();
                                if (IsPeakOver(verticesOld[0], magDiffP)) return; // change
                                if (IsPeakOver(verticesOld[2], magDiffP)) return; // change

                                Vector3 newPos6 = verticesOld[6] + new Vector3(0, moveY, 0);
                                Vector3 newPos7 = verticesOld[7] + new Vector3(0, moveY, 0);
                                //if (newPos6.y > heightRatio || newPos7.y > heightRatio) return;

                                vertices[0] = verticesOld[0] + diffP; // change
                                vertices[2] = verticesOld[2] + diffP; // change
                                vertices[4] = verticesOld[4] + diffP;
                                vertices[5] = verticesOld[5] + diffP;

                                if (newPos6.y > heightRatio)
                                {
                                    if (!isYOver1)
                                    {
                                        vertices[3] = peak3;
                                        isYOver1 = true;
                                    }
                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = peak3 + (magDiffP + MovedDiffP1) * moveDirection;
                                    vertices[3] = verticesOld[3] + new Vector3(moveX + Moved1, 0, 0);

                                }

                                else if (!isYOver2)
                                {
                                    if (isYOver1)
                                    {
                                        vertices[3] = new Vector3(peak3.x, peak3.y /*- 0.001f*/, 0);
                                        isYOver1 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(0, moveY, 0);
                                }



                                if (newPos7.y > heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[1] = peak1;
                                        isYOver2 = true;
                                    }
                                    vertices[6] = verticesOld[6] + new Vector3(0, moveY, 0);

                                    vertices[7] = peak1 + (magDiffP + MovedDiffP2) * moveDirection;
                                    vertices[1] = verticesOld[1] + new Vector3(moveX + Moved2, 0, 0);

                                }

                                else if (!isYOver1)
                                {
                                    if (isYOver2)
                                    {
                                        vertices[1] = new Vector3(peak1.x, peak1.y /*- 0.001f*/, 0);
                                        isYOver2 = false;
                                    }

                                    vertices[7] = verticesOld[7] + new Vector3(0, moveY, 0);
                                    vertices[6] = verticesOld[6] + new Vector3(0, moveY, 0);
                                }

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
                                if (IsPeakOver(verticesOld[1], magDiffP)) return; // change
                                if (IsPeakOver(verticesOld[3], magDiffP)) return; // change

                                Vector3 newPos4 = verticesOld[4] + new Vector3(0, moveY, 0);
                                Vector3 newPos5 = verticesOld[5] + new Vector3(0, moveY, 0);
                               // if (newPos4.y < -heightRatio || newPos5.y < -heightRatio) return;

                                vertices[1] = verticesOld[1] + diffP; // change
                                vertices[3] = verticesOld[3] + diffP; // change
                                vertices[6] = verticesOld[6] + diffP;
                                vertices[7] = verticesOld[7] + diffP;
                                if (newPos5.y < -heightRatio)
                                {
                                    if (!isYOver3)
                                    {
                                        vertices[0] = peak0;
                                        isYOver3 = true;
                                    }
                                    vertices[4] = verticesOld[4] + new Vector3(0, moveY, 0);

                                    vertices[5] = peak0 + (magDiffP - MovedDiffP3) * moveDirection;
                                    vertices[0] = verticesOld[0] + new Vector3(moveX - Moved3, 0, 0);

                                }

                                else if (!isYOver4)
                                {
                                    if (isYOver3)
                                    {
                                        vertices[0] = new Vector3(peak0.x, peak0.y /*- 0.001f*/, 0);
                                        isYOver3 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(0, moveY, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);
                                }



                                if (newPos4.y < -heightRatio)
                                {
                                    if (!isYOver2)
                                    {
                                        vertices[2] = peak2;
                                        isYOver4 = true;
                                    }
                                    vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);

                                    vertices[4] = peak2 + (magDiffP - MovedDiffP4) * moveDirection;
                                    vertices[2] = verticesOld[2] + new Vector3(moveX - Moved4, 0, 0);

                                }

                                else if (!isYOver3)
                                {
                                    if (isYOver4)
                                    {
                                        vertices[2] = new Vector3(peak2.x, peak2.y /*- 0.001f*/, 0);
                                        isYOver4 = false;
                                    }

                                    vertices[4] = verticesOld[4] + new Vector3(0, moveY, 0);
                                    vertices[5] = verticesOld[5] + new Vector3(0, moveY, 0);
                                }

                                UpdateVerticesBackSide();

                                if (vertices[5].y < -heightRatio) UpdateMeshDataWhenOverPeak(3);
                                else if (vertices[4].y < - heightRatio) UpdateMeshDataWhenOverPeak(4);
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
    private bool IsPeakOver(Vector3 peak, float diffu)
    {
        Vector3 test = peak + diffu * new Vector3(lineSlicer.GetChild(0).transform.up.x, lineSlicer.GetChild(0).transform.up.y, 0); // change




        if ((test.x <= -widthRatio && test.y <= -heightRatio) || (test.x <= -widthRatio && test.y >= heightRatio) || (test.x >= widthRatio && test.y <= -heightRatio) || (test.x >= widthRatio && test.y >= heightRatio))
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
                        //4,5,6,
                        //6,5,7,
                        //6,7,1,
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
                        //4,5,6,
                        //6,5,2,
                        //2,5,7,
                        //2,7,1,
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
                         
                         //4,5,6,
                         //6,5,2,
                         //2,5,7,
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
                        //4,5,6,
                        //6,5,7,
                        //6,7,1,
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
                        //4,5,6,
                        //6,5,2,
                        //2,5,7,
                        //2,7,1,
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
                        //4,5,6,
                        //6,5,7,
                        //6,7,1,
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
                        //4,5,6,
                        //6,5,2,
                        //2,5,7,
                        //2,7,1,
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
    private void SortP1P2Position()
    {
        switch (casePos)
        {
            case eCasePos.TH1:
                {

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak0));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak0));

                    break;
                }
            case eCasePos.TH2:
                {

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak1));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak1));

                    break;
                }
            case eCasePos.TH3:
                {

                    swapP1P2();

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak3));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak3));

                    break;
                }
            case eCasePos.TH4:
                {

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak2));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak2));



                    break;

                }
            case eCasePos.TH5:
                {

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak0));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak1));



                    break;
                }
            case eCasePos.TH6:
                {
                    swapP1P2();

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak1));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak3));




                    break;
                }
            case eCasePos.TH7:
                {
                    swapP1P2();

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak3));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak2));



                    break;
                }
            case eCasePos.TH8:
                {

                    disp1 = Vector2.Distance(v3tov2(point1), v3tov2(peak2));
                    disp2 = Vector2.Distance(v3tov2(point2), v3tov2(peak0));


                    break;
                }
            default: break;
        }



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

        float x = UnityEngine.Random.Range(0, 1000f);
        float y = UnityEngine.Random.Range(0, 1000f);

        for (int i = 8; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(x, y);
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
            if (a != new Vector2(float.MaxValue, float.MaxValue) && Mathf.Abs(a.x) <= width && Mathf.Abs(a.y) <= height)
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
                else if (insteadEdge0 * insteadEdge1 > 0 )
                {
                    Debug.Log("TH5"); casePos = eCasePos.TH5;
                }
                else if (insteadEdge0 * insteadEdge1 < 0 ) 
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
        if (isSliced)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(vertices[1], 0.5f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(vertices[3], 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(vertices[6], 0.5f);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[7], 0.5f);
        }



    }
}
