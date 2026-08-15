using System.Collections;
using UnityEngine;

namespace Game.Arena.AI {

    public class BossBullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float bulletDuration = 10f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private ParticleSystem trailParticle;

        public GameObject target;
        private bool canFollow = false;

        private Animator anim;
        private SpriteRenderer sprite;
        private AudioSource audioSource;

        Coroutine coroutine;

        private const string Disperse = "Disperse";
        private const string Hit = "Hit";
        private const string Player = "Player";

        private void OnEnable()
        {
            canFollow = true;
            audioSource.Play();
            coroutine = StartCoroutine(DisperseCooldown(bulletDuration));
            trailParticle.loop = true;
            trailParticle.Play();
        }

        private void Awake()
        {
            anim = GetComponent<Animator>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            trailParticle = GetComponentInChildren<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (canFollow == true && target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

                RotateTowardsPlayer();
            }
        }

        private void RotateTowardsPlayer()
        {
            if (target.transform.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }

        private IEnumerator DisperseCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            coroutine = null;
            canFollow = false;
            anim.SetTrigger(Disperse);
            trailParticle.loop = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Player)
            {
                var iDamage = collision.GetComponent<IDamage>();
                if (iDamage != null)
                {
                    iDamage.TakeDamage(damage, DamageType.Normal);
                    anim.SetTrigger(Hit);
                    trailParticle.loop = false;
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                        coroutine = null;
                    }
                    canFollow = false;
                }
            }
        }

        private void OnDisable()
        {
            canFollow = false;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
