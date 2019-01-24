using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    public GameObject buildingTreePrefab;

    public GameObject rangeImage;
    private Collider2D bodyCollider;
    private BuildingTree activeBuildingTree;

    private void Start()
    {
        bodyCollider = GetComponent<BoxCollider2D>();
    }

    private void OpenBuildingTree()
    {
        if (buildingTreePrefab != null)
        {
            activeBuildingTree = Instantiate<GameObject>(buildingTreePrefab, gameObject.transform).GetComponent<BuildingTree>();
            activeBuildingTree.transform.position = gameObject.transform.position;
            activeBuildingTree.myTower = this;
            bodyCollider.enabled = false;
        }
    }

    private void CloseBuildingTree()
    {
        if (activeBuildingTree != null)
        {
            Destroy(activeBuildingTree.gameObject);
            bodyCollider.enabled = true;
        }
    }

    public void BuildTower(GameObject towerPrefab)
    {
        CloseBuildingTree();

        // if enough money to build

        GameObject newTower = Instantiate<GameObject>(towerPrefab, transform.parent);
        newTower.transform.position = transform.position;
        newTower.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }

    private void UserClick(GameObject obj, string param)
    {
        if (obj == gameObject) // click in this tower
        {
            // Show range;
            if (activeBuildingTree == null) OpenBuildingTree();
        }
        else
        {
            // Hide range;
            CloseBuildingTree();
        }
    }


}
