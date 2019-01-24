using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLightLevel3: MonoBehaviour
{

    private Transform target;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;

    public int damageOverTime = 30;
    public float slowAmount = .5f;
    public float radiusAttack = 3f;

    public string enemyTag = "Enemy";
    public float turnSpeed = 90f;
    public Transform[] firePoints;
    private Transform[] arrayTarget;
    private GameObject nextTarget;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = radiusAttack;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        Debug.Log("shortestDistance " + shortestDistance);
        if (nearestEnemy != null && shortestDistance <= radiusAttack)
        {
            target = nearestEnemy.transform;
            if (nearestEnemy.layer == 8)
            {
                nearestEnemy.GetComponentInChildren<Demon>().SubHealth(damageOverTime);
            }
            if (nearestEnemy.layer == 9)
            {
                nearestEnemy.GetComponentInChildren<Dragon>().SubHealth(damageOverTime);
            }
            if (nearestEnemy.layer == 14)
            {
                nearestEnemy.GetComponentInChildren<OskBane>().SubHealth(damageOverTime);
            }
            if (nearestEnemy.layer == 15)
            {
                nearestEnemy.GetComponentInChildren<IceDemon>().SubHealth(damageOverTime);
            }
            if (nearestEnemy.layer == 16)
            {
                nearestEnemy.GetComponentInChildren<IceDemonChild>().SubHealth(damageOverTime);
            }
            if (nearestEnemy.layer == 17)
            {
                nearestEnemy.GetComponentInChildren<Destroyer>().SubHealth(damageOverTime);
            }
        }
        else
        {
            target = null;
        }
    }

    public void FindNextEnemy(Transform target)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = radiusAttack;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform != target)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nextTarget = enemy;
                }
            }
        }
    }

    void Update()
    {
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Play();
                impactEffect.transform.position = transform.position;
            }
            return;
        }
        Lazer();
    }

    void Lazer()
    {
        if (!lineRenderer.enabled)
        { 
            lineRenderer.enabled = true;
            Debug.Log("lAYER" + lineRenderer.sortingOrder);
            impactEffect.Play();
        }
        FindNextEnemy(target);
        Vector3 dir = new Vector3(0, target.GetComponent<Renderer>().bounds.size.y * 1 / 3, 0);
        lineRenderer.SetPosition(0, firePoints[0].position);
        lineRenderer.SetPosition(1, firePoints[1].position);
        lineRenderer.SetPosition(2, target.position + dir);
        if (nextTarget != null)
        {
            Vector3 dis_y = new Vector3(0, nextTarget.GetComponent<Renderer>().bounds.size.y * 2 / 3, 0);
            lineRenderer.SetPosition(3, nextTarget.transform.position + dis_y);
        }
        else lineRenderer.SetPosition(3, target.position + dir);
        lineRenderer.SetPosition(4, firePoints[0].position);
        impactEffect.transform.position = target.position + dir;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}
