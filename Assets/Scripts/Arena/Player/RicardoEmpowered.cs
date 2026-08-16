using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Arena.AI;
using Game.Arena.Events;
namespace Game.Arena.Player 
{
    public class RicardoEmpowered : PlayerAbility
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float abilityActiveTime;
        [SerializeField] private bool canUse = true;
        [SerializeField] private LayerMask enemyLayer = 11;
        [Header("Effect")]
        [SerializeField] private ParticleSystem empoweredAbilityParticle;
        private Animator animator;
        private PlayerAttack playerAttack;
        private ArenaMovement arenaMovement;
        private PlayerHP playerHP;
        private RicardoPoseDance secondAbility;

        private Coroutine coroutine;
        private ISoundEffect iSoundEffect;

        private bool isEmpowered;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string FirstAbility = "FirstAbility";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerAttack = GetComponent<PlayerAttack>();
            iSoundEffect = GetComponent<ISoundEffect>();
            arenaMovement = GetComponent<ArenaMovement>();
            playerHP = GetComponent<PlayerHP>();
            secondAbility = GetComponent<RicardoPoseDance>();
        }

        private void Update()
        {
            if (Disabled == false)
            {
                if (CanUseAbility)
                {
                    if (Input.GetKeyDown(KeyCode.Q) && canUse == true)
                    {
                        StartDance();
                    }
                }
            }
        }

        private void StartDance()
        {
            iSoundEffect.PlayAbility1Sound();
            secondAbility.CanUseAbility = false;
            canUse = false;
            arenaMovement.enabled = false;
            playerAttack.enabled = false;
            animator.SetBool(RunKey, false);
            animator.SetBool(IdleKey, false);
            ArenaEvents.FirstAbility(true);
            coroutine = StartCoroutine(AbilityActive());
        }

        private IEnumerator AbilityActive()
        {
            animator.SetTrigger(FirstAbility);
            playerHP.canBeHurt = false;
            isEmpowered = true;
            empoweredAbilityParticle.loop = true;
            empoweredAbilityParticle.Play();
            yield return new WaitForSeconds(2.4f);
            arenaMovement.enabled = true;
            playerAttack.enabled = true;
            var timer = 0f;
            while (timer < abilityActiveTime)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
                foreach (Collider2D enemy in enemies)
                {
                    var iDamage = enemy.GetComponent<IDamage>();
                    if (iDamage != null)
                    {
                        iDamage.TakeDamage(1f, DamageType.Normal);
                    }
                }
                timer += Time.deltaTime;
                playerHP.canBeHurt = false;
                yield return null;
            }
            isEmpowered = false;
            empoweredAbilityParticle.loop = false;
            playerHP.canBeHurt = true;
            secondAbility.CanUseAbility = true;
            StartCoroutine(AbilityCooldownTimer());
        }

        public override void CancelAbility()
        {
            if (isEmpowered)
            {
                ArenaEvents.FirstAbility(false);
                StopCoroutine(coroutine);
                empoweredAbilityParticle.loop = false;
                iSoundEffect.StopAbility1Sound();
                playerHP.canBeHurt = true;
                isEmpowered = false;
                StartCoroutine(AbilityCooldownTimer());
            }
        }

        private IEnumerator AbilityCooldownTimer()
        {
            Game.UI.PlayerUI.instance.Used1Ability();
            yield return new WaitForSeconds(AbilityCooldown);
            canUse = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}