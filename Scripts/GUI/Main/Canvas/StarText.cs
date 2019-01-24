using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarText : MonoBehaviour {

    public Text starText;
    public int starGain;

    private void Awake()
    {
        starGain = 0;
        starText.text = starGain.ToString();
    }
}
