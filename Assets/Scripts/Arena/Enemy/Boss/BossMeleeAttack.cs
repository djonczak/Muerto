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
        private Animator attackWaveEffect;

        public AudioClip attackSound;

        private Animator animator;
        private AudioSource audioSource;
        private BossMovement bossMovement;
        private Player.PlayerHP playerHP;

        private float timer;
        private bool isAttacking;

        private const string AttackKey = "Attack";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            attackWaveEffect = transform.GetChild(1).gameObject.GetComponent<Animator>();
            bossMovement = GetComponent<BossMovement>();
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
            if (playerHP.isAlive == true && isAttacking == false)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= attackRange)
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

        private void StartAttack()
        {
            animator.SetTrigger(AttackKey);
            bossMovement.canMove = false;
            isAttacking = true;
            timer = 0f;
        }

        public void ShowEffect()
        {
            attackWaveEffect.gameObject.SetActive(true);
        }

        public void EndAttack()
        {
            isAttacking = false;
            bossMovement.canMove = true;
        }

        public void CastAttack()
        {
            audioSource.PlayOneShot(attackSound);
            Collider2D player = Physics2D.OverlapCircle(attackAreaPoint.position, attackRange, playerLayer);
            if (player != null)
            {
                var iDamage = player.GetComponent<IDamage>();
                if (iDamage != null)
                {
                    iDamage.TakeDamage(attackDamage, DamageType.Normal);
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
