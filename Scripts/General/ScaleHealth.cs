using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleHealth : MonoBehaviour
{
    private Vector3 pos;
    private SpriteRenderer sr;
    private float halfPosX;
    private float posX;

    void Awake()
    {
        pos = transform.localPosition;
        halfPosX = (float)transform.localPosition.x / 2;
        posX = transform.localPosition.x;

        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.parent.localScale.x < 0)
        {
            sr.flipX = true;
            pos.x = -posX / 2;
            transform.localPosition = pos;
        }
        else
        {
            sr.flipX = false;
            pos.x = posX;
            transform.localPosition = pos;
        }
    }
}
