using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhibit : MonoBehaviour
{
    public int sttExh;
    public UILabel numberOfExh;
    public UIGrid grid;

    public SelectMapUI SelectMapUI;

    public SelectMapFrameUI SelectMapFrameUIprefab;
    private void OnEnable()
    {


    }


    public void InstantiateExh()
    {
        Debug.Log(sttExh);
        for (int i = 1; i < 10; i++)
        {
            SelectMapFrameUI sss = Instantiate(SelectMapFrameUIprefab, grid.transform);
            sss.gameObject.name = "Map" + ((sttExh - 1) * 10 + i).ToString();
            sss.indexOfMap = (sttExh - 1) * 10 + i;
           // sss.nameOfMap = ( (sttExh - 1) *9 + i).ToString();
            sss.nameOfMap = DataPlayer.Get10to9(sss.indexOfMap).ToString();
            Debug.Log(this.SelectMapUI);
            sss.SelectMapUI = this.SelectMapUI;

            sss.UpdateStateOfMap();
        }

        GameObject n = new GameObject();
        Instantiate(n, grid.transform);

        SelectMapFrameUI s = Instantiate(SelectMapFrameUIprefab, grid.transform);
        s.gameObject.name = "Map" + ((sttExh - 1) * 10 + 10).ToString();
        s.indexOfMap = (sttExh - 1) * 10 + 10;
       // s.nameOfMap = "Special" + sttExh.ToString();
        s.nameOfMap = "Special" + DataPlayer.Get10to9(s.indexOfMap).ToString();
        s.SelectMapUI = this.SelectMapUI;

        s.UpdateStateOfMap();

        grid.enabled = false;
        grid.enabled = true;
    }



}
