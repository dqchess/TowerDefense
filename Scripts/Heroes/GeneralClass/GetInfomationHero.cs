using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetInfomationHero : MonoBehaviour
{

    public Text speedHero;
    public Text amorHero;
    public Text damagePhysic;
    public Text damageMagic;
    public Text levelHero;
    public GameObject avatarEarthShaker;
    public GameObject avatarNagaSiren;

    private const string HERO_TAG = "Hero";
    private GameObject hero;

    void Update()
    {
        hero = GameObject.FindWithTag(HERO_TAG);

        if (hero != null && hero.layer == 10)
        {
            speedHero.text = "" + hero.GetComponentInChildren<EarthShaker>().speedmove;
            amorHero.text = "" + hero.GetComponentInChildren<EarthShaker>().amor;
            damagePhysic.text = "" + hero.GetComponentInChildren<EarthShaker>().damageBasic;
            damageMagic.text = "" + hero.GetComponentInChildren<EarthShaker>().damageEarthSkill;
            levelHero.text = "" + hero.GetComponentInChildren<EarthShaker>().levelHero;

            avatarEarthShaker.SetActive(true);
        }
        else if (hero.layer == 12 && hero != null)
        {
            speedHero.text = "" + hero.GetComponentInChildren<NagaSiren>().speedMove;
            amorHero.text = "" + hero.GetComponentInChildren<NagaSiren>().amor;
            damagePhysic.text = "" + hero.GetComponentInChildren<NagaSiren>().damagePhysic;
            damageMagic.text = "" + hero.GetComponentInChildren<NagaSiren>().damageMagic;
            levelHero.text = "" + hero.GetComponentInChildren<NagaSiren>().levelHero;

            avatarNagaSiren.SetActive(true);
        }
    }
}
