using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OskBane : MonoBehaviour
{

    public GameObject barBlood;
    public GameObject skillSwordOfDeath;
    public GameObject positionStartSkill;
    public AudioClip soundSkill;
    public AudioClip soundAttack;

    private const float HEALTH = 1200f;
    private const float TIME_IDLE = 5f;
    private const float RANGE_ATTACK = 1.5f;
    private const float ANGLE_SWAP_STATE = 60f;
    private const int EXP_RECEIVE_IF_OSK_DIE = 50;
    private const int GOLD_RECEIVE_IF_OSK_DIE = 50;
    private float[] ANGLE = { 10f, 5f, 0f, -5f, -10f };

    private float timeStartIdle;
    private float currentHealth;
    private float amor;
    private float speedMove;
    private float scaleX;
    private float damagePhysic;
    private float damageMagic;
    private float distance;
    private float halfWidthAttacker;
    private int countPoint;
    private bool isAttack;
    private bool isIdle;
    private bool isLockTarget;

    private PointEnemyFollow pointEnemyFollow;
    private GameObject tree;
    private GameObject UIGamePlay;
    private GameObject attacker;
    private Transform target;
    private Animator anim;
    private SpriteRenderer sr;
    private AudioSource source;

    void Awake()
    {
        currentHealth = HEALTH;
        amor = 50f;
        speedMove = 0.5f;
        scaleX = barBlood.transform.localScale.x;
        damagePhysic = 30f;
        damageMagic = 50f;
        halfWidthAttacker = RANGE_ATTACK / 2;

        sr = gameObject.GetComponent<SpriteRenderer>();
        source = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        UIGamePlay = GameObject.Find("UIGamePlay");

        GameObject gameObjectPoint = GameObject.FindWithTag("PointsBossFollow");
        pointEnemyFollow = gameObjectPoint.GetComponent<PointEnemyFollow>();
        target = pointEnemyFollow.pointTransform[0];

        timeStartIdle = Time.time;
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        if (!isAttack && !isIdle)
        {
            transform.Translate(dir.normalized * speedMove * Time.deltaTime, Space.World);
            StateAnimation(target.position);
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            if (countPoint < pointEnemyFollow.pointTransform.Length - 1)
            {
                GetNextTarget();
            }
        }

        if (Time.time - timeStartIdle < 0 && isIdle)
            AnimationIdle();
        else if (isAttack && Time.time - timeStartIdle >= 0)
            AnimationAttack();

        if (attacker != null)
        {
            distance = Vector3.Distance(transform.position, attacker.transform.position);
            if (distance <= RANGE_ATTACK)
            {
                Vector3 tranAttack = new Vector3(attacker.transform.position.x
                                         - halfWidthAttacker, attacker.transform.position.y, 0f);

                StateAnimation(tranAttack);

                transform.position = Vector3.MoveTowards(transform.position, 
                    tranAttack, speedMove * Time.deltaTime);

                if (transform.position == tranAttack)
                {
                    AnimationAttack();
                    sr.flipX = false;
                }
                
                isAttack = true;
            }
            else
                attacker = null;
        }
        else
        {
            isLockTarget = false;
        }
    }

    private void GetNextTarget()
    {
        countPoint++;
        target = pointEnemyFollow.pointTransform[countPoint];
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Tree" && !isAttack)
        {
            tree = coll.gameObject;
            timeStartIdle = Time.time + TIME_IDLE;
            isAttack = true;
            isIdle = true;
        }
        else if (coll.gameObject.tag == "End")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        isIdle = false;
    }

    public void SubHealth(float damage)
    {
        float damageReceive = damage - damage * amor / 100;
        currentHealth -= damageReceive;
        UpdateBarBlood();
    }

    public void AddHealth(float health)
    {
        currentHealth += health;
        UpdateBarBlood();
    }

    public void SetGameObjectAttacker(GameObject gob)
    {
        if (!isLockTarget)
        {
            attacker = gob;
            isLockTarget = true;
        }
    }

    private void UpdateBarBlood()
    {
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }

        if (currentHealth >= HEALTH)
            currentHealth = HEALTH;

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleX * currentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    private void StateAnimation(Vector2 pos)
    {
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        float x1 = pos.x;
        float y1 = pos.y;

        float angle = GetRotateAngleGameObject(x0, y0, x1, y1);

        if (angle < ANGLE_SWAP_STATE)
        {
            if (y0 < y1)
            {
                AnimationMoveBehind();
            }
            else
            {
                AnimationMoveBefore();
            }
        }
        else if (angle >= ANGLE_SWAP_STATE)
        {
            AnimationMoveLR();

            if (transform.position.x < pos.x)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
    }

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x0 - x1) / Mathf.Abs(y0 - y1));
        return angle;
    }

    private void EventDestroyTree()
    {
        if (tree != null)
            Destroy(tree);

        if (attacker != null)
            Attack();
    }

    private void EventEndAttack()
    {
        isAttack = false;
    }

    private void Attack()
    {
        source.PlayOneShot(soundAttack, Random.Range(0.3f, 0.6f));

        if (attacker.layer == 12)
            attacker.GetComponentInChildren<NagaSiren>().SubHealth(damagePhysic);
        else if (attacker.layer == 10)
            attacker.GetComponentInChildren<EarthShaker>().SubHealth(damagePhysic);
        else if (attacker.layer == 11)
            attacker.GetComponentInChildren<Dwarf>().SubHealth(damagePhysic);
        else if (attacker.layer == 18)
            attacker.GetComponentInChildren<DwarfLV2>().SubHealth(damagePhysic);

        int random = Random.Range(1, 3);
        if (random == 2)
        {
            source.PlayOneShot(soundSkill, Random.Range(0.3f, 0.6f));
            SkillSwordOfDeath();
        }
    }

    private void SkillSwordOfDeath()
    {
        GameObject skill_1 = Instantiate(skillSwordOfDeath, positionStartSkill.transform.position, 
                                 Quaternion.Euler(new Vector3(0f, 0f, ANGLE[0])));
        GameObject skill_2 = Instantiate(skillSwordOfDeath, positionStartSkill.transform.position, 
                                 Quaternion.Euler(new Vector3(0f, 0f, ANGLE[1])));
        GameObject skill_3 = Instantiate(skillSwordOfDeath, positionStartSkill.transform.position, 
                                 Quaternion.Euler(new Vector3(0f, 0f, ANGLE[2])));
        GameObject skill_4 = Instantiate(skillSwordOfDeath, positionStartSkill.transform.position, 
                                 Quaternion.Euler(new Vector3(0f, 0f, ANGLE[3])));
        GameObject skill_5 = Instantiate(skillSwordOfDeath, positionStartSkill.transform.position, 
                                 Quaternion.Euler(new Vector3(0f, 0f, ANGLE[4])));

        skill_1.GetComponentInChildren<SwordOfDeathSkill>().SetDamage(damageMagic);
        skill_2.GetComponentInChildren<SwordOfDeathSkill>().SetDamage(damageMagic);
        skill_3.GetComponentInChildren<SwordOfDeathSkill>().SetDamage(damageMagic);
        skill_4.GetComponentInChildren<SwordOfDeathSkill>().SetDamage(damageMagic);
        skill_5.GetComponentInChildren<SwordOfDeathSkill>().SetDamage(damageMagic);

        skill_1.GetComponentInChildren<SwordOfDeathSkill>().SetAngleStart(ANGLE[0]);
        skill_2.GetComponentInChildren<SwordOfDeathSkill>().SetAngleStart(ANGLE[1]);
        skill_3.GetComponentInChildren<SwordOfDeathSkill>().SetAngleStart(ANGLE[2]);
        skill_4.GetComponentInChildren<SwordOfDeathSkill>().SetAngleStart(ANGLE[3]);
        skill_5.GetComponentInChildren<SwordOfDeathSkill>().SetAngleStart(ANGLE[4]);
    }

    private void Die()
    {
        GameObject[] hero = GameObject.FindGameObjectsWithTag("Hero");

        for (int i = 0; i < hero.Length; i++)
        {
            float distanceWithHero = Vector3.Distance(transform.position, hero[i].transform.position);
            if (distanceWithHero <= 5f)
            {
                if (hero[i].layer == 10)
                    hero[i].GetComponentInChildren<EarthShaker>().AddExp(EXP_RECEIVE_IF_OSK_DIE);
                else if (hero[i].layer == 12)
                    hero[i].GetComponentInChildren<NagaSiren>().AddExp(EXP_RECEIVE_IF_OSK_DIE);
            }
        }

        UIGamePlay.GetComponent<UIGamePlay>().towerCurrency += GOLD_RECEIVE_IF_OSK_DIE;
        Destroy(gameObject);
    }

    private void AnimationAttack()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsAttack", true);
    }

    private void AnimationMoveLR()
    {
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveLR", true);
    }

    private void AnimationMoveBefore()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveBefore", true);
    }

    private void AnimationIdle()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsIdle", true);
    }

    private void AnimationMoveBehind()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBehind", true);
    }
}
