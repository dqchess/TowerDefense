using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeTarget : MonoBehaviour {

    public GameObject rotation;
   // Animator an;
    Vector3 dir;
    float rotSpeed = 90f;
    float zAngle;
    public GameObject target;
	void Start () {
      //  an = rotation.GetComponent<Animator>();
	}
	
	
	void Update () {
        if (target == null)
        {
            GameObject _target = GameObject.FindGameObjectWithTag("Enemy");
            if (_target != null)
            {
                target = _target;
            }
        }
        if (target == null) return;

        dir = target.transform.position - transform.position;
        dir.Normalize();
        zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg-90;
        if (zAngle > 0)
        {
            float deg = -360 + zAngle;
           // an.SetFloat("deg", deg);
        }
        else
        {
           // an.SetFloat("deg", zAngle);
        }
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
        transform.localScale = new Vector3(4f, 2.5f, 0f);
	}
}
