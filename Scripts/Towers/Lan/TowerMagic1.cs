using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMagic1: MonoBehaviour {

    public GameObject rangeMagic;
    public GameObject shadowRangMagic;

    Vector2 targetDefault;
    Vector2 positionDown;
    public GameObject CircleActive;
    //--
    private SpriteRenderer mySpriteRenderer;
    private CapsuleCollider2D myCapsuleCollider;
    //--
    public GameObject[] towerLevels;

    int numberOfUpgrades;
    int refundAmount;
    public int currentLevel;

    public int maxUpgradeLevel;

    public float[] attackDamages = new float[] { 0.5f, 1.5f, 3f};
    public float[] timeAliveOfRangeMagic = new float[] { 1f, 1.5f, 2f };
    public float[] percentSpeedDisable = new float[] { 2f, 5f, 10f };
    public float[] timeCoolDown = new float[] { 10f, 9f, 7f };

    public int[] towerCosts = new int[] { 150, 200, 300 };

    float currentTime;
    public bool isSelect;

    private void Awake()
    {
        targetDefault = rangeMagic.transform.position;
    }

    void Start ()
    {
        mySpriteRenderer = rangeMagic.GetComponent<SpriteRenderer>();
        myCapsuleCollider = rangeMagic.GetComponent<CapsuleCollider2D>();  
    }

    void Update()
    {
        OnMouseOver();
        if (currentTime == 0)
        {
            CircleActive.SetActive(true);
        }
        else CircleActive.SetActive(false);
    }

    void OnMouseOver()
    {
            if (Input.GetMouseButton(0))
            {
                isSelect = true;
                if (currentTime == 0)
                {
                Action();
                currentTime = timeCoolDown[currentLevel];
                }    
            }
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(DelayTimer());
                isSelect = false;
            }
            Timer();
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

    void Action()
    {
        rangeMagic.active = true;
        rangeMagic.GetComponent<SpriteRenderer>().sortingOrder = 5;
        rangeMagic.transform.position = targetDefault;
        mySpriteRenderer.enabled = true;
    }

    IEnumerator DelayTimer()
    {
        rangeMagic.GetComponent<SpriteRenderer>().sortingOrder = 0;
        rangeMagic.transform.position = shadowRangMagic.transform.position;
        myCapsuleCollider.enabled = true;
        yield return new WaitForSeconds(timeAliveOfRangeMagic[currentLevel]);
        rangeMagic.transform.position = targetDefault;
        rangeMagic.active = false;
        mySpriteRenderer.enabled = false;
        myCapsuleCollider.enabled = false;
    }
}
