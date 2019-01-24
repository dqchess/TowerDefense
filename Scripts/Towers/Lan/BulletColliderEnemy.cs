using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColliderEnemy : MonoBehaviour {

    public int attackDamage;
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.layer == 8)
            {
                col.gameObject.GetComponentInChildren<Demon>().SubHealth(attackDamage);
                Destroy(gameObject);
            }

            if (col.gameObject.layer == 9)
            {
                col.gameObject.GetComponentInChildren<Dragon>().SubHealth(attackDamage);
                Destroy(gameObject);
            }
            if (col.gameObject.layer == 14)
            {
                col.gameObject.GetComponentInChildren<OskBane>().SubHealth(attackDamage);
                Destroy(gameObject);
            }
            if (col.gameObject.layer == 15)
            {
                col.gameObject.GetComponentInChildren<IceDemon>().SubHealth(attackDamage);
                Destroy(gameObject);
            }
            if (col.gameObject.layer == 16)
            {
                col.gameObject.GetComponentInChildren<IceDemonChild>().SubHealth(attackDamage);
                Destroy(gameObject);
            }
            if (col.gameObject.layer == 17)
            {
                col.gameObject.GetComponentInChildren<Destroyer>().SubHealth(attackDamage);
                Destroy(gameObject);
            }
        }
    }
}
