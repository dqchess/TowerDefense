using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire : MonoBehaviour
{

    private const float TIME_RETURN_BURNING = 1f;
    private const float TIME_DESTROY = 7f;
    private float nextTimeStartDamaging;
    private float nextTimeStart;
    private float damage;
    private float heightHellFire;

    void Awake()
    {
        nextTimeStart = Time.time;
        nextTimeStartDamaging = Time.time;
        heightHellFire = gameObject.GetComponent<Renderer>().bounds.size.y;
    }

    void Update()
    {
        if (Time.time - nextTimeStart > TIME_DESTROY)
            Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Hero"))
        {
            Fire(coll.gameObject);
        }
        else if (coll.gameObject.CompareTag("Army"))
        {
            Fire(coll.gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage / 10;
    }

    private void Fire(GameObject gob)
    {
        Transform tran = gob.transform;

        if (Time.time - nextTimeStartDamaging >= 0)
        {
            if ((tran.position.y <= transform.position.y + heightHellFire)
                && (tran.position.y >= transform.position.y - heightHellFire))
            {

                if (gob.layer == 10)
                    gob.GetComponentInChildren<EarthShaker>().SubHealth(damage);
                else if (gob.layer == 12)
                    gob.GetComponentInChildren<NagaSiren>().SubHealth(damage);
                else if (gob.layer == 11)
                    gob.GetComponentInChildren<Dwarf>().SubHealth(damage);
            }

            nextTimeStartDamaging = Time.time + TIME_RETURN_BURNING;
        }
    }
}
