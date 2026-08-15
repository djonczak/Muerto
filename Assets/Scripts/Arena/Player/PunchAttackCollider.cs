using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Arena.Extensions;

namespace Game 
{
    public class PunchAttackCollider : MonoBehaviour
    {
        private AudioSource source;
        [SerializeField] private LayerMask enemyLayer;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameObjectExtension.CompareLayerMask(collision.gameObject.gameObject, enemyLayer))
            {
                var iDamage = collision.GetComponent<IDamage>();
                if (iDamage != null)
                {
                    iDamage.TakeDamage(1, DamageType.Normal);
                    if (!source.isPlaying)
                    {
                        source.Play();
                    }
                }
            }
        }
    }
}
