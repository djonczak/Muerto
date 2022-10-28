using UnityEngine;

namespace Game.Arena.AI 
{
    public class BossMeleeAttack : MonoBehaviour
    {
        [Header("Melee Attack Settings")]
        public Transform attackAreaPoint;

        [SerializeField] private float attackDamage = 1f;
        [SerializeField] private float attackSpeed = 0.5f;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private GameObject target;
        [SerializeField] private LayerMask playerLayer = 10;
        [SerializeField] private Animator attackWaveEffect;

        public AudioClip attackSound;

        private Animator _animator;
        private AudioSource _audioSource;
        private float _timer;
        private bool _isAttacking;

        private const string AttackKey = "Attack";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            attackWaveEffect = transform.GetChild(1).gameObject.GetComponent<Animator>();
        }

        private void Start()
        {
            target = PlayerObject.GetPlayerObject();
        }

        private void Update()
        {
            CheckAttack();
        }

        private void CheckAttack()
        {
            if (target.GetComponent<Player.PlayerHP>().isAlive == true && _isAttacking == false)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= attackRange)
                {
                    _timer += Time.deltaTime;
                    if (_timer >= attackSpeed)
                    {
                        StartAttack();
                    }
                }
                else
                {
                    _timer = 0f;
                }
            }
        }

        private void StartAttack()
        {
            _animator.SetTrigger(AttackKey);
            GetComponent<BossMovement>().canMove = false;
            _isAttacking = true;
            _timer = 0f;
        }

        private void ShowEffect()
        {
            attackWaveEffect.gameObject.SetActive(true);
        }

        public void EndAttack()
        {
            _isAttacking = false;
            GetComponent<BossMovement>().canMove = true;
        }

        public void CastAttack()
        {
            _audioSource.PlayOneShot(attackSound);
            Collider2D player = Physics2D.OverlapCircle(attackAreaPoint.position, attackRange, playerLayer);
            if (player != null)
            {
                if (player.GetComponent<IDamage>() != null)
                {
                    player.GetComponent<IDamage>().TakeDamage(attackDamage, DamageType.Normal);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackAreaPoint.position, attackRange);
        }
    }
}
