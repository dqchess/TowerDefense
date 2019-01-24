using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : MonoBehaviour
{

    public GameObject army;

    private const float TIME_RETURN_ARMY = 30f;
    private float timeAppear;

    
    void Awake()
    {
        timeAppear = Time.time;
    }

    void Update()
    {
        if (Time.time > timeAppear)
        {
            Instantiate(army, transform.position, 
                Quaternion.Euler(new Vector3(0, 0, 0)));

            timeAppear = Time.time + TIME_RETURN_ARMY;
        }
    }
}
