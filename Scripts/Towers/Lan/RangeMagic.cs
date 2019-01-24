using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMagic : MonoBehaviour {

    float attackDamage;
    int currentLevel;
    float currentTime;
    private TowerMagic1 towerMagic;

    void Awake()
    {
        towerMagic = gameObject.GetComponentInParent<TowerMagic1>();
        currentLevel = towerMagic.currentLevel;
        attackDamage = towerMagic.attackDamages[currentLevel];
    }

    void Update()
    {
        if (towerMagic.isSelect)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else return;
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            if (col.gameObject.layer == 8)
            {
                Demon demon = col.gameObject.GetComponentInChildren<Demon>();
                demon.SubHealth(attackDamage);
            }
            if (col.gameObject.layer == 9)
            {
                Dragon dragon = col.gameObject.GetComponentInChildren<Dragon>();
                dragon.SubHealth(attackDamage);
            }      
        }
    }
}
