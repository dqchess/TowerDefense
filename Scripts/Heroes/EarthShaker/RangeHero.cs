using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHero : MonoBehaviour
{

    public GameObject earthShaker;
    public GameObject effectLocationTouch;

    private EarthShaker es;
    private ChooseHero chooseHero;
    private Vector2 targetPosition;
    private GameObject isDestroyed;

    private const float TIME_COUNTDOWN_SKILL_EARTH = 2f;
    private const float ANGLE_SWAP_STATE = 60f;
    private float speedMove;
    private float nextEarthSkill;
    private bool checkAttack;
    private bool checkIdle;

    void Awake()
    {
        targetPosition = new Vector2(transform.position.x, transform.position.y);
        es = earthShaker.GetComponentInChildren<EarthShaker>();
        chooseHero = earthShaker.GetComponentInChildren<ChooseHero>();
    }

    void Start()
    {
        speedMove = gameObject.GetComponentInChildren<EarthShaker>().speedmove;
        nextEarthSkill = Time.time - TIME_COUNTDOWN_SKILL_EARTH;
    }

    void Update()
    {
        if (Time.time - nextEarthSkill >= TIME_COUNTDOWN_SKILL_EARTH && checkAttack)
            MessageAttack();

        if (isDestroyed == null)
            checkAttack = false;

        if (Input.GetMouseButtonDown(0) && chooseHero.isChoosedGameObject)
        {
            Ray clickedPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            targetPosition = new Vector2(clickedPosition.origin.x, clickedPosition.origin.y);
            chooseHero.SetIsChoosed(false);

            Instantiate(effectLocationTouch, targetPosition, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        }

        if (!checkAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 
                speedMove * Time.deltaTime);

            AnimationsHeroMove(targetPosition);
        }

        if (new Vector2(transform.position.x, transform.position.y) == targetPosition && !checkAttack)
        {
            es.AnimationIdle();
            checkIdle = true;
        }
        else if (new Vector2(transform.position.x, transform.position.y) != targetPosition)
        {
            checkIdle = false;
            checkAttack = false;
        }
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
                es.AnimationMoveBehind();
            }
            else
            {
                es.AnimationMoveBefore();
            }
        }
        else if (angle >= ANGLE_SWAP_STATE)
        {
            es.AnimationMoveLR();

            GameObject gameObectES = GameObject.Find("ES");
            SpriteRenderer sr = gameObectES.GetComponent<SpriteRenderer>();
            if (transform.position.x < target.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && coll.gameObject.layer != 9)
        {
            if (checkIdle)
                checkAttack = true;

            if (coll.gameObject.layer == 14)
                coll.gameObject.GetComponentInChildren<OskBane>().SetGameObjectAttacker(earthShaker);
            else if (coll.gameObject.layer == 8)
                coll.gameObject.GetComponentInChildren<Demon>().SetGameObjectAttacker(earthShaker);
            else if (coll.gameObject.layer == 15)
                coll.gameObject.GetComponentInChildren<IceDemon>().SetGameObjectAttacker(earthShaker);
            else if (coll.gameObject.layer == 16)
                coll.gameObject.GetComponentInChildren<IceDemonChild>().SetGameObjectAttacker(earthShaker);
            else if (coll.gameObject.layer == 17)
                coll.gameObject.GetComponentInChildren<Destroyer>().SetGameObjectAttacker(earthShaker);
            
            SetGameObjectIsDestroyed(coll.gameObject);
        }
    }

    private void SetGameObjectIsDestroyed(GameObject g)
    {
        this.isDestroyed = g;
    }

    private void MessageAttack()
    {
        es.AnimationAttack();
        nextEarthSkill = Time.time;
    }


    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x0 - x1) / Mathf.Abs(y0 - y1));
        return angle;
    }

}
