using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player
{
    public class MariachiTrumpetAbility : PlayerAbility
    {
        [SerializeField] private float minAttackRange;
        [SerializeField] private float maxAttackRange;
        [SerializeField] private float abilityActiveTime;
        [SerializeField] private bool canUse = true;
        [SerializeField] private LayerMask enemyLayer = 11;
        [Header("Effect")]
        [SerializeField] private ParticleSystem[] notesParticles;
        [SerializeField] private Animator notesAnimator;
        [SerializeField] private Transform notesPoint;
        private Animator animator;
        private PlayerAttack playerAttack;
        private ArenaMovement arenaMovement;
        private PlayerHP playerHP;
        private MariachiGrenadeThrow mariachiGrenadeThrow;

        private Coroutine coroutine;
        private ISoundEffect iSoundEffect;

        private bool isSinging;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string SecondAbility = "SecondAbility";
        private const string Spread = "Spread";

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
                        PlayTrumpet();
                    }
                }
            }
        }

        private void PlayTrumpet()
        {
            iSoundEffect.PlayAbility2SoundLoop();
            mariachiGrenadeThrow.CanUseAbility = false;
            canUse = false;
            arenaMovement.enabled = false;
            playerAttack.enabled = false;
            animator.SetTrigger(SecondAbility);
            animator.SetBool(RunKey, false);
            animator.SetBool(IdleKey, false);
            playerHP.canBeHurt = false;
            ArenaEvents.SecondAbility(true);
            coroutine = StartCoroutine(AbilityActive());
        }

        private IEnumerator AbilityActive()
        {
            playerHP.canBeHurt = false; 
            isSinging = true;
            notesAnimator.SetTrigger(Spread);
            notesParticles[1].loop = true;
            notesParticles[2].loop = true;

            notesParticles[0].transform.position = notesPoint.position;
            notesParticles[0].Play();
            var timer = 0f;
            var startValue = minAttackRange;
            while (timer < abilityActiveTime)
            {
                var value = Mathf.Lerp(startValue, maxAttackRange, timer / abilityActiveTime);
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, value, enemyLayer);
                foreach (Collider2D enemy in enemies)
                {
                    var iDamage = enemy.GetComponent<IDamage>();
                    if (iDamage != null)
                    {
                        iDamage.TakeDamage(1f, DamageType.Normal);
                    }
                }
                timer += Time.deltaTime;
                yield return null;
            }
            isSinging = false;
            notesParticles[1].loop = false;
            notesParticles[2].loop = false;
            iSoundEffect.StopAbility2Sound();
            playerHP.canBeHurt = true;
            arenaMovement.enabled = true;
            playerAttack.enabled = true;
            mariachiGrenadeThrow.CanUseAbility = true;
            StartCoroutine(AbilityCooldownTimer());
        }

        public override void CancelAbility()
        {
            if (isSinging)
            {
                ArenaEvents.SecondAbility(false);
                StopCoroutine(coroutine);
                notesParticles[1].loop = false;
                notesParticles[2].loop = false;
                iSoundEffect.StopAbility2Sound();
                playerHP.canBeHurt = true;
                isSinging = false;
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
            Gizmos.DrawWireSphere(transform.position, minAttackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxAttackRange);
        }
    }
}