using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchedEarth : MonoBehaviour
{

    public AudioClip soundSkill;
    private AudioSource source;
    private const float RANGE = 2f;
    private const float TIME_RETURN_BURN = 1f;
    private float damage;
    private float c;
    private float timeNextBurn;
    private float timeNextSound;
    private float lengthSound;

    void Awake()
    {
        timeNextBurn = Time.time;
        timeNextSound = Time.time;
        source = GetComponent<AudioSource>();
        lengthSound = soundSkill.length;
    }

    void Update()
    {
        GameObject[] armies = GameObject.FindGameObjectsWithTag("Army");
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");

        if (Time.time - timeNextBurn >= 0)
        {
            for (int i = 0; i < armies.Length; i++)
            {
                if (Vector2.Distance(armies[i].transform.position, transform.position) <= RANGE)
                {
                    if (armies[i].layer == 11)
                        armies[i].GetComponentInChildren<Dwarf>().SubHealth(damage);
                }
            }

            for (int i = 0; i < heroes.Length; i++)
            {
                if (Vector2.Distance(heroes[i].transform.position, transform.position) <= RANGE)
                {
                    if (heroes[i].layer == 10)
                        heroes[i].GetComponentInChildren<EarthShaker>().SubHealth(damage);
                    else if (heroes[i].layer == 12)
                        heroes[i].GetComponentInChildren<NagaSiren>().SubHealth(damage);
                }
            }

            timeNextBurn = Time.time + TIME_RETURN_BURN;
        }

        if (Time.time - timeNextSound >= 0)
        {
            source.PlayOneShot(soundSkill, Random.Range(0.3f, 0.5f));
            timeNextSound = Time.time + lengthSound;
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
