using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayProcess : MonoBehaviour
{
    public GameObject answerFindedDisplay;
    public UILabel labelCurrentStage;

    public static int numberOfCurrentStage;

    public List<GameObject> image;
    private GameObject currentStageGoj;
    private bool isDisplayPanelConfirmSlice;



    void Start()
    {
        numberOfCurrentStage = 0;
        currentStageGoj = GameObject.Instantiate(image[numberOfCurrentStage], gameObject.transform);
        currentStageGoj.SetActive(true);
        labelCurrentStage.text = numberOfCurrentStage.ToString();
    }

    private void Update()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
    }


    public void StartGame()
    {





    }




    public void UpdateTextOfCurrentStage()
    {
        labelCurrentStage.text = numberOfCurrentStage.ToString();
    }
    public void FindedAnswer()
    {
        answerFindedDisplay.GetComponent<answerController>().DisplayFindedAnswer();
    }

    public void ClickNextStage()
    {
        if (numberOfCurrentStage + 1 > image.Count - 1) return;
        numberOfCurrentStage++;
        LoadStage();
    }



    public void ClickPrevStage()
    {
        if (numberOfCurrentStage - 1 < 0) return;
        numberOfCurrentStage--;
        LoadStage();
    }

    public void LoadStage()
    {
        //SceneManager.LoadScene(0);
        GameObject.Destroy(currentStageGoj);
        currentStageGoj = GameObject.Instantiate(image[numberOfCurrentStage], gameObject.transform);

    }

    public void ClickReplay()
    {
        //edgeSliced.enabled = false;

        GameObject.Destroy(currentStageGoj);
        currentStageGoj = GameObject.Instantiate(image[numberOfCurrentStage], gameObject.transform);
        currentStageGoj.SetActive(true);

    }

    public void ClickReloadScreen()
    {
        SceneManager.LoadScene(0);

    }



}
