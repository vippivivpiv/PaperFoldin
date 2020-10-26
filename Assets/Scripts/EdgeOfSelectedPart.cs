using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeOfSelectedPart : MonoBehaviour
{
    public Slice169 slice169;
    public LineRenderer lineRenderer;

    void Update()
    {


        if (slice169.isSliced)

        {

            if (slice169.isClick)
            {

                //gameObject.SetActive(true);
                lineRenderer.enabled = true;
                lineRenderer.positionCount = slice169.edgeSliced.Length;
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    lineRenderer.SetPosition(i, slice169.edgeSliced[i]);
                }
            }


        }
    }
}
