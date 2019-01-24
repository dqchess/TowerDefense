using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cell : MonoBehaviour {

    public int cellIndex;
    public GameObject mainCamera;
    public GameObject heroCamp;
    public GameObject heroCell;
    public GameObject heroList;
    private HeroCell classHC;
    Vector3 heroCampPosision;

    private void Awake()
    {
        classHC = heroCell.GetComponent<HeroCell>();
        cellIndex = Int32.Parse(gameObject.name);
    }

    private void OnMouseDown()
    {
        classHC.SelectedCellIndex = cellIndex;
        heroCampPosision = heroCamp.transform.position;
        mainCamera.transform.position = new Vector3(heroCampPosision.x, heroCampPosision.y, -10);
        if (transform.transform.childCount != 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        UpdateHeroList();
    }

    private void UpdateHeroList()
    {
        for (int i = 0; i < classHC.checkHeroExist.Length ; i++)
        {
            if (classHC.checkHeroExist[i])
            {
                heroList.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                heroList.transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                heroList.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                heroList.transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}
