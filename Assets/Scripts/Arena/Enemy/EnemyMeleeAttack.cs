using UnityEngine;

namespace Game.Arena.AI
{
    public class EnemyMeleeAttack : MonoBehaviour, IReset
    {
        [Header("Melee Attack Settings")]
        [SerializeField] private float attackDamage = 1f;
        [SerializeField] private float attackSpeed = 0.5f;
        [SerializeField] private float attackRadius = 0.5f;
        [SerializeField] private GameObject target;
        [SerializeField] private LayerMask playerLayer = 10;

        private Animator animator;
        private AudioSource audioSource;
        private EnemyMovement enemyMovement;
        private Player.PlayerHP playerHP;

        private float timer;
        private bool isAttacking;
        public bool canAttack = true;

        private const string RunKey = "Run";
        private const string AttackKey = "Attack";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            enemyMovement = GetComponent<EnemyMovement>();
        }

        private void Start()
        {
            target = PlayerObject.GetPlayerObject();
            playerHP = target.GetComponent<Player.PlayerHP>();
        }


        private void Update()
        {
            CheckAttack();
        }

        private void CheckAttack()
        {
            if (canAttack)
            {
                if (playerHP.isAlive == true && isAttacking == false)
                {
                    var distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= attackRadius)
                    {
                        timer += Time.deltaTime;
                        if (timer >= attackSpeed)
                        {
                            StartAttack();
                        }
                    }
                    else
                    {
                        timer = 0f;
                    }
                }
            }
        }

        private void StartAttack()
        {
            animator.SetTrigger(AttackKey);
            enemyMovement.canMove = false;
            isAttacking = true;
            timer = 0f;
        }

        public void EndAttack()
        {
            animator.SetBool(RunKey, true);
            isAttacking = false;
            enemyMovement.canMove = true;
        }

        public void CastAttack()
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
            if (player != null)
            {
                if (player.GetComponent<IDamage>() != null)
                {
                    audioSource.PlayOneShot(audioSource.clip);
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
            timer = 0f;
            enemyMovement.canMove = true;
            animator.SetBool(RunKey, true);
            isAttacking = false;
            canAttack = true;
        }
    }
}
