using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : MonoBehaviour
{

    public Animator anim;
    public GameObject barBlood;
    public float damageBasic;
    public float amor;
    public float currentHealth;

    private GameObject gameObjectChoosedToAttack;
    private Demon demon;
    private Destroyer destroyer;
    private OskBane osk;
    private IceDemon iceDemon;
    private IceDemonChild iceDemonChild;
    private float scaleX;

    private const int DRAGON = 1;
    private const float HEALTH = 220;

    void Awake()
    {
        anim = GetComponent<Animator>();
        damageBasic = 5f;
        amor = 5f;
        scaleX = barBlood.transform.localScale.x;
        currentHealth = HEALTH;
    }

    public void AnimationIdle()
    {
        anim.SetBool("IsMoveHind", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", true);
    }

    public void AnimationAttack()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveHind", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsAttack", true);
    }

    public void AnimationMoveBeHind()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveHind", true);
    }

    public void AnimationMoveBefore()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveLR", false);
        anim.SetBool("IsMoveHind", false);
        anim.SetBool("IsMoveBefore", true);
    }

    public void AnimationMoveLR()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoveHind", false);
        anim.SetBool("IsMoveBefore", false);
        anim.SetBool("IsMoveLR", true);
    }

    public void SubHealth(float damage)
    {
        float damageReceive = damage - damage * amor / 100;
        if (damageReceive <= 0)
            damageReceive = 1;

        currentHealth -= damageReceive;
        UpdateBarBlood();
    }

    public void AddHealth(float healthReceive)
    {
        currentHealth += healthReceive;
        UpdateBarBlood();
    }

    private void UpdateBarBlood()
    {
        if (currentHealth < 0f)
        {
            currentHealth = 0f;
        }

        if (currentHealth >= HEALTH)
            currentHealth = HEALTH;

        Vector2 scale = barBlood.transform.localScale;
        scale.x = scaleX * currentHealth / HEALTH;
        barBlood.transform.localScale = scale;
    }

    public void AddAmor(float amorReceive)
    {
        amor += amorReceive;
    }

    public void SetGameObjectToAttack(GameObject gameObjectChoosed)
    {
        this.gameObjectChoosedToAttack = gameObjectChoosed;
    }

    private void Attack()
    {
        if (gameObjectChoosedToAttack != null)
        {
            if (gameObjectChoosedToAttack.layer == 8)
            {
                demon = gameObjectChoosedToAttack.GetComponentInChildren<Demon>();
                demon.SubHealth(damageBasic);
                demon.SetGameObjectAttacker(gameObject);
            }
            else if (gameObjectChoosedToAttack.layer == 14)
            {
                osk = gameObjectChoosedToAttack.GetComponentInChildren<OskBane>();
                osk.SubHealth(damageBasic);
                osk.SetGameObjectAttacker(gameObject);
            }
            else if (gameObjectChoosedToAttack.layer == 15)
            {
                iceDemon = gameObjectChoosedToAttack.GetComponentInChildren<IceDemon>();
                iceDemon.SubHealth(damageBasic);
                iceDemon.SetGameObjectAttacker(gameObject);
            }
            else if (gameObjectChoosedToAttack.layer == 16)
            {
                iceDemonChild = gameObjectChoosedToAttack.GetComponentInChildren<IceDemonChild>();
                iceDemonChild.SubHealth(damageBasic);
                iceDemonChild.SetGameObjectAttacker(gameObject);
            }
            else if (gameObjectChoosedToAttack.layer == 17)
            {
                destroyer = gameObjectChoosedToAttack.GetComponentInChildren<Destroyer>();
                destroyer.SubHealth(damageBasic);
                destroyer.SetGameObjectAttacker(gameObject);
            }
        }
    }
}
