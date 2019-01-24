using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour
{
    private Transform[] child;
    private int[] order;
    private SpriteRenderer sr;
    private SpriteRenderer[] srChild;

    void Awake()
    {
        child = new Transform[transform.childCount];
        srChild = new SpriteRenderer[transform.childCount];
        order = new int[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            child[i] = transform.GetChild(i);

            srChild[i] = child[i].gameObject.GetComponent<SpriteRenderer>();
            if (srChild[i] != null)
            {
                order[i] = srChild[i].sortingOrder;
            }
        }
    }

    void Update()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sortingOrder = (int)(-gameObject.transform.position.y * 100);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (srChild[i] != null)
            {
                srChild[i].sortingOrder = order[i] + sr.sortingOrder;
            }
        }
    }

}
