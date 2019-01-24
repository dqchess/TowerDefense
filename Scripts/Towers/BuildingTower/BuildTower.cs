using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTower : MonoBehaviour
{

    [Header("GameObject Child")]
    public GameObject chooseTypeTower;
    public GameObject chooseTowerMagic;
    public GameObject chooseTowerDef;
    public GameObject chooseTowerSkill;
    public GameObject upgradeOrCost;
    public GameObject cost;
    public GameObject upgradeTower;

    [Header("Towers")]
    public GameObject towerMagicLV1;
    public GameObject towerMagicLV2;
    public GameObject towerMagicLV3;
    public GameObject towerMagicLV4;
    public GameObject towerDefLV1;
    public GameObject towerSkillLV1;

    private ChooseTowerMagic chooseTM;
    private ChooseToweDef chooseDef;
    private ChooseTowerSkill chooseSkill;
    private Vector2 targetPosition;
    private Vector3 position;
    private bool checkBuildTowerMagic;
    private bool checkBuildTowerDef;
    private bool checkBuildTowerSkill;

    void Awake()
    {
        if (chooseTowerMagic != null)
            chooseTM = chooseTowerMagic.GetComponentInChildren<ChooseTowerMagic>();
        if (chooseTowerDef != null)
            chooseDef = chooseTowerDef.GetComponentInChildren<ChooseToweDef>();
        if (chooseTowerSkill != null)
            chooseSkill = chooseTowerSkill.GetComponentInChildren<ChooseTowerSkill>();
    }

    void Start()
    {
        position = gameObject.transform.position;
    }

    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)(-gameObject.transform.position.y * 100);

        CheckTouch();

        if (chooseTM != null)
        {
            if (chooseTM.isBuild && !checkBuildTowerMagic)
            {
                chooseTypeTower.SetActive(false);
                Instantiate(towerMagicLV1, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                checkBuildTowerMagic = true;
            }
        }

        if (chooseDef != null)
        {
            if (chooseDef.isBuild && !checkBuildTowerDef)
            {
                chooseTypeTower.SetActive(false);
                Instantiate(towerDefLV1, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                checkBuildTowerDef = true; 
            }
        }

        if (chooseSkill != null)
        {
            if (chooseSkill.isBuild && !checkBuildTowerSkill)
            {
                chooseTypeTower.SetActive(false);
                Instantiate(towerSkillLV1, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                checkBuildTowerSkill = true; 
            }
        }
    }

    void OnMouseDown()
    {
        if (!checkBuildTowerMagic && !checkBuildTowerDef && !checkBuildTowerSkill)
            chooseTypeTower.SetActive(true);
        else if (checkBuildTowerMagic || checkBuildTowerDef || checkBuildTowerSkill)
            upgradeOrCost.SetActive(true);
    }

    private void CheckTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray clickedPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            targetPosition = new Vector2(clickedPosition.origin.x, clickedPosition.origin.y);
        }
            

        Vector3 sizeGameObject = gameObject.GetComponent<Renderer>().bounds.size;

        if ((position.x + sizeGameObject.x >= targetPosition.x / 2 && targetPosition.x >= position.x - sizeGameObject.x / 2) &&
            (position.y + sizeGameObject.y / 2 >= targetPosition.y && targetPosition.y >= position.y - sizeGameObject.y / 2))
        {
            //Will be code later
            Debug.Log("Test Touch In GameObject");
        }
    }
}
