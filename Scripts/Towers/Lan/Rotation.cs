using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    public GameObject rotation;
    Animator an;
    Vector3 dir;
    float rotSpeed = 90f;
    float zAngle;
    void Start()
    {
        an = rotation.GetComponent<Animator>();
    }


    void Update()
    {
        Quaternion desiredRot = Quaternion.Euler(0, 0, 5f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }
}
