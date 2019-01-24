using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int damage = 1;
    public int health = 100;
    public int maxHealth = 100;
    public int maxReward = 3;
    public int minReward = 1;
    public Image healthBar;
    public GameObject _BattleManager;

    void Start()
    {
        _BattleManager = GameObject.Find("BattleManager");
        healthBar = transform.Find("EnemyCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
    }

    // Damage overall health and destroy this enemy
    public void DamagePlayerHealth()
    {
        _BattleManager.GetComponent<BattleManager>().health -= damage;
        Destroy(this.gameObject);
    }

    public void DamageUnitHealth()
    {

    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        healthBar.fillAmount = (float)health / maxHealth;

        if (health <= 0)
        {
            _BattleManager.GetComponent<BattleManager>().towerCurrency += Random.Range(minReward, maxReward);
            Destroy(this.gameObject);
        }
    }
}
