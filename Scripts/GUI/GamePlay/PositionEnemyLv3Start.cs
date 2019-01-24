using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEnemyLv3Start : MonoBehaviour
{

    public GameObject iceDemon;
    public GameObject oskBane;

    private const float TIME_RETURN_WAVE_ENEMY_LV3 = 20f;
    private const float TIME_RETURN_NEXT_ENEMY = 5f;
    private int numberEnemyLv3;
    private float timeStartWaveEnemyLV3;
    private float timeStartBossOskBane;
    private bool isShowBoss;

    void Start()
    {
        timeStartBossOskBane = 10f;
        timeStartWaveEnemyLV3 = 60f;
        numberEnemyLv3 = 3; 
    }

    void Update()
    {
        if (Time.time >= timeStartWaveEnemyLV3)
        {
            StartCoroutine(CallWaveIceDemon());
            timeStartWaveEnemyLV3 = Time.time + TIME_RETURN_WAVE_ENEMY_LV3;
        }

        if (Time.time >= timeStartBossOskBane && !isShowBoss)
        {
            CallEnemyOskBane();
            isShowBoss = true;
        }
    }

    IEnumerator CallWaveIceDemon()
    {
        for (int i = 0; i < numberEnemyLv3; i++)
        {
            CallEnemyIceDemon();
            yield return new WaitForSeconds(TIME_RETURN_NEXT_ENEMY);
        }
    }

    private void CallEnemyIceDemon()
    {
        Instantiate(iceDemon, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
    }

    private void CallEnemyOskBane()
    {
        Instantiate(oskBane, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
    }
}
