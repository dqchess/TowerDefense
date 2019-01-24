using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NagaSiren : MonoBehaviour
{

    public GameObject arow;
    public GameObject barBlood;
    public GameObject positionFire;
    public GameObject effectLocationTouch;
    public GameObject shadowLeft;
    public GameObject shadowBeHind;
    public GameObject shadowBeFore;
    public GameObject shadowRight;
    public GameObject nagaInllusion;
    public AudioClip soundShoot;

    public float speedMove;
    public float amor;
    public float damagePhysic;
    public float damageMagic;
    public int levelHero;

    private Animator anim;
    private AudioSource source;
    private Vector2 touchPosition;
    private GameObject enemyChoosedToAttack;
    private SpriteRenderer sr;
    private ExpPerLevel epl;
    private ChooseHero chooseHero;

    private const float HEALTH = 150f;
    private const float TIME_ADD_HEALTH = 10f;
    private const float RANGE = 3f;
    private const float TIME_DELAY_FIRE = 1f;
    private const float TIME_EXIST_INLLUSION = 10f;
    private const float TIME_COUNTDOWN_SKILL_INLLUSION = 25f;
    private const float ANGLE_SWAP_STATE = 60f;
   
    private float timeEndFire;
    private float timeStartInllusion;
    private float timeStartAddHearth;
    private float currentHealth;
    private float distanceWithEnemy;
    private float scaleX;

    private int[] exp;
    private int currentExp;
    private bool isIdle;
    private bool isAttack;

    void Awake()
    {
        currentHealth = HEALTH;
        speedMove = 1.5f;
        amor = 5f;
        damagePhysic = 60f;
        damageMagic = 80f;
        levelHero = 1;
        timeEndFire = timeStartAddHearth = timeStartInllusion = Time.time;
        scaleX = barBlood.transform.localScale.x;

        touchPosition = new Vector2(transform.position.x, transform.position.y);
        sr = GetComponent<SpriteRenderer>();
        chooseHero = gameObject.GetComponentInChildren<ChooseHero>();

        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        epl = new ExpPerLevel();

        positionFire.transform.position = new Vector3(positionFire.transform.position.x, 
            positionFire.transform.position.y, 0);

        exp = new int[]
        { epl.EXP_LV_2, epl.EXP_LV_3, epl.EXP_LV_4, epl.EXP_LV_5, epl.EXP_LV_6, 
            epl.EXP_LV_7, epl.EXP_LV_8, epl.EXP_LV_9, epl.EXP_LV_10
        };
    }

    void Update()
    {
        GetEnemyToAttack();
        checkLevelHero();

        if (gameObject == GameObject.FindWithTag("NagaInllusion") &&
            Time.time - timeStartInllusion > TIME_EXIST_INLLUSION)
        {
            Destroy(gameObject);
        }

        if (gameObject != GameObject.FindWithTag("NagaInllusion") &&
            Time.time - timeStartInllusion > 0f)
        {
            Instantiate(nagaInllusion, positionFire.transform.position, 
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            timeStartInllusion = Time.time + TIME_COUNTDOWN_SKILL_INLLUSION;
        }

        if (enemyChoosedToAttack != null)
        {
            float distance = Vector2.Distance(gameObject.transform.position, 
                                 enemyChoosedToAttack.transform.position);

            if (distance > RANGE)
                enemyChoosedToAttack = null;
        }

        if (enemyChoosedToAttack == null && isIdle)
        {
            isAttack = false;
            AnimationIdle();
            isIdle = false;
        }

        if (new Vector2(transform.position.x, transform.position.y) == touchPosition && !isAttack)
        {
            isIdle = true;
        }

        if (Input.GetMouseButtonDown(0) && chooseHero != null && chooseHero.isChoosedGameObject)
        {
            Ray clickedPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            touchPosition = new Vector2(clickedPosition.origin.x, clickedPosition.origin.y);
            chooseHero.SetIsChoosed(false);

            Instantiate(effectLocationTouch, touchPosition, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        }
            
        if (!isAttack && !isIdle)
        {
            transform.position = Vector2.MoveTowards(transform.position, touchPosition, 
                speedMove * Time.deltaTime);
            AnimationsHeroMove(touchPosition);
        }

        if (new Vector2(transform.position.x, transform.position.y) != touchPosition)
        {
            isIdle = false;
            isAttack = false;
        }

        if (isAttack && enemyChoosedToAttack != null)
        {
            if (transform.position.x < enemyChoosedToAttack.transform.position.x)
                RotateLeftHero();
            else
                RotateRightHero();
        }

        if (Time.time - timeStartAddHearth >= TIME_ADD_HEALTH)
        {
            AddHealthPerTime();
            timeStartAddHearth = Time.time + TIME_ADD_HEALTH;
        }
    }

    public void AddExp(int exp)
    {
        currentExp += exp;
        Debug.Log("Naga: " + currentExp);
    }

    public void  SubHealth(float damage)
    {
        float damageReceive = damage - damage * amor / 100;
        currentHealth -= damageReceive;

        UpdateBarBlood();
    }

    private void UpdateBarBlood()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
        if (currentHealth >= HEALTH)
            currentHealth = HEALTH;

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleX * currentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    private void RotateRightHero()
    {
        Vector2 scale = transform.localScale;
        if (scale.x < 0)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }

        shadowBeHind.SetActive(false);
        shadowLeft.SetActive(false);
        shadowBeFore.SetActive(false);
        shadowRight.SetActive(true);
    }

    private void RotateLeftHero()
    {
        Vector2 scale = transform.localScale;
        if (scale.x > 0)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }

        shadowBeHind.SetActive(false);
        shadowLeft.SetActive(true);
        shadowBeFore.SetActive(false);
        shadowRight.SetActive(false);
    }

    private void AddStrengthHero()
    {
        amor += levelHero * 2;
        damagePhysic += levelHero * 4;
        damageMagic += levelHero * 2;
    }

    private void AddHealthPerTime()
    {
        currentHealth += levelHero * 2f;
        UpdateBarBlood();
    }

    private void checkLevelHero()
    {
        for (int i = 0; i < exp.Length; i++)
        {
            if (currentExp > exp[i] && levelHero - 2 < i)
            {
                levelHero = i + 2;
                AddStrengthHero();
                break;
            }
        }      
    }

    private void GetEnemyToAttack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (!isAttack)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                distanceWithEnemy = Vector2.Distance(gameObject.transform.position, 
                    enemies[i].transform.position);

                if (distanceWithEnemy <= RANGE && isIdle)
                {
                    isAttack = true;
                    enemyChoosedToAttack = enemies[i];
                    
                    AnimationAttack();

                    if (transform.position.x < enemyChoosedToAttack.transform.position.x)
                        RotateLeftHero();
                    else
                        RotateRightHero();
                }
            }
        }
    }

    private void EventShoot()
    {
        source.PlayOneShot(soundShoot, Random.Range(0.6f, 1f));

        GameObject arowGo = Instantiate(arow, positionFire.transform.position, 
                                positionFire.transform.rotation);

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

        if (arowGo != null)
        {
            arowGo.GetComponentInChildren<Arow>().SetDamage(damagePhysic);
            arowGo.GetComponentInChildren<Arow>().SetGameObjectAttacking(enemyChoosedToAttack);
        }
    }

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x0 - x1) / Mathf.Abs(y0 - y1));
        return angle;
    }

    private void AnimationsHeroMove(Vector2 target)
    {
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        float x1 = target.x;
        float y1 = target.y;

        float angle = GetRotateAngleGameObject(x0, y0, x1, y1);

        if (angle < ANGLE_SWAP_STATE)
        {
            if (y0 < y1)
            {
                AnimationMoveBehind();
                shadowBeHind.SetActive(true);
                shadowLeft.SetActive(false);
                shadowBeFore.SetActive(false);
                shadowRight.SetActive(false);
            }
            else
            {
                AnimationMoveBefore();
                shadowBeHind.SetActive(false);
                shadowLeft.SetActive(false);
                shadowBeFore.SetActive(true);
                shadowRight.SetActive(false);
            }
        }
        else if (angle >= ANGLE_SWAP_STATE)
        {
            AnimationMoveLR();

            if (transform.position.x < target.x)
            {
                RotateLeftHero();
            }
            else
            {
                RotateRightHero();
            }
        }
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
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBehind", true);
    }

    public void AnimationMoveBefore()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBefore", true);
    }

    public void AnimationAttack()
    {
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveBehind", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsAttack", true);
    }
}
