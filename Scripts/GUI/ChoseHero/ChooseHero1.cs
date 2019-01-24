using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseHero1 : MonoBehaviour {

    private const float MAX_HP = 500f;
    private const float MAX_AMOR = 20f;
    private const float MAX_DAME_PHYSIC = 50f;
    private const float MAX_SPEED = 3f;
    private const float MAX_RANGE = 5f;
    private const float MAX_DAME_SPELL = 50f;

    public GameObject damageBar;
    public GameObject amorBar;
    public GameObject damageSpellBar;
    public GameObject hpBar;
    public GameObject speedBar;
    public GameObject rangeBar;

    public GameObject HeroBoard;
    private ButtonAdd classBtnAdd;
    public GameObject borderSelect;
    public GameObject buttonAdd;
    public int heroIndex;

    private const float SCALE_X = 0.559f;
    public float hp;
    public float dam;
    public float amor;
    public float damspell;
    public float speed;
    public float range;

    public bool isActived;

    void Start()
    {
        classBtnAdd = buttonAdd.GetComponent<ButtonAdd>();
        if (gameObject.name == "BtnES")
        {
            UpdateBar();
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnMouseDown()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < HeroBoard.transform.childCount; i++) 
        {
            if (HeroBoard.transform.GetChild(i).gameObject.name != gameObject.name)
            {
                HeroBoard.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
        borderSelect.transform.position = transform.position;
        classBtnAdd.selectedHeroIndex = heroIndex;
        UpdateBar();
    }

    private void UpdateBar()
    {
        float hpNew = SCALE_X * hp / MAX_HP;
        hpBar.transform.localScale = new Vector3(hpNew, 1, 1);

        float damNew = SCALE_X * dam / MAX_DAME_PHYSIC;
        damageBar.transform.localScale = new Vector3(damNew, 1, 1);

        float amorNew = SCALE_X * amor / MAX_AMOR;
        amorBar.transform.localScale = new Vector3(amorNew, 1, 1);

        float dameSpellNew = SCALE_X * damspell / MAX_DAME_SPELL;
        damageSpellBar.transform.localScale = new Vector3(dameSpellNew, 1, 1);

        float speedNew = SCALE_X * speed / MAX_SPEED;
        speedBar.transform.localScale = new Vector3(speedNew, 1, 1);

        float rangeNew = SCALE_X * range / MAX_RANGE;
        rangeBar.transform.localScale = new Vector3(rangeNew, 1, 1);
    }
}
