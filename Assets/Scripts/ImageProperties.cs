using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProperties : MonoBehaviour
{
    public string KeyWord;
    public Sprite Image;
    public Sprite BackAnswer;
    public Material AnswerMaterial, OriginalMaterial;

    private Slice169 slice169;

    [Header("Answer")]
    public Vector2 point1;
    public Vector2 point2;
    public float angle;
    [Header("WinCondition")]
    public float distanceSlice;
    public float distanceWin;
    public float holdTime;
    [Header("AnswerDisplay")]
    public GameObject spritePoint1;
    public GameObject spritePoint2;
    public GameObject spriteDisplay;




    private void Start()
    {
        slice169 = GetComponentInParent<Slice169>();
        ShowOrHideAnswer();

        spriteDisplay.GetComponent<SpriteRenderer>().sprite = BackAnswer;
    }

    public void ShowOrHideAnswer()
    {
        if (!DataPlayer.ShowAnswer) slice169.meshRenderer.material = OriginalMaterial;
        else slice169.meshRenderer.material = AnswerMaterial;
    }






    string name;
    bool isFinded;
    
    Vector3 point1ImagePos;
    Vector3 point2ImagePos;
    Vector3 displayImagePos;

    
    float disP1toLine;
    float disP2toLine;
    Vector2 oldPosPointMove;


    public Vector2 Point1 { get => point1; set => point1 = value; }
    public Vector2 Point2 { get => point2; set => point2 = value; }
    public float Angle { get => angle; set => angle = value; }
    public string Name { get => name; set => name = value; }
    public float DisP1toLine { get => disP1toLine; set => disP1toLine = value; }
    public float DisP2toLine { get => disP2toLine; set => disP2toLine = value; }
    public Vector2 OldPosPointMove { get => oldPosPointMove; set => oldPosPointMove = value; }
    public Vector3 Point1ImagePos { get => point1ImagePos; set => point1ImagePos = value; }
    public Vector3 Point2ImagePos { get => point2ImagePos; set => point2ImagePos = value; }
    public Vector3 DisplayImagePos { get => displayImagePos; set => displayImagePos = value; }
    public bool IsFinded { get => isFinded; set => isFinded = value; }


    public ImageProperties(string name, Vector2 p1, Vector2 p2)
    {
        this.name = name;
        point1 = p1;
        point2 = p2;
    }
}
