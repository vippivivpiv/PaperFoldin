using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class testtttttDiemVaDaGiac : MonoBehaviour
{
   public  Camera main;
    public Transform[] point;
    public Vector3 mousePosition;

    void Update()
    {
       // Debug.Log(Input.mousePosition);
        if ( Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition);
            Debug.Log(main.ScreenToWorldPoint(Input.mousePosition));
            mousePosition = main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Debug.Log(mousePosition);
            Calculate();
        }
    }

    private void Calculate()
    {
        float angle = 0;

        for (int i = 0; i < point.Length-1; i++)
        {
            angle += Vector3.Angle(point[i + 1].position - mousePosition, point[i].position - mousePosition);
            Debug.Log(angle);
        }
        angle += Vector3.Angle(point[0].position - mousePosition, point[point.Length-1].position - mousePosition);
        Debug.Log(angle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(mousePosition, 0.5f);
    }
}
