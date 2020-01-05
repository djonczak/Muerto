using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPillar : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    public void UsePillar(Transform target)
    {
        transform.position = target.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
        }
    }
}
