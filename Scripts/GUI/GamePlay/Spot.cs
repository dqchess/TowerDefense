using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    public GameObject builder;
    private Animator anim;
    private bool shouldBeHide;

    void Start()
    {
        anim = builder.GetComponent<Animator>();
    }

    void Update()
    {
        if (shouldBeHide && Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Hide", true);
            shouldBeHide = false;
        }
    }
    void OnMouseDown()
    {
        Debug.Log("focus");
        builder.SetActive(true);
        anim.SetBool("Hide", false);
    }

    void OnMouseExit()
    {
        shouldBeHide = true;
    }

    //void OnMouseExit()
    //{
    //    anim.SetBool("Hide", true);
    //    StartCoroutine(Wait());
    //}

    //IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(1.2f);
    //    builder.SetActive(false);
    //}
}
