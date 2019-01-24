using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEnemyStart : MonoBehaviour
{

    public GameObject enemy;
    public GameObject demon;

    private const float TIME_RETURN_WAVE_ENEMY_LV1_2 = 10f;
    private const float TIME_RETURN_NEXT_ENEMY = 3f;

    private int index;
    private float timeStartWaveEnemyLV1_2;

    void Start()
    {
        index = 0;
        timeStartWaveEnemyLV1_2 = Time.time; 
    }

    void Update()
    {
        if (Time.time >= timeStartWaveEnemyLV1_2)
        {
            index++;
            if (index > 8)
                index = 8;
            
            StartCoroutine(CallWaveDragon());
            CallEnemyDemon();
            timeStartWaveEnemyLV1_2 = Time.time + TIME_RETURN_WAVE_ENEMY_LV1_2;
        }
    }

    IEnumerator CallWaveDragon()
    {
        for (int i = 0; i < index; i++)
        {
            CallEnemyDragon();
            yield return new WaitForSeconds(TIME_RETURN_NEXT_ENEMY);
        }
    }

    private void CallEnemyDragon()
    {
        Instantiate(enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    private void CallEnemyDemon()
    {
        Instantiate(demon, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }
}
