using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : MonoBehaviour
{
    AudioSource source;

	void Start ()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<IDamage>().TakeDamage(1, DamageType.Normal);
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
    }
}
