using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player
{

    public class TableChargeAbility : MonoBehaviour
    {
        public bool disable = false;
        public float abilityCooldown = 15f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private float abilityRange = 1f;
        [SerializeField] private float abilityDuration = 5f;
        [SerializeField] private float chargeSpeed = 10f;
        [SerializeField] private bool canUse = true;
        [SerializeField] private bool hasCharged = false;
        public bool isCharging = false;
        [SerializeField] private LayerMask enemyLayer = 11;
        private Animator _animator;
        private Rigidbody2D _rigidbody;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string ChargeKey = "Charge";
        private const string ChargeIdleKey = "ChargeIdle";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (disable == false)
            {
                Input();

                Charge();
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
            _rigidbody.velocity = Vector3Extension.CalculateDirectionTowardsMouse(transform.position) * chargeSpeed;
            StartCoroutine(ChargeDuration(abilityDuration));
            _animator.SetBool(ChargeKey, true);
            isCharging = true;
            ArenaEvents.PlayerCharge();
            hasCharged = false;
        }

        private void PrepareForCharge()
        {
            canUse = false;
            GetComponent<ArenaMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<DivingElbowAbility>().enabled = false;
            _animator.SetTrigger(ChargeIdleKey);
            _animator.SetBool(RunKey, false);
            _animator.SetBool(IdleKey, false);
            hasCharged = true;
            GetComponent<PlayerHP>().canBeHurt = false;
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
                        enemy.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
                        GetComponent<ISoundEffect>().PlayAbility2Sound();
                    }
                }

                ChargeRotation();
            }
        }

        private void ChargeRotation()
        {
            if (_rigidbody.velocity.x > 0.1f)
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
                _rigidbody.velocity = Vector3Extension.CalculateDirectionTowardsMouse(transform.position);
            }
        }

        IEnumerator ChargeDuration(float time)
        {
            yield return new WaitForSeconds(time);
            GetComponent<PlayerHP>().canBeHurt = true;
            _animator.SetBool(ChargeKey, false);
            isCharging = false;
            ArenaEvents.PlayerCharge();
            _rigidbody.velocity = Vector2.zero;
            GetComponent<ArenaMovement>().enabled = true;
            GetComponent<PlayerAttack>().enabled = true;
            GetComponent<DivingElbowAbility>().enabled = true;
            StartCoroutine(AbilityCooldown(abilityCooldown));
        }

        IEnumerator AbilityCooldown(float time)
        {
            Game.UI.PlayerUI.instance.Used2Ability();
            yield return new WaitForSeconds(time);
            canUse = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, abilityRange);
        }
    }
}
