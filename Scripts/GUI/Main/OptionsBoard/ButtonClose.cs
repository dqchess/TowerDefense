using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClose : MonoBehaviour {

    public GameObject BtnOptions;
    ButtonOptions classBtnOptions;
    private AudioSource pressSound;

    void Awake()
    {
        pressSound = GetComponent<AudioSource>();
        classBtnOptions = BtnOptions.GetComponent<ButtonOptions>();
    }
	void OnMouseDown()
    {
        pressSound.Play();
        classBtnOptions.HideOptions();
    }
}
