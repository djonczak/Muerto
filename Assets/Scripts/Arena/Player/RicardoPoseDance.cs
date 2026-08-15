using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Arena.AI;
using Game.Arena.Events;

namespace Game.Arena.Player
{
    public class RicardoPoseDance : PlayerAbility
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float abilityActiveTime;
        [SerializeField] private float pullForce = 15f;
        [SerializeField] private bool canUse = true;
        [SerializeField] private LayerMask enemyLayer = 11;
        [Header("Effect")]
        [SerializeField] private ParticleSystem[] vortexParticles;
        private Animator animator;
        private PlayerAttack playerAttack;
        private ArenaMovement arenaMovement;
        private PlayerHP playerHP;
        private MariachiGrenadeThrow mariachiGrenadeThrow;

        private Coroutine coroutine;
        private ISoundEffect iSoundEffect;

        private bool isDancing;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string SecondAbility = "SecondAbility";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerAttack = GetComponent<PlayerAttack>();
            iSoundEffect = GetComponent<ISoundEffect>();
            arenaMovement = GetComponent<ArenaMovement>();
            playerHP = GetComponent<PlayerHP>();
            mariachiGrenadeThrow = GetComponent<MariachiGrenadeThrow>();
        }

        private void Update()
        {
            if (Disabled == false)
            {
                if (CanUseAbility)
                {
                    if (Input.GetKeyDown(KeyCode.W) && canUse == true)
                    {
                        StartDance();
                    }
                }
            }
        }

        private void StartDance()
        {
            iSoundEffect.PlayAbility2Sound();
          //  mariachiGrenadeThrow.CanUseAbility = false;
            canUse = false;
            arenaMovement.enabled = false;
            playerAttack.enabled = false;
            animator.SetTrigger(SecondAbility);
            animator.SetBool(RunKey, false);
            animator.SetBool(IdleKey, false);
            ArenaEvents.SecondAbility(true);
            coroutine = StartCoroutine(AbilityActive());
        }

        private IEnumerator AbilityActive()
        {
            playerHP.canBeHurt = false;
            isDancing = true;
            vortexParticles[1].transform.position = transform.position;
            vortexParticles[0].loop = true;
            vortexParticles[1].loop = true;
            vortexParticles[0].Play();
            vortexParticles[1].Play();
            var timer = 0f;
            while (timer < abilityActiveTime)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
                foreach (Collider2D enemy in enemies)
                {
                    var enemyEntity = enemy.GetComponent<EnemyTagger>();
                    if (enemyEntity != null)
                    {
                        var iDamage = enemy.GetComponent<IDamage>();
                        if (iDamage != null)
                        {
                            iDamage.DeathOverTime(2f);
                        }

                        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            Vector2 direction = ((Vector2)transform.position - rb.position).normalized;
                            rb.AddForce(direction * pullForce * Time.deltaTime, ForceMode2D.Force);
                        }
                    }
                }
                timer += Time.deltaTime;
                playerHP.canBeHurt = false;
                yield return null;
            }
            isDancing = false;
            vortexParticles[0].loop = false;
            vortexParticles[1].loop = false;
            playerHP.canBeHurt = true;
            arenaMovement.enabled = true;
            playerAttack.enabled = true;
         //   mariachiGrenadeThrow.CanUseAbility = true;
            StartCoroutine(AbilityCooldownTimer());
        }

        public override void CancelAbility()
        {
            if (isDancing)
            {
                ArenaEvents.SecondAbility(false);
                StopCoroutine(coroutine);
                vortexParticles[0].loop = false;
                vortexParticles[1].loop = false;
                iSoundEffect.StopAbility2Sound();
                playerHP.canBeHurt = true;
                isDancing = false;
                StartCoroutine(AbilityCooldownTimer());
            }
        }

        private IEnumerator AbilityCooldownTimer()
        {
            Game.UI.PlayerUI.instance.Used2Ability();
            yield return new WaitForSeconds(AbilityCooldown);
            canUse = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
