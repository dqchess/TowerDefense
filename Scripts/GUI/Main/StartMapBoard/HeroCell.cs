using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCell : MonoBehaviour {

    private const int AMOUNT_OF_HERO = 5;
    public bool[] checkHeroExist;
    public int SelectedCellIndex;

    private void Awake()
    {
        checkHeroExist = new bool[AMOUNT_OF_HERO];
    }

    private void Update()
    {
        Check();
        //for (int index = 0; index < AMOUNT_OF_HERO; index++)
        //{
        //    Debug.Log(index + "+" + checkHeroExist[index]);
        //}
    }

    private void Check()
    {
        for (int i = 0; i < AMOUNT_OF_HERO; i++)
        {
            checkHeroExist[i] = false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                switch (transform.GetChild(i).GetChild(j).gameObject.name)
                {
                    case "ES(Clone)":
                        checkHeroExist[0] = true;
                        break;
                    case "WR(Clone)":
                        checkHeroExist[1] = true;
                        break;
                    case "MAL(Clone)":
                        checkHeroExist[2] = true;
                        break;
                    case "NAGA(Clone)":
                        checkHeroExist[3] = true;
                        break;
                    case "PUCK(Clone)":
                        checkHeroExist[4] = true;
                        break;
                }
            }
        }
    }







}
