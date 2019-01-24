using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundBulletGame
{
    ARROW,
    LIGHT_LEVEL_1,
    LIGHT_LEVEL_2,
    LIGHT_LEVEL_3,
    BOOM
}

public class SoundBullet : MonoBehaviour {

    public AudioClip soundArrow;
    public AudioClip soundLightLevel1;
    public AudioClip soundLightLevel2;
    public AudioClip soundLightLevel3;
    public AudioClip soundBoom;

    public static SoundBullet instance;

    void Start () {
        instance = this;
	}
	

	void Update () {
		
	}

    public static void PlaySound(soundBulletGame currentSound)
    {
        switch (currentSound)
        {
            case soundBulletGame.ARROW:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.soundArrow);
                    break;
                }
            case soundBulletGame.LIGHT_LEVEL_1:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.soundLightLevel1);
                    break;
                }
            case soundBulletGame.LIGHT_LEVEL_2:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.soundLightLevel2);
                    break;
                }
            case soundBulletGame.LIGHT_LEVEL_3:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.soundLightLevel3);
                    break;
                }
            case soundBulletGame.BOOM:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.soundBoom);
                    break;
                }
        }
    }
}
