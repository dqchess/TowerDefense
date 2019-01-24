using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMagic : MonoBehaviour
{
    public GameObject effect;
    public AudioClip soundEffect;

    private const float SPEED = 5f;
    private const float DAMAGE = 30f;
    private const float TIME_COUNTDOWN_EFFECT = 0.02f;

    private GameObject gameObjectTarget;
    private AudioSource source;

    private float halfHeightTarget;
    private float timeStartEffect;

    void Awake()
    {
        timeStartEffect = Time.time;
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (gameObjectTarget != null)
            halfHeightTarget = gameObjectTarget.GetComponent<Renderer>().bounds.size.y / 2;

        source.PlayOneShot(soundEffect, Random.Range(0.4f, 0.8f));
    }

    void Update()
    {
        if (gameObjectTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        RotateBullet();
       
        transform.position = Vector2.MoveTowards(transform.position, 
            gameObjectTarget.transform.position + new Vector3(0f, halfHeightTarget, 0f), SPEED * Time.deltaTime);

        if (new Vector2(transform.position.x, transform.position.y) == new Vector2(
                gameObjectTarget.transform.position.x, gameObjectTarget.transform.position.y + halfHeightTarget))
        {
            if (gameObjectTarget.layer == 9)
                gameObjectTarget.GetComponentInChildren<Dragon>().SubHealth(DAMAGE);
            else if (gameObjectTarget.layer == 8)
                gameObjectTarget.GetComponentInChildren<Demon>().SubHealth(DAMAGE);
            else if (gameObjectTarget.layer == 14)
                gameObjectTarget.GetComponentInChildren<OskBane>().SubHealth(DAMAGE);
            else if (gameObjectTarget.layer == 15)
                gameObjectTarget.GetComponentInChildren<IceDemon>().SubHealth(DAMAGE);
            else if (gameObjectTarget.layer == 16)
                gameObjectTarget.GetComponentInChildren<IceDemonChild>().SubHealth(DAMAGE);
            else if (gameObjectTarget.layer == 17)
                gameObjectTarget.GetComponentInChildren<Destroyer>().SubHealth(DAMAGE);

            Destroy(gameObject);
        }

        if (Time.time - timeStartEffect > TIME_COUNTDOWN_EFFECT)
        {
            Instantiate(effect, transform.position, transform.rotation);
            timeStartEffect = Time.time + TIME_COUNTDOWN_EFFECT;
        }
    }

    public void SetTarget(GameObject target)
    {
        gameObjectTarget = target;
    }

    private void RotateBullet()
    {
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        float x1 = gameObjectTarget.transform.position.x;
        float y1 = gameObjectTarget.transform.position.y;

        float angle = GetRotateAngleGameObject(x0, y0, x1, y1);

        if (y0 < y1)
            transform.rotation = Quaternion.Euler(0f, 0f, 90f + angle);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 90f - angle);           
    }

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan((x0 - x1) / (y0 - y1));
        return angle;
    }
}
