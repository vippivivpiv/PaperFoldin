using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMousePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown(0))
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString("F4"));
        }
        //[Log] (2) (-1.6535, 8.9254, -10.0000)

        // (0.4838, -8.0962, -10.0000)  (0.4875, 2.5531, -10.0000)

    }
}
