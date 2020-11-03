using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoGiay : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triagles;
    private Vector2[] uvs;
    // private Vector3 peak0, peak1, peak2, peak3, mid1, mid2;
    public Transform peak0, peak1, peak2, peak3, mid1, mid2;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        //peak2= peak0 = new Vector3(0, -5, -5);
        //peak3= peak1 = new Vector3(0, 5, -5);

        //mid1 = new Vector3(0, -5, 0);
        //mid2 = new Vector3(0, 5, 5);

        //triagles = new int[]
        //{
        //    0,1,4,
        //    4,1,5,
        //    2,4,5,
        //    2,5,3,
        //};
        triagles = new int[]
        {
            0,1,2,
            2,1,3,
        };
        uvs = new Vector2[6];

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(1, 1);
        //uvs[4] = new Vector2(0.5f, 0);
        //uvs[5] = new Vector2(0.5f, 1);


    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateVerticesPos();
        CreateMesh();
    }


    private void UpdateVerticesPos()
    {
        vertices = new Vector3[6];
        vertices[0] = peak0.transform.position;
        vertices[1] = peak1.transform.position;
        vertices[2] = peak2.transform.position;
        vertices[3] = peak3.transform.position;
        vertices[4] = mid1.transform.position;
        vertices[5] = mid2.transform.position;
    }
    private void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triagles;
        mesh.uv = uvs;

        //mesh.colors = new Color[] { natural, natural, natural, natural };
    }
}
