using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE: MonoBehaviour {
    public float radius;
    public int damage;
    public GameObject explosion;
    public float explosionDuration;
    private bool isQuitting;


    void Start()
    {
        isQuitting = false;
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach(Collider2D col in cols)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    if (col.gameObject.layer == 8)
                    {
                        col.gameObject.GetComponentInChildren<Demon>().SubHealth(damage);
                        Destroy(gameObject);
                    }

                    if (col.gameObject.layer == 9)
                    {
                        col.gameObject.GetComponentInChildren<Dragon>().SubHealth(damage);
                        Destroy(gameObject);
                    }
                    if (col.gameObject.layer == 14)
                    {
                        col.gameObject.GetComponentInChildren<OskBane>().SubHealth(damage);
                        Destroy(gameObject);
                    }
                    if (col.gameObject.layer == 15)
                    {
                        col.gameObject.GetComponentInChildren<IceDemon>().SubHealth(damage);
                        Destroy(gameObject);
                    }
                    if (col.gameObject.layer == 16)
                    {
                        col.gameObject.GetComponentInChildren<IceDemonChild>().SubHealth(damage);
                        Destroy(gameObject);
                    }
                    if (col.gameObject.layer == 17)
                    {
                        col.gameObject.GetComponentInChildren<Destroyer>().SubHealth(damage);
                        Destroy(gameObject);
                    }
                }
            }

        }
    }

    private void SceneQuite(GameObject obj, string param)
    {
        isQuitting = true;
    }
}
