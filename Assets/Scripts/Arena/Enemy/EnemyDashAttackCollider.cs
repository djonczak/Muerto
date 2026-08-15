using UnityEngine;

namespace Game.Arena.AI 
{
    public class EnemyDashAttackCollider : MonoBehaviour
    {
        [SerializeField] private float damage;
        private AudioSource audioSource;

        private const string PlayerTag = "Player";

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            damage = GetComponentInParent<EnemyDashAttack>().attackDamage;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == PlayerTag)
            {
                collision.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }
}
