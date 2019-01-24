using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDemonChild : MonoBehaviour
{

    public GameObject barBlood;

    private const float HEALTH = 25f;
    private const float RANGE_ATTACK = 0.8f;
    private const float ANGLE_SWAP_STATE = 60f;
    private const int EXP_RECEIVE_IF_ICEDEMON_DIE = 5;
    private const int GOLD_RECEIVE_IF_ICEDEMON_DIE = 5;

    private float amor;
    private float currentHealth;
    private float damagePhysic;
    private float speedMove;
    private float halfWidthAttacker;
    private float scaleX;
    private int countPoint;
    private bool isLockTargetAttack;
    private bool isAttack;

    private PointEnemyFollow pointEnemyFollow;
    private Transform target;
    private Animator anim;
    private SpriteRenderer sr;
    private GameObject attacker;
    private GameObject UIGamePlay;

    void Awake()
    {
        currentHealth = HEALTH;
        amor = 5f;
        damagePhysic = 15f;
        speedMove = 0.6f;
        halfWidthAttacker = RANGE_ATTACK / 2;
        scaleX = barBlood.transform.localScale.x;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        UIGamePlay = GameObject.Find("UIGamePlay");

        GameObject point = GameObject.FindWithTag("PointsBossFollow");
        pointEnemyFollow = point.GetComponentInChildren<PointEnemyFollow>();

        target = pointEnemyFollow.pointTransform[countPoint];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        if (!isAttack)
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

        if (attacker != null)
        {
            Vector2 thisPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 attackerPosition = new Vector2(attacker.transform.position.x, 
                                           attacker.transform.position.y);

            float distance = Vector2.Distance(thisPosition, attackerPosition);

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
                    sr.flipX = true;
                }

                isAttack = true;
            }
            else if (distance > RANGE_ATTACK)
                attacker = null;
        }
        else
        {
            isLockTargetAttack = false;
            isAttack = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("End"))
            Destroy(gameObject);
    }

    public void SetPointIceDemonParentDie(int count)
    {
        this.countPoint = count;
    }

    public void SetGameObjectAttacker(GameObject attacker)
    {
        if (!isLockTargetAttack)
        {
            this.attacker = attacker;
            isLockTargetAttack = true;
        }
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

    private void UpdateBarBlood()
    {
        if (currentHealth < 0f)
        {
            currentHealth = 0f;
            Die();
        }
        if (currentHealth > HEALTH)
            currentHealth = HEALTH;

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleX * currentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    private void GetNextTarget()
    {
        countPoint++;
        target = pointEnemyFollow.pointTransform[countPoint];
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
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
    }

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x0 - x1) / Mathf.Abs(y0 - y1));
        return angle;
    }

    private void EventAttack()
    {
        if (attacker != null && attacker.layer == 10)
            attacker.GetComponentInChildren<EarthShaker>().SubHealth(damagePhysic);
        else if (attacker != null && attacker.layer == 11)
            attacker.GetComponentInChildren<Dwarf>().SubHealth(damagePhysic);
        else if (attacker != null && attacker.layer == 12)
            attacker.GetComponentInChildren<NagaSiren>().SubHealth(damagePhysic);
        else if (attacker != null && attacker.layer == 18)
            attacker.GetComponentInChildren<DwarfLV2>().SubHealth(damagePhysic);

        isAttack = false;
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
                    hero[i].GetComponentInChildren<EarthShaker>().AddExp(EXP_RECEIVE_IF_ICEDEMON_DIE);
                else if (hero[i].layer == 12)
                    hero[i].GetComponentInChildren<NagaSiren>().AddExp(EXP_RECEIVE_IF_ICEDEMON_DIE);
            }
        }

        UIGamePlay.GetComponent<UIGamePlay>().towerCurrency += GOLD_RECEIVE_IF_ICEDEMON_DIE;
        Destroy(gameObject);
    }

    private void AnimationMoveBehind()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", true);
    }

    private void AnimationMoveBefore()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveBefore", true);
    }

    private void AnimationMoveLR()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveLR", true);
    }

    private void AnimationAttack()
    {
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsAttack", true);
    }
}
