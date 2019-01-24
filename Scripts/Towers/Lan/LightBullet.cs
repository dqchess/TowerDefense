using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBullet : BulletColliderEnemy {

    public Vector3 target;
    public float speedBullet;
    public float TIME_ALIVE = 0.5f;

    void Start()
    {
        Destroy(gameObject, TIME_ALIVE);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speedBullet);
        /* if (Target != null)
         {
             gameObject.GetComponent<Rigidbody2D>().velocity = calcBallisticVelocityVector(transform, Target.transform, 70f);
         }
         else return;*/
    } 
}
