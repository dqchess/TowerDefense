using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShaker : MonoBehaviour
{

    public GameObject earthSkill;
    public GameObject auraSkill;
    public float damageEarthSkill;
    public float damageBasic;
    public float amor;
    public float speedmove;
    public int levelHero;

    private Animator anim;
    private GameObject barBlood;
    private ExpPerLevel epl;

    private const float HEALTH = 300;
    private const float TIME_ADD_HEALTH = 10f;
    private const float TIME_COUNTDOWN_AURA_SKILL = 12f;

    private int[] exp;
    private int currentExp;
    private int countNumberAura;
    private float amorNotAura;
    private float amorPlusWhenHaveAura;
    private float timeStartAddHearth;
    private float timeStartAuraSkill;
    private float currentHealth;
    private float scaleBarBlood_X;

    void Awake()
    {
        currentHealth = HEALTH;
        damageEarthSkill = 50f;
        damageBasic = 10f;
        speedmove = 1f;
        currentExp = 0;
        levelHero = 1;
        amor = 5f;
        amorPlusWhenHaveAura = 10f;
        timeStartAddHearth = Time.time;
        timeStartAuraSkill = Time.time - TIME_COUNTDOWN_AURA_SKILL;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        barBlood = GameObject.Find("healthBarHero_1");
        scaleBarBlood_X = barBlood.transform.localScale.x;
        epl = new ExpPerLevel();

        exp = new int[]
        { epl.EXP_LV_2, epl.EXP_LV_3, epl.EXP_LV_4, epl.EXP_LV_5, epl.EXP_LV_6, 
            epl.EXP_LV_7, epl.EXP_LV_8, epl.EXP_LV_9, epl.EXP_LV_10
        };
    }

    void Update()
    {
        checkLevelHero();

        if (Time.time - timeStartAddHearth >= TIME_ADD_HEALTH)
        {
            AddHealthPerTime();
            timeStartAddHearth = Time.time + TIME_ADD_HEALTH;
        }

        if (Time.time - timeStartAuraSkill >= TIME_COUNTDOWN_AURA_SKILL)
        {
            auraSkill.SetActive(true);
            amorNotAura = amor;
            amor += amorPlusWhenHaveAura;
            timeStartAuraSkill = Time.time + TIME_COUNTDOWN_AURA_SKILL;
        }
    }

    public void AnimationAttack()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", false); 
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsAttack", true);
    }

    public void AnimationIdle()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", false); 
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", true);
    }

    public void AnimationMoveLR()
    {
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", false); 
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveLR", true);
    }

    public void AnimationMoveBehind()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBehind", false); 
        anim.SetBool("IsMoveBefore", true);
    }

    public void AnimationMoveBefore()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", true);
    }

    private void SkillAttack()
    {
        Instantiate(earthSkill, transform.position, 
            Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    private void checkLevelHero()
    {
        for (int i = 0; i < exp.Length; i++)
        {
            if (currentExp > exp[i] && levelHero - 2 < i)
            {
                levelHero = i + 2;
                Debug.Log("Level Hero: " + levelHero);
                AddStrengthHero();
                break;
            }
        }      
    }

    private void AddStrengthHero()
    {
        amor += levelHero * 3;
        amorPlusWhenHaveAura += levelHero * 5;
        damageBasic += levelHero * 2;
        damageEarthSkill += levelHero * 3;
    }

    private void UpdateBarBlood()
    {

        if (currentHealth <= 0)
            currentHealth = 0;

        if (currentHealth > HEALTH)
            currentHealth = HEALTH;

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleBarBlood_X * currentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    public void SubHealth(float damageReceive)
    {
        float damage = (damageReceive - damageReceive * amor / 100);
        if (damage <= 0)
            damage = 1;
        currentHealth -= damage;
        
        UpdateBarBlood();
    }

    public void AddExp(int expReceive)
    {
        currentExp += expReceive;
    }

    private void AddHealthPerTime()
    {
        currentHealth += levelHero * 5f;

        UpdateBarBlood();
    }

    private void EventEndAuraSkill()
    {
        countNumberAura++;
        if (countNumberAura > 3)
        {
            countNumberAura = 0;
            amor = amorNotAura;
            auraSkill.SetActive(false);
        }
    }
}
