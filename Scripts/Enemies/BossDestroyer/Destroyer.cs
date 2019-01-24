using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    public GameObject barBlood;
    public GameObject hellFire;
    public GameObject scorchedEarth;
    public GameObject posStartHellFire_1;
    public GameObject posStartHellFire_2;
    public AudioClip soundMoving;
    public AudioClip soundAttack;

    private const float HEALTH = 1000f;
    private const float RANGE_ATTACK = 1.6f;
    private const float ANGLE_SWAP_STATE = 60f;
    private const int EXP_RECEIVE_IF_OSK_DIE = 200;
    private const int GOLD_RECEIVE_IF_OSK_DIE = 200;

    private float speedMove;
    private float currentHealth;
    private float amor;
    private float damagePhysic;
    private float damageMagic;
    private float halfWidthAttacker;
    private float scaleX;
    private float distance;
    private bool isAttack;
    private bool isLockTarget;
    private int countPoint;

    private GameObject attacker;
    private Transform targetMove;
    private Animator anim;
    private AudioSource source;
    private SpriteRenderer sr;
    private PointEnemyFollow pointEnemyFollow;

    void Awake()
    {
        speedMove = 0.5f;
        amor = 15f;
        damagePhysic = 100f;
        damageMagic = 50f;

        halfWidthAttacker = RANGE_ATTACK / 2;
        currentHealth = HEALTH;
        scaleX = barBlood.transform.localScale.x;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        
        GameObject gameObjectPoint = GameObject.FindWithTag("PointsDemonFollow");
        pointEnemyFollow = gameObjectPoint.GetComponent<PointEnemyFollow>();
        targetMove = pointEnemyFollow.pointTransform[0];

        scorchedEarth.GetComponent<ScorchedEarth>().SetDamage(damageMagic / 4);
    }

    void Update()
    {
        Vector3 dir = targetMove.position - transform.position;

        if (!isAttack)
        {
            transform.Translate(dir.normalized * speedMove * Time.deltaTime, Space.World);
            StateAnimation(targetMove.position);
        }

        if (Vector3.Distance(transform.position, targetMove.position) <= 0.1f)
        {
            if (countPoint < pointEnemyFollow.pointTransform.Length - 1)
            {
                GetNextTarget();
            }
        }

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
                    sr.flipX = true;
                }

                isAttack = true;
            }
            else
                attacker = null;
        }
        else
        {
            isLockTarget = false;
            isAttack = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("End"))
            Destroy(gameObject);
    }

    private void GetNextTarget()
    {
        countPoint++;
        targetMove = pointEnemyFollow.pointTransform[countPoint];
    }

    public void SetGameObjectAttacker(GameObject gob)
    {
        if (!isLockTarget)
        {
            attacker = gob;
            isLockTarget = true;
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
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
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

    private void EventMoving()
    {
        source.PlayOneShot(soundMoving, Random.Range(0.3f, 0.7f));
    }

    private void EventAttack()
    {
        GameObject[] armies = GameObject.FindGameObjectsWithTag("Army");
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");

        for (int i = 0; i < armies.Length; i++)
        {
            if (Vector2.Distance(transform.position, armies[i].transform.position) <= 1.5f * halfWidthAttacker)
            {
                if (transform.position.x <= armies[i].transform.position.x)
                {
                    if (armies[i].layer == 11)
                        armies[i].GetComponentInChildren<Dwarf>().SubHealth(damagePhysic);
                    else if (armies[i].layer == 18)
                        armies[i].GetComponentInChildren<DwarfLV2>().SubHealth(damagePhysic);
                }
            }
        }

        for (int i = 0; i < heroes.Length; i++)
        {
            if (Vector2.Distance(transform.position, heroes[i].transform.position) <= 1.5f * halfWidthAttacker)
            {
                if (transform.position.x <= heroes[i].transform.position.x)
                {
                    if (heroes[i].layer == 10)
                        heroes[i].GetComponentInChildren<EarthShaker>().SubHealth(damagePhysic);
                    else if (heroes[i].layer == 12)
                        heroes[i].GetComponentInChildren<NagaSiren>().SubHealth(damagePhysic);
                }
            }
        }
    }

    private void EventLastFrameAttack()
    {
        isAttack = false;
    }

    private void EventStartFrameAttack()
    {
        source.PlayOneShot(soundAttack, Random.Range(0.6f, 1f));
    }

    private void EventSkillHellFire_1()
    {
        GameObject Fire = (GameObject)Instantiate(hellFire, 
                              posStartHellFire_1.transform.position, 
                              Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        
        Fire.GetComponentInChildren<HellFire>().SetDamage(damageMagic);
    }

    private void EventSkillHellFire_2()
    {
        GameObject Fire = (GameObject)Instantiate(hellFire, 
                              posStartHellFire_2.transform.position, 
                              Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        
        Fire.GetComponentInChildren<HellFire>().SetDamage(damageMagic);
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
