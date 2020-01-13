using UnityEngine;

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

    private Animator anim;
    private AudioSource sound;
    private float timer;
    private bool isAttacking;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
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
        if (target.GetComponent<PlayerHP>().isAlive == true && isAttacking == false)
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
        anim.SetTrigger("Attack");
        GetComponent<BossMovement>().canMove = false;
        isAttacking = true;
        timer = 0f;
    }

    private void ShowEffect()
    {
        attackWaveEffect.gameObject.SetActive(true);
    }

    public void EndAttack()
    {
        isAttacking = false;
        GetComponent<BossMovement>().canMove = true;
    }

    public void CastAttack()
    {
        sound.PlayOneShot(attackSound);
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
