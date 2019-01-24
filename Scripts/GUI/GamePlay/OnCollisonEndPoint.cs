using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCollisonEndPoint : MonoBehaviour {

    public Text LiveText;
    public Text GoldText;
    public static int tempLive;
    public static int tempGold;
    

    void Awake()
    {
        tempGold = 200;
        tempLive = 20;
        LiveText.text = "20";
        GoldText.text = "200";
    }

    void Update()
    {
        LiveText.text = tempLive.ToString(); 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Dragon")
        {
            tempLive -= 1;
        }
        if (tempLive<=0)
        { Debug.Log("GAME OVER!"); }
    }
}
