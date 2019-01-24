using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelect1 : MonoBehaviour {

    public GameObject range;
    private SpriteRenderer mySpriteRenderer;

	void Start () {
        mySpriteRenderer = range.GetComponent<SpriteRenderer>();
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        }
    }
}
