﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBullet:BulletColliderEnemy {

    public Vector3 target;
    public GameObject Target;
    public float speedBullet;
    public float TIME_ALIVE = 0.5f;

    void Start () {
        Destroy(gameObject, TIME_ALIVE);
    }

    void Update () {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speedBullet);
            if (Target != null)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = calcBallisticVelocityVector(transform, Target.transform, 70f);
            }
            else return;
    }

    Vector3 calcBallisticVelocityVector(Transform source, Transform target, float angle)
    {
        Vector3 direction = target.position - source.position; 
        float h = direction.y;
        direction.y = 0; 
        float distance = direction.magnitude; 
        float a = angle * Mathf.Deg2Rad; 
        direction.y = distance * Mathf.Tan(a); 
        distance += h / Mathf.Tan(a); 
        if (distance < 0) distance *= -1;
        float velocity = Mathf.Sqrt(distance * Physics2D.gravity.magnitude / Mathf.Sin(2 * a));

        return velocity * direction.normalized;
    }

   
}
