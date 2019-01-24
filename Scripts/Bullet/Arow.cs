using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arow : MonoBehaviour
{

    private const float SPEED = 4f;
    private float damage;
    private float distance;
    private float halfHeightTarget;

    private GameObject gameObjectAttacking;
    private Vector2 follow;

    void Start()
    {
        if (gameObjectAttacking != null)
        {
            distance = Vector2.Distance(transform.position, gameObjectAttacking.transform.position);

            follow = new Vector2(gameObjectAttacking.transform.position.x, 
                gameObjectAttacking.transform.position.y + distance);
        }

        if (gameObjectAttacking != null)
            halfHeightTarget = gameObjectAttacking.GetComponent<Renderer>().bounds.size.y / 2;
    }

    void Update()
    {
        if (gameObjectAttacking == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            follow = Vector2.MoveTowards(follow, new Vector2(gameObjectAttacking.transform.position.x, 
                    gameObjectAttacking.transform.position.y + halfHeightTarget), SPEED * Time.deltaTime);

            transform.position = Vector2.MoveTowards(transform.position, 
                follow, SPEED * Time.deltaTime);
        }

        if (new Vector2(transform.position.x, transform.position.y) == new Vector2(
                gameObjectAttacking.transform.position.x, gameObjectAttacking.transform.position.y + halfHeightTarget))
        {
            Destroy(gameObject);
        }

        if (follow != new Vector2(transform.position.x, transform.position.y))
            RotateBullet();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (gameObjectAttacking != null && coll.gameObject == gameObjectAttacking)
        {
            if (gameObjectAttacking.layer == 8)
                gameObjectAttacking.GetComponentInChildren<Demon>().SubHealth(damage);
            else if (gameObjectAttacking.layer == 9)
                gameObjectAttacking.GetComponentInChildren<Dragon>().SubHealth(damage);
            else if (gameObjectAttacking.layer == 14)
                gameObjectAttacking.GetComponentInChildren<OskBane>().SubHealth(damage);
            else if (gameObjectAttacking.layer == 15)
                gameObjectAttacking.GetComponentInChildren<IceDemon>().SubHealth(damage);
            else if (gameObjectAttacking.layer == 16)
                gameObjectAttacking.GetComponentInChildren<IceDemonChild>().SubHealth(damage);
            else if (gameObjectAttacking.layer == 17)
                gameObjectAttacking.GetComponentInChildren<Destroyer>().SubHealth(damage);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetGameObjectAttacking(GameObject gameObjectAttacking)
    {
        this.gameObjectAttacking = gameObjectAttacking;
    }

    private void RotateBullet()
    {
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        float x1 = follow.x;
        float y1 = follow.y;

        float angle = GetRotateAngleGameObject(x0, y0, x1, y1);

        if (y0 < y1)
            transform.rotation = Quaternion.Euler(0f, 0f, 90f - angle);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 90f + angle);        
    }

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan((x0 - x1) / (y0 - y1));
        return angle;
    }
}
