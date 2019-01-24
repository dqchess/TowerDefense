using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDwarf : MonoBehaviour
{

    private const float ANGLE_SWAP_STATE = 60f;
    private float speedMove;
    private float minDistance;
    private bool isIdle;
    private bool isAttack;
    private bool checkLockTarget;

    private Vector3 targetPosition;
    private Transform enemyTransform;
    private Dwarf dwaft;
    private PointEnemyFollow pointEnemyFollow;

    void Awake()
    {
        speedMove = 0.3f;
        dwaft = gameObject.GetComponentInChildren<Dwarf>();
    }

    void Start()
    {
        GameObject gameObjectPoint = GameObject.FindWithTag("PointsDragonFollow");
        pointEnemyFollow = gameObjectPoint.GetComponent<PointEnemyFollow>();
        minDistance = Vector2.Distance(transform.position, 
            pointEnemyFollow.pointTransform[0].position);

        for (int i = 0; i < pointEnemyFollow.pointTransform.Length - 1; i++)
        {
            if (minDistance > Vector2.Distance(transform.position, 
                    pointEnemyFollow.pointTransform[i + 1].position))
            {
                targetPosition = pointEnemyFollow.pointTransform[i + 1].position;
                minDistance = Vector2.Distance(transform.position, targetPosition);
            } 
        }

        targetPosition = new Vector3(targetPosition.x + Random.Range(-0.3f, 0.3f),
            targetPosition.y, targetPosition.z);
    }

    void Update()
    {  
        if (dwaft.currentHealth <= 0)
            Destroy(gameObject);

        if (!isAttack)
        {
            AnimationFollowEnemy(targetPosition);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 
                speedMove * Time.deltaTime);
        }

        if (transform.position == targetPosition && !isIdle)
        {
            dwaft.AnimationIdle();
            isIdle = true;
        }

        if (enemyTransform != null)
        {
            Vector2 posAttack = new Vector2(enemyTransform.position.x, 
                                    enemyTransform.position.y);

            if (Vector2.Distance(transform.position, enemyTransform.position) <= 0.8f)
            {
                dwaft.AnimationAttack();
            }
            else
            {
                AnimationFollowEnemy(posAttack);
                transform.position = Vector2.MoveTowards(transform.position, posAttack, 
                    speedMove * Time.deltaTime);
            } 

            isAttack = true;
        }
        else if (enemyTransform == null && isIdle)
        {
            checkLockTarget = false;
            isAttack = false;
            dwaft.AnimationIdle();
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && !isAttack && !checkLockTarget && coll.gameObject.layer != 9)
        {
            checkLockTarget = true;
            enemyTransform = coll.gameObject.transform;
            dwaft.SetGameObjectToAttack(coll.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && checkLockTarget && coll.gameObject.layer != 9)
        {
            checkLockTarget = false;
            enemyTransform = null;
            targetPosition = gameObject.transform.position;
        }
    }

    private void AnimationFollowEnemy(Vector2 position)
    {
        float x1 = position.x;
        float y1 = position.y;
        float x0 = gameObject.transform.position.x;
        float y0 = gameObject.transform.position.y;

        float angle = GetRotateAngleGameObject(x0, y0, x1, y1);

        if (angle < ANGLE_SWAP_STATE)
        {
            if (y0 < y1)
            {
                dwaft.AnimationMoveBeHind();
            }
            else
            {
                dwaft.AnimationMoveBefore();
            }
        }
        else if (angle >= ANGLE_SWAP_STATE)
        {
            dwaft.AnimationMoveLR();

            if (transform.position.x < position.x)
            {
                Vector2 scale = transform.localScale;
                scale.x = -1f;
                transform.localScale = scale;
            }
            else
            {
                Vector2 scale = transform.localScale;
                scale.x = 1f;
                transform.localScale = scale;
            }
        }
    }

    private float GetRotateAngleGameObject(float x0, float y0, float x1, float y1)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x0 - x1) / Mathf.Abs(y0 - y1));
        return angle;
    }
}
