using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerNew : MonoBehaviour
{
    public GameObject chop;
    public GameObject bullet;

    public int attackDamage;
    public float attackSpeed;

    float currentTime;

    bool isLockTarget;
    public GameObject target;
    public GameObject chopEffect;
    private Animator animTower;

    public AudioClip soundBullet;
    private AudioSource source;
    private float lengthSound;

    public bool isUseBoom;
    public bool isUseArrow;
    public bool isUseLightLevel1;

    void Awake()
    {
        //animTower = GetComponent<Animator>();
    }
    void Start()
    {
    }

    void Update()
    {
        if (isLockTarget && currentTime==0)
        {
            Shooting(target);
            currentTime = attackSpeed;        
        }
        Timer();
        if (target == null)
        {
            isLockTarget = false;
        }
    }

    void Timer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
                currentTime = 0;
        }
    }

    public void Shooting(GameObject _target)
    {
        if (target == null)
        {
            Debug.Log("destroy");

        }
        else
        {
            GameObject _bullet = Instantiate(bullet, chop.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            if (isUseArrow)
            {
                ArrowBullet _classArrowBullet = _bullet.GetComponent<ArrowBullet>();
                _classArrowBullet.target = _target.transform.position;
                _classArrowBullet.Target = _target;
            }
            if (isUseBoom)
            {
                BoomBullet _classBoomBullet = _bullet.GetComponent<BoomBullet>();
                _classBoomBullet.target = _target.transform.position;
                _classBoomBullet.Target = _target;
            }
            if (isUseLightLevel1)
            {
                LightBullet _classLightBullett = _bullet.GetComponent<LightBullet>();
                _classLightBullett.target = _target.transform.position;
            }


            //SoundBullet.PlaySound(soundBulletGame.ARROW);


        }
    }


    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && !isLockTarget)
        {
            target = coll.gameObject;
            isLockTarget = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && isLockTarget)
        {
            isLockTarget = false;
        }

    }
}
