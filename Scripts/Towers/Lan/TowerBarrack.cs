using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBarrack : MonoBehaviour
{
    public GameObject army;
    public GameObject creating;
    private const float TIME_RETURN_ARMY = 3f;
    private float timeAppear;


    private void Awake()
    {
        timeAppear = Time.time;
    }
    void Start () {
		
	}
	
	void Update () {
		if (Time.time > timeAppear)
        {
       
            for (int i=0; i<3; i++)
            {
                Instantiate(army, creating.transform.position+new Vector3(i*0.2f,i*0.2f,0), Quaternion.Euler(new Vector3(0, 0, 0)));
            }
            
            timeAppear = Time.time + TIME_RETURN_ARMY;
        }
	}
}
