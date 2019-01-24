using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    
    public GameObject enemy;
    public GameObject spawnPoint;
    public Text healthText;
    public Text currencyText;
    public Text waveText;
    public float spawnTimer = 20f;
    public int towerCurrency = 200;
    public int health = 20;
    public int maxWaves = 7;
    public int currentWave = 0;
    public int numberToSpawn;
    
    private void Start()
    {
        StartCoroutine(WaveSpawner());
    }

    private void Update()
    {
        healthText.text = "Health: " + health.ToString();
        currencyText.text = "Currency: " + towerCurrency.ToString();
        waveText.text = "Wave: " + currentWave.ToString() + "/" + maxWaves.ToString();
    }

    IEnumerator WaveSpawner()
    {
        while (true)
        {
            if (currentWave < maxWaves)
            {
                currentWave++;
                switch (currentWave)
                {
                    case 1:
                        numberToSpawn = 4;
                        break;
                    case 2:
                        numberToSpawn = 6;
                        break;
                    case 3:
                        numberToSpawn = 8;
                        break;
                    case 4:
                        numberToSpawn = 10;
                        break;
                    case 5:
                        numberToSpawn = 12;
                        break;
                    case 6:
                        numberToSpawn = 14;
                        break;
                    case 7:
                        numberToSpawn = 15;
                        break;
                }

                for (int i = 0; i < numberToSpawn; i++)
                {
                    Instantiate(enemy, new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y), Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }

                yield return new WaitForSeconds(spawnTimer);
            }
            else
                yield break;
        }

    }
}
