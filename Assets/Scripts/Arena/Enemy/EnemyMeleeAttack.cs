using UnityEngine;

namespace Game.Arena.Player {

    public class EnemyMeleeAttack : MonoBehaviour, IReset
    {
        [Header("Melee Attack Settings")]
        [SerializeField] private float attackDamage = 1f;
        [SerializeField] private float attackSpeed = 0.5f;
        [SerializeField] private float attackRadius = 0.5f;
        [SerializeField] private GameObject target;
        [SerializeField] private LayerMask playerLayer = 10;

        private Animator _animator;
        private AudioSource _audioSource;
        private float _timer;
        private bool _isAttacking;

        private const string RunKey = "Run";
        private const string AttackKey = "Attack";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
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
                if (distance <= attackRadius)
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
            GetComponent<EnemyMovement>().canMove = false;
            _isAttacking = true;
            _timer = 0f;
        }

        public void EndAttack()
        {
            _animator.SetBool(RunKey, true);
            _isAttacking = false;
            GetComponent<EnemyMovement>().canMove = true;
        }

        public void CastAttack()
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
            if (player != null)
            {
                if (player.GetComponent<IDamage>() != null)
                {
                    _audioSource.PlayOneShot(_audioSource.clip);
                    player.GetComponent<IDamage>().TakeDamage(attackDamage, DamageType.Normal);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }

        public void OnDeathReset()
        {
            _timer = 0f;
            GetComponent<EnemyMovement>().canMove = true;
            _animator.SetBool(RunKey, true);
            _isAttacking = false;
        }
    }
}
