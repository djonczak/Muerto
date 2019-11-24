using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttackCollider : MonoBehaviour
{
    AudioSource source;

	void Start ()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bat" || collision.tag == "Chupacabra")
        {
            collision.GetComponent<IDamage>().TakeDamage(1, DamageType.Normal);
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
    }
}
