using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloseHeroCamp : MonoBehaviour {

    public GameObject mainCamera;

    private void OnMouseDown()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }
}
