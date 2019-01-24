using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    
    Animator anim;

    public GameObject towerMenu;
    public GameObject buildSpot;
    public GameObject nextLevelTower;

    public int towerCost;

    int refundAmount;

    GameObject UIGamePlay;

    float currentTime;
    bool isSelected;
    
    void Start()
    {
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        UIGamePlay = GameObject.Find("UIGamePlay");
        refundAmount = (int)(towerCost * 0.3f);
        //towerMenu.transform.position = gameObject.transform.position;
        //towerMenu.transform.rotation = gameObject.transform.rotation;
    }

    void Update()
    {
        CheckForInput();
    }

    void OnMouseOver()
    {
        isSelected = true;
        if (Input.GetMouseButtonDown(0))
        {
            towerMenu.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        isSelected = false;
    }

    void RefundTower(int refundValue)
    {
        //-------
        Debug.Log("Refund tower! refundValue = " + refundValue);
        Instantiate(buildSpot, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        UIGamePlay.GetComponent<UIGamePlay>().towerCurrency += (towerCost - refundAmount);
        Destroy(this.gameObject);
        return;
        //-------
    }

    void UpgradeTower(int upgradeValue)
    {
        //-------
        Debug.Log("Upgrade tower! upgradeValue = " + upgradeValue);
        if (nextLevelTower != null && UIGamePlay.GetComponent<UIGamePlay>().towerCurrency >= upgradeValue)
        {
            UIGamePlay.GetComponent<UIGamePlay>().towerCurrency -= upgradeValue;
            Instantiate(nextLevelTower, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Not enough money or your tower already max level!");
        }
        return;
        //-------
    }

    void CheckForInput()
    {
        if (Input.GetMouseButtonDown(0) && isSelected == false)
        {
            towerMenu.SetActive(false);
        }
    }
}
