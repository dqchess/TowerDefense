using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOfDeathSkill : MonoBehaviour
{

    public GameObject effecf;
    public GameObject positionStartEffect;

    private float speed_x;
    private float speed_y;
    private float angle;
    private float damage;
    private float timeStart;
    private float widthOfGOB;
    private const float TIME_OUTLAST = 4f;

    void Awake()
    {
        timeStart = Time.time;
        widthOfGOB = gameObject.GetComponent<Renderer>().bounds.size.x;
    }

    void Start()
    {
        speed_x = 6f;
        float radian = angle * (Mathf.PI / 180);
        speed_y = (float)(speed_x * Mathf.Tan(radian));
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + speed_x * Time.deltaTime, 
            transform.position.y + speed_y * Time.deltaTime, 1);

        if (Time.time - timeStart >= TIME_OUTLAST)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Hero"))
        {
            if (coll.gameObject.layer == 10)
                coll.gameObject.GetComponentInChildren<EarthShaker>().SubHealth(damage);
            else if (coll.gameObject.layer == 12)
                coll.gameObject.GetComponentInChildren<NagaSiren>().SubHealth(damage);

            Instantiate(effecf, positionStartEffect.transform.position, 
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        }
        else if (coll.gameObject.CompareTag("Army"))
        {
            if (coll.gameObject.layer == 11)
                coll.gameObject.GetComponentInChildren<Dwarf>().SubHealth(damage);

            Instantiate(effecf, positionStartEffect.transform.position, 
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetAngleStart(float angle)
    {
        this.angle = angle;
    }
}
