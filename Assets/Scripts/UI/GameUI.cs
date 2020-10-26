using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject Panel;

    public void Show()
    {
        Panel.SetActive(true);
    }

    public void Hide()
    {
        Panel.SetActive(false);
    }
}
