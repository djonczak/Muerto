using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player 
{
    public class MariachiGrenade : MonoBehaviour
    {
        [SerializeField] private float explosionRange;
        [SerializeField] private Transform throwPoint;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask enemyLayer = 11;
        [SerializeField] private Transform grenade;

        private ParticleSystem explosionParticle;
        private AudioSource explosionSound;

        private void Awake()
        {
            explosionSound = GetComponent<AudioSource>();
            explosionParticle = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            grenade.localScale = Vector3.zero;
        }

        public void ThrowGrenade(Vector3 throwPoint)
        {
            StartCoroutine(ThrowGrenadeCalculation(throwPoint));
            StartCoroutine(AppearGrenade());
        }

        IEnumerator ThrowGrenadeCalculation(Vector3 targetPosition)
        {
            float timer = 0f;
            Vector3 startPos = throwPoint.position;
            float archHeight = Vector3.Distance(startPos, targetPosition) / 2f;
            float distance = Vector3.Distance(startPos, targetPosition);
            var duration = distance / speed;
            transform.position = throwPoint.position;
            while (timer < duration)
            {
                var t = timer / duration;
                Vector3 position = Vector3.Lerp(startPos, targetPosition, t);
                position.y += archHeight * (4f * t * (1f - t));
                transform.position = position;
                timer += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            grenade.localScale = Vector3.zero;
            explosionSound.Play();
            explosionParticle.Play();
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRange, enemyLayer);
            if (enemies != null)
            {
                foreach (Collider2D enemy in enemies)
                {
                    var iDamage = enemy.GetComponent<IDamage>();
                    if (iDamage != null)
                    {
                        iDamage.TakeDamage(1, DamageType.Normal);
                    }
                }
            }
        }

        private IEnumerator AppearGrenade()
        {
            float timer = 0f;
            Vector3 startScale = grenade.localScale;
            while (timer < 1F)
            {
                var value = Mathf.Lerp(startScale.x, 1f, timer / 1f);
                grenade.localScale = new Vector3(value, value, value);
                timer += Time.deltaTime;
                yield return null;
            }
            grenade.localScale = new Vector3(1,1,1);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }
    }
}
