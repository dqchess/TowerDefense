using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGate : MonoBehaviour {

    GameObject UIGamePlay;

    private void Start()
    {
        UIGamePlay = GameObject.Find("UIGamePlay");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            UIGamePlay.GetComponent<UIGamePlay>().health -= 1;
        }
    }

}
