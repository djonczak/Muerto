using UnityEngine;

namespace Game.Arena.AI 
{
    public class BossFirstAbility : MonoBehaviour
    {
        [Header("Range Attack Options")]
        [SerializeField] private float activateRadius = 1.76f;
        [SerializeField] private int amountBulletsToSpawn = 1;
        [SerializeField] private GameObject target;

        public AudioClip abilitySound;

        private Animator animator;
        private bool canUseAbility = true;
        private AudioSource audioSource;
        private BossMovement bossMove;

        private const string AbilityKey = "Ability";
        private const string BossBulletKey = "BossBullet";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            bossMove = GetComponent<BossMovement>();
        }

        private void Start()
        {
            target = PlayerObject.GetPlayerObject();
        }

        private void Update()
        {
            CheckAbility();
        }

        private void CheckAbility()
        {
            if (canUseAbility == true)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > activateRadius)
                {
                    bossMove.canMove = false;
                    canUseAbility = false;
                    animator.SetTrigger(AbilityKey);
                }
            }
        }

        public void ShootProjectile()
        {
            for (int i = 0; i < amountBulletsToSpawn; i++)
            {
                GameObject projectile = Pooler.ObjectPooler.instance.GetPooledObject(BossBulletKey);
                if (projectile != null)
                {
                    var posX = Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
                    var posY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
                    projectile.transform.position = new Vector2(posX, posY);
                    projectile.SetActive(true);
                    projectile.GetComponent<BossBullet>().target = target;
                }
            }
            audioSource.PlayOneShot(abilitySound);
        }

        public void EndAbility()
        {
            canUseAbility = true;
            bossMove.canMove = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, activateRadius);
        }
    }
}
