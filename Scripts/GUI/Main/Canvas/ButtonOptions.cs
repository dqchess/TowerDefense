using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOptions : MonoBehaviour {

    public GameObject optionsBoard;
    public GameObject startBoard;
    public GameObject pointMap;
    private Animator animOptionsBoard;
    private bool check;
    private AudioSource pressSound;


    private void Awake()
    {
        pressSound = GetComponent<AudioSource>();
        animOptionsBoard = optionsBoard.GetComponent<Animator>();
        check = false;
    }

    public void ShowOptions()
    {
        optionsBoard.SetActive(true);
        UnactiveBoxColliderPointMap();
    }
    
    public void HideOptions()
    {
        animOptionsBoard.SetTrigger("isHideOptions");
        ReactiveBoxColliderPointMap();
    }

    public void OnClick()
    {
        pressSound.Play();
        if (!startBoard.activeInHierarchy)
        {
            if (!check)
            {
                ShowOptions();
                check = true;
            }
            else
            {
                HideOptions();
                check = false;
            }
        }
    }

    private void UnactiveBoxColliderPointMap()
    {
        for (int i = 0; i < pointMap.transform.childCount; i++)
        {
            pointMap.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void ReactiveBoxColliderPointMap()
    {
        for (int i = 0; i < pointMap.transform.childCount; i++)
        {
            pointMap.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
        }
    }

}
