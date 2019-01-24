using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCloseStartBoard : MonoBehaviour {

    public GameObject startBoard;
    public GameObject pointMap;
    public Text mapName;
    private AudioSource pressSound;
    private Animator anim;

    private void Awake()
    {
        anim = startBoard.GetComponent<Animator>();
        pressSound = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        pressSound.Play();
        anim.SetTrigger("Hide");
        mapName.gameObject.SetActive(false);
        ReActiveBoxColliderPointMap();
    }

    private void ReActiveBoxColliderPointMap()
    {
        for (int i = 0; i < pointMap.transform.childCount; i++)
        {
            pointMap.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
