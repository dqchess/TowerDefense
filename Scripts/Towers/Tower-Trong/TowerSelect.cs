using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelect : MonoBehaviour {

    public GameObject towerType;
    GameObject UIGamePlay;
    int towerCurrency;
    int towerCost;

    void Start()
    {
        UIGamePlay = GameObject.Find("UIGamePlay");
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Building tower...");

            // Debug.Log("On mouse over --- Tower Select");
            towerCurrency = UIGamePlay.GetComponent<UIGamePlay>().towerCurrency;
            // get tower info in script
            towerCost = towerType.GetComponent<Tower>().towerCost;

            if (towerCurrency >= towerCost)
            {
                UIGamePlay.GetComponent<UIGamePlay>().towerCurrency -= towerCost;
                SendMessageUpwards("ActiveProcessBar", towerType);
                Debug.Log("Active done!");
            }
            else
            {
                Debug.Log("You dont have enough money to build that!");
                Debug.Log("Tower Price: " + towerCost);
                Debug.Log("Money left: " + towerCurrency);
            }

        }
    }
}
