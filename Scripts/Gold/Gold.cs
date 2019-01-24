using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{

    private int gold;

    void Awake()
    {
        gold = 0;
    }

    void Update()
    {
        // will be update later
    }

    public void AddGold(int goldNeedAdd)
    {
        gold += goldNeedAdd;
    }

    public void SubGold(int goldNeedSub)
    {
        gold -= goldNeedSub;
        if (gold <= 0)
            gold = 0;
    }
}
