using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour {

    GameObject UIGamePlay;
    public Transform towerSelect;
    public Sprite overlayImage;
    Animator[] anim;
    Sprite startImage;
    bool shouldbeActive;
    bool activeInCurrent;

    private void Start()
    {
        UIGamePlay = GameObject.Find("UIGamePlay");
        startImage = gameObject.GetComponent<SpriteRenderer>().sprite;
        towerSelect = this.transform.Find("Tower Select");
        activeInCurrent = false;
        anim = new Animator[7];
        for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
            anim[i] = gameObject.transform.GetChild(0).transform.GetChild(i).GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && shouldbeActive == false && activeInCurrent == true)
        {
            for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
            {
                if (!anim[i].GetCurrentAnimatorStateInfo(0).IsName("BackgroundIcon_FadeIn")
                    && !anim[i].GetCurrentAnimatorStateInfo(0).IsName("BackgroundIcon_FadeOut"))
                    anim[i].SetTrigger("Hide");
            }
            StartCoroutine(Wait());
            activeInCurrent = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.35f);
        towerSelect.gameObject.SetActive(false);
    }
    
    private void OnMouseOver()
    {
        shouldbeActive = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = overlayImage;
        if (Input.GetMouseButtonDown(0))
        {
            if (activeInCurrent == true)
            {
                for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
                {
                    if (!anim[i].GetCurrentAnimatorStateInfo(0).IsName("BackgroundIcon_FadeIn")
                        && !anim[i].GetCurrentAnimatorStateInfo(0).IsName("BackgroundIcon_FadeOut"))
                        anim[i].SetTrigger("Hide");
                }
                StartCoroutine(Wait());
            }
            else
            {
                towerSelect.gameObject.SetActive(true);
            }
            activeInCurrent = !activeInCurrent;
        }
    }

    private void OnMouseExit()
    {
        shouldbeActive = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = startImage;
    }

    //void BuildTower(GameObject towerType)
    //{
    //    if (UIGamePlay.GetComponent<UIGamePlay>().towerCurrency >= towerType.GetComponent<Tower>().towerCost)
    //    {
    //        Instantiate(towerType, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    //        UIGamePlay.GetComponent<UIGamePlay>().towerCurrency -= towerType.GetComponent<Tower>().towerCost;
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Debug.Log("Not enough money!");
    //    }
    //}

    void ActiveProcessBar(GameObject towerType)
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(ProcessingBar(towerType));
    }

    IEnumerator ProcessingBar(GameObject towerType)
    {
        yield return new WaitForSeconds(.8f);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        Instantiate(towerType, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        Destroy(gameObject);
        Debug.Log("Build done!");
    }
}
