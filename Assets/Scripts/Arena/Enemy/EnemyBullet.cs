using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 0f;
    [SerializeField] private float timeToDisperse = 4f;
    [SerializeField] private float projectileSpeed = 3.5f;

    private void OnEnable()
    {
        Invoke("DisableObject", timeToDisperse);
        GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
        CancelInvoke();
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }

}
