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

        private Animator _animator;
        private bool _canUseAbility = true;
        private AudioSource _audioSource;
        private BossMovement _bossMove;

        private const string AbilityKey = "Ability";
        private const string BossBulletKey = "BossBullet";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _bossMove = GetComponent<BossMovement>();
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
            if (_canUseAbility == true)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > activateRadius)
                {
                    _bossMove.canMove = false;
                    _canUseAbility = false;
                    _animator.SetTrigger(AbilityKey);
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
            _audioSource.PlayOneShot(abilitySound);
        }

        public void EndAbility()
        {
            _canUseAbility = true;
            _bossMove.canMove = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, activateRadius);
        }
    }
}
