using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panda : MonoBehaviour {

    public GameObject barBlood;

    private float damageBasic;
    private float currentHealth;
    private float amor;
    private float scaleBarBlood_X;
    private int countPoint;
    private bool checkAttack;

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
    private GameObject gameObjectChoosedToAttack;
    private GameObject UIGamePlay;
    private EarthShaker es;
    private NagaSiren naga;
    private Dwarf dwarf;

    void Awake()
    {
        currentHealth = HEALTH;
        damageBasic = 5f;
        amor = 5f;
        scaleBarBlood_X = barBlood.transform.localScale.x;
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject gameObjectPoint = GameObject.Find("PointsEnemyFollow");
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

        if (!checkAttack)
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
        else if (coll.gameObject.tag == "Hero" && !checkAttack)
        {
            AnimationAttack();
            gameObjectChoosedToAttack = coll.gameObject;
        }
        else if (coll.gameObject.tag == "Dwarf" && !checkAttack)
        {
            AnimationAttack();
            gameObjectChoosedToAttack = coll.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Hero")
            checkAttack = false;
        if (coll.gameObject.tag == "Dwarf")
            checkAttack = false;
    }

    private void StateAnimationDragon(Transform tranf)
    {
        float x0 = previousTransform.position.x;
        float y0 = previousTransform.position.y;
        float x1 = tranf.transform.position.x;
        float y1 = tranf.transform.position.y;

        float angle = GetRotateAngleGameObject(x0, y0, x1, y1);

        if (angle < ANGLE_SWAP_STATE && !checkAttack)
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
        else if (angle >= ANGLE_SWAP_STATE && !checkAttack)
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
        anim.SetBool("isMoveLR", false);
        anim.SetBool("isMoveBefore", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isMoveBehind", true);
    }

    private void AnimationMoveBefore()
    {
        anim.SetBool("isMoveLR", false);
        anim.SetBool("isMoveBehind", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isMoveBefore", true);
    }

    private void AnimationMoveLR()
    {
        anim.SetBool("isMoveBehind", false);
        anim.SetBool("isMoveBefore", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isMoveLR", true);
    }

    private void AnimationAttack()
    {
        checkAttack = true;
        anim.SetBool("isMoveBefore", false);
        anim.SetBool("isMoveBehind", false);
        anim.SetBool("isMoveLR", false);
        anim.SetBool("isAttack", true);
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

    private void Attack()
    {
        if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 10)
        {
            es = gameObjectChoosedToAttack.GetComponentInChildren<EarthShaker>();
            es.SubHealth(damageBasic);
        }
        else if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 11)
        {
            dwarf = gameObjectChoosedToAttack.GetComponentInChildren<Dwarf>();
            dwarf.SubHealth(damageBasic);
        }
        else if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 12)
        {
            naga = gameObjectChoosedToAttack.GetComponentInChildren<NagaSiren>();
            naga.SubHealth(damageBasic);
        }
    }

    private void Die()
    {
        es.AddExp(EXP_RECEIVE_IF_DRAGON_DIE);
        UIGamePlay.GetComponent<UIGamePlay>().towerCurrency += GOLD_RECEIVE_IF_DRAGON_DIE;
        Destroy(gameObject);
    }
}
