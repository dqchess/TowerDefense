using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{

    public Text healthText;
    public Text currencyText;
    public Text waveText;

    public int towerCurrency = 200;
    public int health = 20;
    public int maxWaves = 15;
    public int currentWave = 0;

    private void Update()
    {
        healthText.text = health.ToString();
        currencyText.text = towerCurrency.ToString();
        waveText.text = currentWave.ToString() + "/" + maxWaves.ToString();
    }
}
