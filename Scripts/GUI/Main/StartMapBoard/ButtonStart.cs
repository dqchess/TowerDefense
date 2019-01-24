using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonStart : MonoBehaviour {

    public int sceneIndex;
    public Text energyText;
    private EnergyText energyClass;
    public GameObject heroCell;
    private HeroCell classHC;

    private void Awake()
    {
        energyClass = energyText.GetComponent<EnergyText>();
        classHC = heroCell.GetComponent<HeroCell>();
    }

    private void OnMouseDown()
    {
        UpdateEnergyBar();
        SceneManager.LoadScene(sceneIndex+1, LoadSceneMode.Single);
        for (int i = 0; i < 5; i++)
        {
            if (classHC.checkHeroExist[i])
            {
                PlayerPrefs.SetInt("Hero", 1);
                Debug.Log("Hero" + i);
            }
        }
    }

    private void UpdateEnergyBar()
    {
        energyClass.energy -= 10;
        energyClass.energyText.text = energyClass.energy.ToString() + "/100";
    }

}
