using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.AI 
{
    public class EnemyBullet : MonoBehaviour
    {
        public float damage = 0f;
        [SerializeField] private float timeToDisperse = 4f;
        [SerializeField] private float projectileSpeed = 3.5f;
        [SerializeField] private ParticleSystem trailParticle;

        private Coroutine coroutine;

        private const string Player = "Player";

        private void OnEnable()
        {
            coroutine = StartCoroutine(DisappearBullet());
            GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
            trailParticle.loop = true;
            trailParticle.Play();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Player)
            {
                collision.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
                trailParticle.loop = false;
                gameObject.SetActive(false);
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
        }

        private IEnumerator DisappearBullet()
        {
            yield return new WaitForSeconds(timeToDisperse);
            trailParticle.loop = false;
            gameObject.SetActive(false);
            coroutine = null;
        }

        private void OnDisable()
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
