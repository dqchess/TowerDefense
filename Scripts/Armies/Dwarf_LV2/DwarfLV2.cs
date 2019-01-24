using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfLV2 : MonoBehaviour
{
    public GameObject barBlood;

    private const float TIME_ADD_HEALTH = 10f;
    private const float HEALTH_PLUS_PER_TIME = 20f;
    private const float ANGLE_SWAP_STATE = 60f;
    private const float RANGE = 1f;
    private const float HEALTH = 350f;

    private float timeStartAddHearth;
    private float curentHealth;
    private float speedMove;
    private float damagePhysic;
    private float amor;
    private float scaleX;
    private bool isAttack;
    private bool checkLockTarget;

    private Vector2 posStart;
    private Transform enemyTransform;
    private Animator anim;
    private SpriteRenderer sr;
    private GameObject enemyChoosedToAttack;

    void Awake()
    {
        speedMove = 0.7f;
        damagePhysic = 30f;
        amor = 20f;
        curentHealth = HEALTH;
        scaleX = barBlood.transform.localScale.x;
        timeStartAddHearth = Time.time;

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (!checkLockTarget)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].layer != 9 &&
                    Vector2.Distance(enemies[i].transform.position, transform.position) <= RANGE)
                {
                    enemyTransform = enemies[i].transform;
                    enemyChoosedToAttack = enemies[i];
                    checkLockTarget = true;
                }
            }
        }
        else
        {
            if (enemyTransform != null &&
                Vector2.Distance(enemyTransform.position, transform.position) > RANGE)
                enemyTransform = null;
        }

        if (enemyTransform == null)
        {
            checkLockTarget = false;
            isAttack = false;
        }
        else
        {
            SetGameObjectNeedAttack(enemyChoosedToAttack);
            Vector3 posAttack = enemyTransform.transform.position;

            if (Vector2.Distance(transform.position, enemyTransform.position) <= 0.8f)
            {
                AnimationAttack();
            }
            else
            {
                StateAnimation(posAttack);
                transform.position = Vector2.MoveTowards(transform.position, posAttack, 
                    speedMove * Time.deltaTime);
            } 

            isAttack = true;
        }

        if (!isAttack && new Vector2(transform.position.x, transform.position.y) == posStart)
        {
            AnimationIdle();
        }

        if (!checkLockTarget && !isAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, posStart, 
                speedMove * Time.deltaTime);
            StateAnimation(posStart);
        }

        if (Time.time - timeStartAddHearth >= TIME_ADD_HEALTH)
        {
            AddHealthPerTime();
            timeStartAddHearth = Time.time + TIME_ADD_HEALTH;
        }
    }

    public void SubHealth(float damage)
    {
        float damageReceive = damage - damage * amor / 100;
        curentHealth -= damageReceive;

        UpdateBarBlood();
    }

    public void AddHealth(float health)
    {
        curentHealth += health;

        UpdateBarBlood();
    }

    public void GetPositionStart(Vector2 pos)
    {
        this.posStart = pos;
    }

    private void UpdateBarBlood()
    {
        if (curentHealth < 0)
        {
            curentHealth = 0;
            Die();
        }

        if (curentHealth > HEALTH)
            curentHealth = HEALTH;

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleX * curentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    private void SetGameObjectNeedAttack(GameObject gob)
    {
        this.enemyChoosedToAttack = gob;

        if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 14)
            enemyChoosedToAttack.GetComponentInChildren<OskBane>().SetGameObjectAttacker(gameObject);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 8)
            enemyChoosedToAttack.GetComponentInChildren<Demon>().SetGameObjectAttacker(gameObject);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 15)
            enemyChoosedToAttack.GetComponentInChildren<IceDemon>().SetGameObjectAttacker(gameObject);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 16)
            enemyChoosedToAttack.GetComponentInChildren<IceDemonChild>().SetGameObjectAttacker(gameObject);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 17)
            enemyChoosedToAttack.GetComponentInChildren<Destroyer>().SetGameObjectAttacker(gameObject);
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

    private void AddHealthPerTime()
    {
        curentHealth += HEALTH_PLUS_PER_TIME;
        UpdateBarBlood();
    }

    private void EventAttack()
    {
        if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 8)
            enemyChoosedToAttack.GetComponentInChildren<Demon>().SubHealth(damagePhysic);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 14)
            enemyChoosedToAttack.GetComponentInChildren<OskBane>().SubHealth(damagePhysic);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 15)
            enemyChoosedToAttack.GetComponentInChildren<IceDemon>().SubHealth(damagePhysic);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 16)
            enemyChoosedToAttack.GetComponentInChildren<IceDemonChild>().SubHealth(damagePhysic);
        else if (enemyChoosedToAttack != null && enemyChoosedToAttack.layer == 17)
            enemyChoosedToAttack.GetComponentInChildren<Destroyer>().SubHealth(damagePhysic);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void AnimationIdle()
    {
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", true);
    }

    private void AnimationAttack()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsAttack", true);
    }

    private void AnimationMoveBehind()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBehind", true);
    }

    private void AnimationMoveBefore()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveBefore", true);
    }

    private void AnimationMoveLR()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveLR", true);
    }
}
