using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletArrow : MonoBehaviour
{
    public int damage = 5;
    public float lifeTime = 0.5f;
    public float speed = 3f;
    public float speedUpOverTime = 0.5f;
    public float hitDistance = 1f;
    public float ballisticOffset = 1F;
    public bool freezeRotation = false;

    private Vector3 originPoint;
    public GameObject target;
    private Vector3 aimPoint;
    private Vector3 myVirtualPosition;
    private Vector3 myPreviousPosition;
    private float counter;
    private SpriteRenderer sprite;

    private Demon demon;
    private Dragon dragon;

    void Start()
    {
        Debug.Log("fIRE");
        //sprite = GetComponent<SpriteRenderer>();
        //sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        //this.target = target;
        Destroy(gameObject, lifeTime);
    }

    void Update ()
    {
  
        if (target != null)
        {
            aimPoint = target.transform.position;
        }
        Vector3 originDistance = aimPoint - originPoint;
        Vector3 distanceToAim = aimPoint - (Vector3)myVirtualPosition;
        myVirtualPosition = Vector3.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        LookAtDirection2D((Vector3)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;
        if (distanceToAim.magnitude <= hitDistance)
        {
            if (target != null)
            {
                if (target.layer == 8)
                {
                    Debug.Log("DEMON_ATTACK");
                    demon = target.gameObject.GetComponentInChildren<Demon>();
                    demon.SubHealth(damage);
                    Destroy(gameObject);
                }

                if (target.layer == 9)
                {
                    Debug.Log("DRAGON_ATTACK");
                    dragon = target.GetComponentInChildren<Dragon>();
                    dragon.SubHealth(damage);
                    Destroy(gameObject);
                }
                Debug.Log("Target!=null");
            }

            Destroy(gameObject);
        }
    }

    private void LookAtDirection2D(Vector3 direction)
    {
        Debug.Log("LOOKATDIRECTION2D");
        if (freezeRotation == false)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private Vector3 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
            Debug.Log("AddBallisticOffset-ballisticOffset>0");
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            return (Vector3)myVirtualPosition + (ballisticOffset * offset * Vector3.up);
        }
        else
        {
            return myVirtualPosition;
        }
    }

}
