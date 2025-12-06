using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player
{
    public class TableChargeAbility : PlayerAbility
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private float abilityRange = 1f;
        [SerializeField] private float abilityDuration = 5f;
        [SerializeField] private float chargeSpeed = 10f;
        [SerializeField] private bool canUse = true;
        [SerializeField] private bool hasCharged = false;
        public bool isCharging = false;
        [SerializeField] private LayerMask enemyLayer = 11;

        private Animator animator;
        private Rigidbody2D rigidbody2D;
        private PlayerAttack playerAttack;
        private DivingElbowAbility divineElbowAbility;
        private ArenaMovement arenaMovement;
        private PlayerHP playerHP;

        private ISoundEffect iSoundEffect;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string ChargeKey = "Charge";
        private const string ChargeIdleKey = "ChargeIdle";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            playerAttack = GetComponent<PlayerAttack>();
            iSoundEffect = GetComponent<ISoundEffect>();
            divineElbowAbility = GetComponent<DivingElbowAbility>();
            arenaMovement = GetComponent<ArenaMovement>();
            playerHP = GetComponent<PlayerHP>();
        }

        private void Update()
        {
            if (Disabled == false)
            {
                if (CanUseAbility)
                {
                    Input();

                    Charge();
                }
            }
        }

        private void Input()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.W) && canUse == true)
            {
                PrepareForCharge();
            }

            if (hasCharged == true)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartCharge();
                }
            }
        }

        private void StartCharge()
        {
            Time.timeScale = 1f;
            rigidbody2D.velocity = Vector3Extension.CalculateDirectionTowardsMouse(transform.position) * chargeSpeed;
            StartCoroutine(ChargeDuration(abilityDuration));
            animator.SetBool(ChargeKey, true);
            isCharging = true;
            ArenaEvents.PlayerCharge();
            hasCharged = false;
        }

        private void PrepareForCharge()
        {
            canUse = false;
            arenaMovement.enabled = false;
            playerAttack.enabled = false;
            divineElbowAbility.CanUseAbility = false;
            animator.SetTrigger(ChargeIdleKey);
            animator.SetBool(RunKey, false);
            animator.SetBool(IdleKey, false);
            hasCharged = true;
            playerHP.canBeHurt = false;
            Time.timeScale = 0.5f;
        }

        private void Charge()
        {
            if (isCharging == true)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, abilityRange, enemyLayer);
                if (enemies != null)
                {
                    foreach (Collider2D enemy in enemies)
                    {
                        var iDamage = enemy.GetComponent<IDamage>();
                        if (iDamage != null)
                        {
                            iDamage.TakeDamage(damage, DamageType.Normal);
                            iSoundEffect.PlayAbility2Sound();
                        }
                    }
                }

                ChargeRotation();
            }
        }

        private void ChargeRotation()
        {
            if (rigidbody2D.velocity.x > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -1);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 8)
            {
                rigidbody2D.velocity = Vector2.zero;
                var direction = Vector3Extension.CalculateDirectionTowardsMouse(transform.position);
                rigidbody2D.velocity = direction * chargeSpeed;
            }
        }

        IEnumerator ChargeDuration(float time)
        {
            yield return new WaitForSeconds(time);
            playerHP.canBeHurt = true;
            animator.SetBool(ChargeKey, false);
            isCharging = false;
            ArenaEvents.PlayerCharge();
            rigidbody2D.velocity = Vector2.zero;
            arenaMovement.enabled = true;
            playerAttack.enabled = true;
            divineElbowAbility.CanUseAbility = true;
            StartCoroutine(AbilityCooldownTimer());
        }

        public void CancelAbility()
        {
            if (isCharging)
            {
                StopAllCoroutines();
                playerHP.canBeHurt = true;
                animator.SetBool(ChargeKey, false);
                isCharging = false;
                ArenaEvents.PlayerCharge();
                rigidbody2D.velocity = Vector2.zero;
                arenaMovement.enabled = true;
                playerAttack.enabled = true;
                divineElbowAbility.CanUseAbility = true;
                StartCoroutine(AbilityCooldownTimer());
            }
        }

        IEnumerator AbilityCooldownTimer()
        {
            Game.UI.PlayerUI.instance.Used2Ability();
            yield return new WaitForSeconds(AbilityCooldown);
            canUse = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, abilityRange);
        }
    }
}
