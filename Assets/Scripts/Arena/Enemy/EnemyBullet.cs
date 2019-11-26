using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 0f;
    [SerializeField]
    private float timeToDisperse = 4f;

    private void OnEnable()
    {
        Invoke("DisableObject", timeToDisperse);
        GetComponent<Rigidbody2D>().velocity = transform.right * 2f;
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
