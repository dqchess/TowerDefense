/* This is enemy lv1. it can't attack.
   This is enemy can fly --> Armies and Heroes of type mele can't attack it. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{

    public GameObject barBlood;

    private float currentHealth;
    private float amor;
    private float scaleBarBlood_X;
    private int countPoint;

    private const float HEALTH = 100f;
    private const float SPEED = 0.8f;
    private const float ANGLE_SWAP_STATE = 60f;
    private const int EXP_RECEIVE_IF_DRAGON_DIE = 5;
    private const int GOLD_RECEIVE_IF_DRAGON_DIE = 10;

    private Transform target;
    private Transform previousTransform;
    private PointEnemyFollow pointEnemyFollow;
    private Animator anim;
    private GameObject heroEarthShaker;
    private GameObject UIGamePlay;
    private EarthShaker es;

    void Awake()
    {
        currentHealth = HEALTH;
        amor = 5f;
        scaleBarBlood_X = barBlood.transform.localScale.x;
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject gameObjectPoint = GameObject.FindWithTag("PointsDragonFollow");
        pointEnemyFollow = gameObjectPoint.GetComponent<PointEnemyFollow>();
        target = pointEnemyFollow.pointTransform[0];

        heroEarthShaker = GameObject.Find("ES");
        es = heroEarthShaker.GetComponent<EarthShaker>();
        UIGamePlay = GameObject.Find("UIGamePlay");
    }

    void Update()
    {
        previousTransform = gameObject.transform;
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * SPEED * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            if (countPoint < pointEnemyFollow.pointTransform.Length - 1)
            {
                GetNextTarget();
            }
        }

        StateAnimationDragon(target);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "End")
        {
            Destroy(gameObject);
        }
    }

    private void StateAnimationDragon(Transform tranf)
    {
        float x0 = previousTransform.position.x;
        float y0 = previousTransform.position.y;
        float x1 = tranf.transform.position.x;
        float y1 = tranf.transform.position.y;

        float angle = GetRotateAngleGameObject(x0, y0, x1, y1);

        if (angle < ANGLE_SWAP_STATE)
        {
            if (y0 < y1)
            {
                AnimationMoveBefore();
            }
            else
            {
                AnimationMoveBehind();
            }
        }
        else if (angle >= ANGLE_SWAP_STATE)
        {
            AnimationMoveLR();

            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            if (transform.position.x < tranf.transform.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
    }

    private void AnimationMoveBehind()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveBehind", true);
    }

    private void AnimationMoveBefore()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveBefore", true);
    }

    private void AnimationMoveLR()
    {
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsMoveLR", true);
    }

    private void AnimationAttack()
    {
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsAttack", true);
    }

    public void SubHealth(float damage)
    {
        float damageRecive = damage - damage * amor / 100;
        if (damageRecive <= 0)
            damageRecive = 1;

        currentHealth -= damageRecive;
        UpdateBarBlood();
    }


    private void UpdateBarBlood()
    {
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleBarBlood_X * currentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    private void GetNextTarget()
    {
        countPoint++;
        target = pointEnemyFollow.pointTransform[countPoint];
    }

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x0 - x1) / Mathf.Abs(y0 - y1));
        return angle;
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
                    hero[i].GetComponentInChildren<EarthShaker>().AddExp(EXP_RECEIVE_IF_DRAGON_DIE);
                else if (hero[i].layer == 12)
                    hero[i].GetComponentInChildren<NagaSiren>().AddExp(EXP_RECEIVE_IF_DRAGON_DIE);
            }
        }

        UIGamePlay.GetComponent<UIGamePlay>().towerCurrency += GOLD_RECEIVE_IF_DRAGON_DIE;
        Destroy(gameObject);
    }
}
