using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBullet : MonoBehaviour {

    public GameObject bulletEffect;

	void Start () {
		
	}
	
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            removeForce();
            Instantiate(bulletEffect, other.transform.position, Quaternion.identity);
        }
    }

    void removeForce()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
