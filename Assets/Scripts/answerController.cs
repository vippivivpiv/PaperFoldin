using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class answerController : MonoBehaviour
{
    public UILabel Ans0, Ans1, Ans2, Ans3;
    public static bool isFindAns0, isFindAns1, isFindAns2, isFindAns3;

    public void  DisplayFindedAnswer()
    {
        if (isFindAns0) Ans0.color = Color.green;
        else Ans0.color = Color.red;
        if (isFindAns1) Ans1.color = Color.green;
        else Ans1.color = Color.red;
        if (isFindAns2) Ans2.color = Color.green;
        else Ans2.color = Color.red;
        if (isFindAns3) Ans3.color = Color.green;
        else Ans3.color = Color.red;
    }
}
