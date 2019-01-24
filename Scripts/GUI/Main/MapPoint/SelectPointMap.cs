using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SelectPointMap : MonoBehaviour {

    public GameObject startMapBoard;
    public GameObject optionsBoard;
    public GameObject btnStart;
    public GameObject pointMap;
    public Text mapName;
    private ButtonStart classBS;
    private Scene sceneWillBeLoad;
    private AudioSource pressSound;
    private string[] sceneName = new string[20];

    private void Awake()
    {
        pressSound = gameObject.GetComponent<AudioSource>();
        classBS = btnStart.GetComponent<ButtonStart>();
        for (int i = 0; i < sceneName.Length; i++)
        {
            sceneName[i] = "Map Level " + i.ToString();
        }
    }
    private void OnMouseDown()
    {
        if (!startMapBoard.activeInHierarchy && !optionsBoard.activeInHierarchy)
        {
            pressSound.Play();
            startMapBoard.SetActive(true);
            classBS.sceneIndex = Int32.Parse(gameObject.name);
            mapName.gameObject.SetActive(true);
            mapName.text = sceneName[classBS.sceneIndex];
            UnactiveBoxColliderPointMap();
        }
    }

    private void UnactiveBoxColliderPointMap()
    {
        for (int i = 0; i < pointMap.transform.childCount; i++)
        {
            pointMap.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
