/* Enemy Lv2. if being attacked and the attaker inside it's range, it will attack the attacker. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{

    public GameObject barBlood;

    private const float HEALTH = 300f;
    private const float ANGLE_SWAP_STATE = 60f;
    private const float RANGE_ATTACK = 1.2f;
    private const float TIME_SWAP_ENEMY = 8f;
    private const int EXP_RECEIVE_IF_DEMON_DIE = 10;
    private const int GOLD_RECEIVE_IF_DEMON_DIE = 20;

    private float speedMove;
    private float currentHealth;
    private float amor;
    private float damageBasic;
    private float damageSkill;
    private float scaleBarBlood_X;
    private float timeStartSwap;
    private float halfWidthAttacker;
    private int countPoint;
    private bool checkAttack;
    private bool isLockTarget;

    private Animator anim;
    private Transform target;
    private SpriteRenderer sr;
    private PointEnemyFollow pointDemonFollow;
    private GameObject gameObjectChoosedToAttack;
    private GameObject UIGamePlay;
    private EarthShaker es;
    private NagaSiren naga;
    private Dwarf dwarf;

    void Awake()
    {
        speedMove = 0.8f;
        currentHealth = HEALTH;
        amor = 30f;
        damageBasic = 10f;
        damageSkill = 50f;
        countPoint = 0;
        halfWidthAttacker = RANGE_ATTACK / 2;
        scaleBarBlood_X = barBlood.transform.localScale.x;
        timeStartSwap = Time.time;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject gameObjectPoint = GameObject.FindWithTag("PointsDemonFollow");
        pointDemonFollow = gameObjectPoint.GetComponentInChildren<PointEnemyFollow>();
        target = pointDemonFollow.pointTransform[0];

        UIGamePlay = GameObject.Find("UIGamePlay");
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;

        if (!checkAttack)
        {
            transform.Translate(dir.normalized * speedMove * Time.deltaTime, Space.World);
            StateAnimation(target.position);
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            if (countPoint < pointDemonFollow.pointTransform.Length - 1)
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
                Vector3 tranAttack = new Vector3(gameObjectChoosedToAttack.transform.position.x
                                         - halfWidthAttacker, gameObjectChoosedToAttack.transform.position.y, 0f);
                
                StateAnimation(tranAttack);
                
                transform.position = Vector3.MoveTowards(transform.position, 
                    tranAttack, speedMove * Time.deltaTime);

                if (transform.position == tranAttack)
                {
                    AnimationAttack();
                    sr.flipX = true;
                }
                
                checkAttack = true;
            }
            else if (distance > RANGE_ATTACK)
                gameObjectChoosedToAttack = null;
        }
        else
        {
            isLockTarget = false;
            checkAttack = false;
        }
    }

    private void GetNextTarget()
    {
        countPoint++;
        target = pointDemonFollow.pointTransform[countPoint];
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

    private void StateAnimation(Vector2 pos)
    {
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        float x1 = pos.x;
        float y1 = pos.y;

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
        GameObject[] hero = GameObject.FindGameObjectsWithTag("Hero");

        for (int i = 0; i < hero.Length; i++)
        {
            float distanceWithHero = Vector3.Distance(transform.position, hero[i].transform.position);
            if (distanceWithHero <= 5f)
            {
                if (hero[i].layer == 10)
                    hero[i].GetComponentInChildren<EarthShaker>().AddExp(EXP_RECEIVE_IF_DEMON_DIE);
                else if (hero[i].layer == 12)
                    hero[i].GetComponentInChildren<NagaSiren>().AddExp(EXP_RECEIVE_IF_DEMON_DIE);
            }
        }

        UIGamePlay.GetComponent<UIGamePlay>().towerCurrency += GOLD_RECEIVE_IF_DEMON_DIE;
        Destroy(gameObject);
    }

    private void Attack()
    {
        if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 10)
            gameObjectChoosedToAttack.GetComponentInChildren<EarthShaker>().SubHealth(damageBasic);
        else if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 11)
            gameObjectChoosedToAttack.GetComponentInChildren<Dwarf>().SubHealth(damageBasic);
        else if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 12)
            gameObjectChoosedToAttack.GetComponentInChildren<NagaSiren>().SubHealth(damageBasic);
        else if (gameObjectChoosedToAttack != null && gameObjectChoosedToAttack.layer == 18)
            gameObjectChoosedToAttack.GetComponentInChildren<DwarfLV2>().SubHealth(damageBasic);

        checkAttack = false;
    }
}
