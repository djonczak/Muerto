using UnityEngine;

namespace Game.Arena.AI 
{
    public class EnemyDashAttackCollider : MonoBehaviour
    {
        [SerializeField] private float damage;
        private AudioSource _audioSource;

        private const string PlayerTag = "Player";

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
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
                _audioSource.PlayOneShot(_audioSource.clip);
            }
        }
    }
}
