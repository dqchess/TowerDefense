using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public GameObject optionBoard;
    public GameObject startMapBoard;
    private Vector3 optionBoardPos;
    private Vector3 startMapBoardPos;

    private void Update()
    {
        optionBoardPos = optionBoard.transform.position;
        startMapBoardPos = startMapBoard.transform.position;

        optionBoard.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        startMapBoard.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (Input.GetMouseButton(0) && transform.position.y ==0)
        {
            if (Input.GetAxis("Mouse X") < 0.1)
            {
                transform.Translate(new Vector3(7 * Time.deltaTime, 0, 0));
            }
            if (Input.GetAxis("Mouse X") > -0.1)
            {
                transform.Translate(new Vector3(-7 * Time.deltaTime, 0, 0));
            }
        }

        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, 0, -10);
        }

        if (transform.position.x > 5.31f)
        {
            transform.position = new Vector3(5.31f, 0, -10);
        }
    }

    
}
