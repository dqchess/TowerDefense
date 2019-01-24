using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackLV2 : MonoBehaviour
{

    public GameObject dwarfLV2;

    private const float TIME_RETURN_ARMY = 10f;
    private const int NUMBER_ARMY = 2;
    private float timeAppear;
    private float minDistance;

    private GameObject[] armies;
    private Vector2 posDef;
    private Vector2 posDef_1;
    private Vector2 posDef_2;
    private GameObject[] pointDef;


    void Awake()
    {
        timeAppear = Time.time;
        armies = new GameObject[NUMBER_ARMY];

        GameObject dwarf_1 = Instantiate(dwarfLV2, transform.position, 
                                 Quaternion.Euler(new Vector3(0, 0, 0)));
        GameObject dwarf_2 = Instantiate(dwarfLV2, transform.position, 
                                 Quaternion.Euler(new Vector3(0, 0, 0)));

        armies[0] = dwarf_1;
        armies[1] = dwarf_2;
    }

    void Start()
    {

        pointDef = GameObject.FindGameObjectsWithTag("PointDef");
        minDistance = Vector2.Distance(transform.position, pointDef[0].transform.position);

        for (int i = 0; i < pointDef.Length; i++)
        {
            if (minDistance > Vector2.Distance(transform.position, pointDef[i].transform.position))
            {
                posDef = new Vector2(pointDef[i].transform.position.x, 
                    pointDef[i].transform.position.y);
                minDistance = Vector2.Distance(transform.position, pointDef[i].transform.position);
            }
        }
            
        posDef_1 = new Vector2(posDef.x + 0.3f, posDef.y + 0.1f);
        posDef_2 = new Vector2(posDef.x - 0.3f, posDef.y - 0.1f);

        armies[0].GetComponentInChildren<DwarfLV2>().GetPositionStart(posDef_1);
        armies[1].GetComponentInChildren<DwarfLV2>().GetPositionStart(posDef_2);
    }

    void Update()
    {
        if (Time.time - timeAppear > 0)
        {
            for (int i = 0; i < NUMBER_ARMY; i++)
            {
                if (armies[i] == null)
                {
                    GameObject dwarf = Instantiate(dwarfLV2, transform.position, 
                                           Quaternion.Euler(new Vector3(0, 0, 0)));

                    if (i == 0)
                        armies[0] = dwarf;
                    else if (i == 1)
                        armies[1] = dwarf;
                }
            }

            armies[0].GetComponentInChildren<DwarfLV2>().GetPositionStart(posDef_1);
            armies[1].GetComponentInChildren<DwarfLV2>().GetPositionStart(posDef_2);

            timeAppear = Time.time + TIME_RETURN_ARMY;
        }
    }
}
