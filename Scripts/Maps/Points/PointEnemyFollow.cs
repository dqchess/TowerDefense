using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEnemyFollow : MonoBehaviour
{

    public Transform[] pointTransform;

    void Awake()
    {
        pointTransform = new Transform[transform.childCount];
        for (int i = 0; i < pointTransform.Length; i++)
        {
            pointTransform[i] = transform.GetChild(i);
        }
    }
}
