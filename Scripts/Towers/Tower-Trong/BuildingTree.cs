using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTree : MonoBehaviour {

    [HideInInspector]
    public TowerController myTower;
    
    void Start()
    {
        Debug.Assert(myTower, "Wrong initial parameters");
    }

    public void Build(GameObject prefab)
    {
        myTower.BuildTower(prefab);
    }
}
