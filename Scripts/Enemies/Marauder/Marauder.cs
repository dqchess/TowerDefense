using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marauder : MonoBehaviour {
    
    public GameObject barBlood;

    private const float HEALTH = 300f;
    private const float ANGLE_SWAP_STATE = 60f;
    private const float RANGE_ATTACK = 1.2f;
    private const int MARAUDER_EXP = 10;
    private const int MARAUDER_GOLD = 20;

    private float speedMove;
    private float currentHealth;
    private float amor;
    private float damageBasic;
    private float damageSkill;
    private float scaleBarBlood_X;
    private int countPoint;
    private bool checkAttack;
    private bool isLockTarget;

    private Animator anim;
    private Transform target;
    private Transform previousTransform;
    private PointEnemyFollow pointMarauder;
    private GameObject gameObjectChoosedToAttack;
    private GameObject heroEarthShaker;
    private GameObject UIGamePlay;
    private EarthShaker es;
    private NagaSiren naga;
    private Dwarf dwarf;

    void Awake()
    {
        speedMove = 1f;
        currentHealth = HEALTH;
        amor = 30f;
        damageBasic = 10f;
        damageSkill = 50f;
        countPoint = 0;
        scaleBarBlood_X = barBlood.transform.localScale.x;
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject gameObjectPoint = GameObject.FindWithTag("PointsDemonFollow");
        pointMarauder = gameObjectPoint.GetComponentInChildren<PointEnemyFollow>();
        target = pointMarauder.pointTransform[0];

        heroEarthShaker = GameObject.Find("ES");
        es = heroEarthShaker.GetComponent<EarthShaker>();
        UIGamePlay = GameObject.Find("UIGamePlay");
    }

    void Update()
    {
        previousTransform = gameObject.transform;
        Vector3 dir = target.position - transform.position;

        if (!checkAttack)
            transform.Translate(dir.normalized * speedMove * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            if (countPoint < pointMarauder.pointTransform.Length - 1)
            {
                GetNextTarget();
            }
        }

        if (gameObjectChoosedToAttack != null)
        {
            Vector2 thisPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 attackerPosition = new Vector2(gameObjectChoosedToAttack.transform.position.x,
                                           gameObjectChoosedToAttack.transform.position.y);
            float distance = Vector2.Distance(thisPosition, attackerPosition);
            if (distance <= RANGE_ATTACK)
            {
                AnimationAttack();
                checkAttack = true;
            }
            else
                gameObjectChoosedToAttack = null;
        }
        else
            isLockTarget = false;

        StateAnimationDragon(target);
    }

    private void GetNextTarget()
    {
        countPoint++;
        target = pointMarauder.pointTransform[countPoint];
    }

    private void AnimationMove(int moveState)
    {
        anim.SetInteger("MoveState", moveState);
    }

    private void AnimationAttack()
    {
        anim.SetInteger("MoveState", -1);
        anim.SetTrigger("Attack");
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
                AnimationMove(2);
            }
            else
            {
                AnimationMove(1);
            }
        }
        else if (angle >= ANGLE_SWAP_STATE && !checkAttack)
        {
            AnimationMove(0);

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

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x0 - x1) / Mathf.Abs(y0 - y1));
        return angle;
    }

    public void AddDamage(float damagePlus)
    {
        damageBasic += damagePlus;
        damageSkill += damagePlus;
    }

    public void SubHealth(float damage)
    {
        float damageReceive = damage - damage * amor / 100;
        currentHealth -= damageReceive;

        UpdateBarBlood();
    }

    public void SetGameObjectAttacker(GameObject gob)
    {
        if (!isLockTarget)
        {
            this.gameObjectChoosedToAttack = gob;
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

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleBarBlood_X * currentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    private void Die()
    {
        //es.AddExp(EXP_RECEIVE_IF_DEMON_DIE);
        //UIGamePlay.GetComponent<UIGamePlay>().towerCurrency += GOLD_RECEIVE_IF_DEMON_DIE;
        Destroy(gameObject);
    }

    private void Attack()
    {
        if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 10)
        {
            es = gameObjectChoosedToAttack.GetComponentInChildren<EarthShaker>();
            es.SubHealth(damageBasic);
        }
        if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 11)
        {
            dwarf = gameObjectChoosedToAttack.GetComponentInChildren<Dwarf>();
            dwarf.SubHealth(damageBasic);
        }
        if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 12)
        {
            naga = gameObjectChoosedToAttack.GetComponentInChildren<NagaSiren>();
            naga.SubHealth(damageBasic);
        }

        checkAttack = false;
    }
}
