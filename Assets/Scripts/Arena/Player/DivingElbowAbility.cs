using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player
{
    public class DivingElbowAbility : PlayerAbility
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private float abilityRange = 5f;
        [SerializeField] private float fallSpeed = 10f;
        [SerializeField] private bool canUse = true;
        [SerializeField] private bool preparedToJump = false;
        [SerializeField] private bool isFalling = false;
        [SerializeField] private LayerMask enemyLayer = 11;

        private Animator animator;
        private PlayerAttack playerAttack;
        private TableChargeAbility tableChargeAbility;
        private ArenaMovement arenaMovement;
        private PolygonCollider2D polygonCollider2D;
        private PlayerHP playerHP;

        private ISoundEffect iSoundEffect;

        private const string RunKey = "Run";
        private const string IdleKey = "Idle";
        private const string FallAttackKey = "FallAttack";
        private const string DustWaveKey = "DustWave";

        private Vector3 targetPosition;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerAttack = GetComponent<PlayerAttack>();
            iSoundEffect = GetComponent<ISoundEffect>();
            tableChargeAbility = GetComponent<TableChargeAbility>();
            arenaMovement = GetComponent<ArenaMovement>();
            polygonCollider2D = GetComponent<PolygonCollider2D>();
            playerHP = GetComponent<PlayerHP>();
        }

        public void Update()
        {
            if (Disabled == false)
            {
                if (CanUseAbility)
                {
                    Input();

                    FallDawn();
                }
            }
        }

        private void Input()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q) && canUse == true)
            {
                PrepareToJump();
            }

            if (preparedToJump == true)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Jump();
                }
            }
        }

        private void PrepareToJump()
        {
            canUse = false;
            preparedToJump = true;
            Time.timeScale = 0.5f;
            playerAttack.enabled = false;
            polygonCollider2D.isTrigger = true;
            tableChargeAbility.CanUseAbility = false;
            playerHP.canBeHurt = false;
        }

        private void Jump()
        {
            Game.UI.PlayerUI.instance.Used1Ability();
            transform.position = new Vector3(Vector3Extension.MousePosition().x, transform.position.y + 1.7f, transform.position.z);
            Time.timeScale = 1f;
            arenaMovement.enabled = false;
            targetPosition = Vector3Extension.MousePosition();
            isFalling = true;
            animator.SetBool(FallAttackKey, true);
            animator.SetBool(RunKey, false);
            animator.SetBool(IdleKey, false);
            preparedToJump = false;
        }

        private void FallDawn()
        {
            if (isFalling == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
                if (0.01f > Vector3Extension.DistanceBetweenPlayerMouse(transform.position, targetPosition))
                {
                    PoundAttack();
                }
            }
        }

        private void PoundAttack()
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, abilityRange, enemyLayer);
            foreach (Collider2D enemy in enemies)
            {
                var iDamage = enemy.GetComponent<IDamage>();
                if (iDamage != null)
                {
                    iDamage.TakeDamage(damage, DamageType.Normal);
                }
            }
            ParticleEffect();
            isFalling = false;
            iSoundEffect.PlayAbility1Sound();
            animator.SetBool(FallAttackKey, false);
            StartCoroutine(AbilityCooldownTimer());
        }

        private IEnumerator AbilityCooldownTimer()
        {
            tableChargeAbility.CanUseAbility = false;
            polygonCollider2D.isTrigger = false;
            playerAttack.enabled = true;
            arenaMovement.enabled = true;
            yield return new WaitForSeconds(1.5f);
            playerHP.canBeHurt = true;
            yield return new WaitForSeconds(AbilityCooldown - 1.5f);
            canUse = true;
        }

        private void ParticleEffect()
        {
            GameObject effect = Pooler.ObjectPooler.instance.GetPooledObject(DustWaveKey);
            if (effect != null)
            {
                effect.transform.position = transform.position;
                effect.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, abilityRange);
        }
    }
}
