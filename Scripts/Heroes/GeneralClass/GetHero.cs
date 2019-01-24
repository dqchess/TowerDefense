using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHero : MonoBehaviour
{

    public GameObject earthShaker;
    public GameObject naga;
    private int getHero_0;
    private int getHero_3;

    void Awake()
    {
        getHero_0 = PlayerPrefs.GetInt("Hero0");
        getHero_3 = PlayerPrefs.GetInt("Hero3");
    }

    void Start()
    {
        if (getHero_0 == 0)
            Instantiate(earthShaker, transform.position, transform.rotation);
        if (getHero_3 == 3)
            Instantiate(naga, transform.position, transform.rotation);
    }
}
