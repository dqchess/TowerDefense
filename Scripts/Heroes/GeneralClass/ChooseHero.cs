using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseHero : MonoBehaviour
{

    public GameObject selected;
    public AudioClip soundWhenChoosedHero;
    public bool isChoosedGameObject;

    private AudioSource source;
    private bool check;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isChoosedGameObject && Input.GetMouseButtonDown(0))
        {
            selected.SetActive(false);
        }

        if (!check)
            isChoosedGameObject = false;
    }

    void OnMouseDown()
    {
        selected.SetActive(true);

        if (gameObject.layer == 10)
            source.PlayOneShot(soundWhenChoosedHero, Random.Range(0.5f, 0.8f));
        if (gameObject.layer == 12)
            source.PlayOneShot(soundWhenChoosedHero, Random.Range(0.5f, 0.8f));
    }

    void OnMouseUp()
    {
        if (selected.activeSelf)
        {
            this.isChoosedGameObject = true;

            this.check = true;
        }
    }

    public void SetIsChoosed(bool check)
    {
        this.check = check;
    }
}
