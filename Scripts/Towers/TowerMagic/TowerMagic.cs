using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMagic : MonoBehaviour
{

    public GameObject positionFireBullet;
    public GameObject magic;
    public GameObject effect;
    public GameObject startEffect;

    private const float TIME_RETURN_FIRE = 2f;
    private float nextFire;
    private bool isLockTarget;

    private GameObject gameObjectTarget;

    void Awake()
    {
        nextFire = Time.time;
    }

    void Update()
    {
        if (Time.time > nextFire && isLockTarget)
        {
            Instantiate(effect, startEffect.transform.position, 
                Quaternion.Euler(new Vector3(0, 0, 0)));

            GameObject bulletMagic = (GameObject)Instantiate(magic, positionFireBullet.transform.position, 
                                         Quaternion.Euler(new Vector3(0, 0, 0)));

            BulletMagic bullet = bulletMagic.GetComponentInChildren<BulletMagic>();
            if (bullet != null)
            {
                bullet.SetTarget(gameObjectTarget);
            }

            nextFire = Time.time + TIME_RETURN_FIRE;
        }

        if (gameObjectTarget == null)
            isLockTarget = false;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && !isLockTarget)
        {
            gameObjectTarget = coll.gameObject;
            isLockTarget = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && isLockTarget)
        {
            isLockTarget = false;
        }
    }
}
