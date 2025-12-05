using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player
{
    public class MariachiTrumpetAbility : MonoBehaviour
    {
        public bool disable = false;
        [SerializeField] private bool canUseAbility = true;

        public bool CanUseAbility
        {
            set => canUseAbility = value;
        }

        [SerializeField] private float minAttackRange;
        [SerializeField] private float maxAttackRange;
        [SerializeField] private float abilityActiveTime;
        public float abilityCooldown = 15f;
        [SerializeField] private bool canUse = true;
        [SerializeField] private LayerMask enemyLayer = 11;
        [SerializeField] private ParticleSystem notesParticles;
        [SerializeField] private Transform notesPoint;
        private Animator animator;
        private PlayerAttack playerAttack;
        private ArenaMovement arenaMovement;
        private PlayerHP playerHP;
        private MariachiGrenadeThrow mariachiGrenadeThrow;

        private ISoundEffect iSoundEffect;

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
            if (disable == false)
            {
                if (canUseAbility)
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
            StartCoroutine(AbilityActive());
        }

        private IEnumerator AbilityActive()
        {
            notesParticles.transform.position = notesPoint.position;
            notesParticles.Play();
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
            iSoundEffect.StopAbility2Sound();
            playerHP.canBeHurt = true;
            arenaMovement.enabled = true;
            playerAttack.enabled = true;
            mariachiGrenadeThrow.CanUseAbility = true;
            StartCoroutine(AbilityCooldown());
        }

        private IEnumerator AbilityCooldown()
        {
            Game.UI.PlayerUI.instance.Used2Ability();
            yield return new WaitForSeconds(abilityCooldown);
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