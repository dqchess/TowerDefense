using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSkill : MonoBehaviour
{

    public AudioClip soundEarthSkill;
    private EarthShaker es;
    private AudioSource source;

    void Awake()
    {
        GameObject gameObjectES = GameObject.Find("ES");
        es = gameObjectES.GetComponentInChildren<EarthShaker>();
        source = GetComponent<AudioSource>();

        source.PlayOneShot(soundEarthSkill, Random.Range(0.3f, 0.6f));
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            if (coll.gameObject.layer == 8)
            {
                coll.gameObject.GetComponentInChildren<Demon>().SubHealth(es.damageEarthSkill);
            }
            else if (coll.gameObject.layer == 14)
            {
                coll.gameObject.GetComponentInChildren<OskBane>().SubHealth(es.damageEarthSkill);
            }
            else if (coll.gameObject.layer == 15)
            {
                coll.gameObject.GetComponentInChildren<IceDemon>().SubHealth(es.damageEarthSkill);
            }
            else if (coll.gameObject.layer == 16)
            {
                coll.gameObject.GetComponentInChildren<IceDemonChild>().SubHealth(es.damageEarthSkill);
            }
            else if (coll.gameObject.layer == 17)
            {
                coll.gameObject.GetComponentInChildren<Destroyer>().SubHealth(es.damageEarthSkill);
            }
        }
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
