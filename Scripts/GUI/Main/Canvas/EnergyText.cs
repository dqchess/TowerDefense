using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyText : MonoBehaviour {

    public Text energyText;
    public int energy;

    private void Start()
    {
        energy = 100;
        energyText.text = energy.ToString() + "/100";
    }

}
