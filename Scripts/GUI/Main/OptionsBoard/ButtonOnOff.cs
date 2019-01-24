using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnOff : MonoBehaviour {

    public enum Button
    {
        Music, Sound
    }

    public Button function;
    private AudioSource pressSound;
    public GameObject music;
    public GameObject sound;
    private AudioSource musicSrc;
    private AudioSource soundSrc;
    public GameObject on;
    public GameObject off;
    private bool check;
    

    private void Awake()
    {
        pressSound = GetComponent<AudioSource>();
        on.SetActive(true);
        off.SetActive(false);
        check = false;
    }

    private void OnMouseDown()
    {
        pressSound.Play();
        if (!check)
        {
            on.SetActive(false);
            off.SetActive(true);
            check = true;
        }
        else
        {
            on.SetActive(true);
            off.SetActive(false);
            check = false;
        }

        if(function == Button.Music)
        {
            musicSrc = music.GetComponent<AudioSource>();
            if (on.active == false)
            {
                musicSrc.Pause();
            }
            else
            {
                musicSrc.UnPause();
            }
        }
        else
        {
            soundSrc = sound.GetComponent<AudioSource>();
            if (on.active == false)
            {
                soundSrc.Pause();
            }
            else
            {
                soundSrc.UnPause();
            }
        }
    }
}
