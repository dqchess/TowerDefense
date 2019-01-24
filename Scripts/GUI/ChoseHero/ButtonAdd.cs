using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonAdd : MonoBehaviour {

    public int selectedHeroIndex;
    public GameObject heroCell;
    private HeroCell classHC;
    public GameObject mainCamera;
    public GameObject heroList;
    public GameObject Es;
    public GameObject Wr;
    public GameObject Naga;
    public GameObject Puck;
    public GameObject Mal;

    private void Awake()
    {
        classHC = heroCell.GetComponent<HeroCell>();
    }

    private void OnMouseDown()
    {
        GameObject[] hero = { Es, Wr, Mal, Naga, Puck };
        mainCamera.transform.position = new Vector3(0, 0, -10);
        Instantiate(hero[selectedHeroIndex], heroCell.transform.GetChild(classHC.SelectedCellIndex));
        heroList.transform.GetChild(selectedHeroIndex).GetChild(1).gameObject.SetActive(true);
    }
}
    