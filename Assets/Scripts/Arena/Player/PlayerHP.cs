using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game.Arena.Events;

namespace Game.Arena.Player
{
    public class PlayerHP : MonoBehaviour, IDamage, IHeal
    {
        [SerializeField] private float currentHP = 0;
        [SerializeField] private float maxHP = 3;
        public Image[] healthBars;
        private int i = -1;
        public bool isAlive = true;
        public bool canBeHurt;

        private ISoundEffect iSoundEffect;
        private ISpriteEffect iSpriteEffect;

        private const string RunKey = "Run";
        private const string IdleKey = "Idle";
        private const string DeathKey = "Death";

        private void Awake()
        {
            iSoundEffect = GetComponent<ISoundEffect>();
            iSpriteEffect = GetComponent<ISpriteEffect>();
        }

        private void Start()
        {
            currentHP = maxHP;
        }

        public void TakeDamage(float amount, DamageType type)
        {
            if (isAlive == true)
            {
                if (canBeHurt == true)
                {
                    currentHP -= amount;
                    i++;
                    healthBars[i].enabled = false;
                    iSpriteEffect.DamageEffect();
                    ArenaEvents.PlayerGotHurt();
                    if (currentHP <= 0)
                    {
                        Death();
                        return;
                    }
                    StartCoroutine(DamageCooldown());

                }

                if (currentHP <= 0)
                {
                    Death();
                }
            }
        }

        public void DeathOverTime(float duration)
        {

        }

        private IEnumerator DamageCooldown()
        {
            canBeHurt = false;
            yield return new WaitForSeconds(1.5f);
            canBeHurt = true;
        }

        private void Death()
        {
            isAlive = false;
            DisableAbilities();
            var animator = GetComponent<Animator>();
            animator.SetTrigger(DeathKey);
            animator.SetBool(IdleKey, false);
            animator.SetBool(RunKey, false);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<ArenaMovement>().enabled = false;
            iSoundEffect.PlayDeathSound();
            this.enabled = false;
            ArenaEvents.PlayerDeath();
        }

        public void Heal(float amount, TacoHeal taco)
        {
            if (currentHP < maxHP)
            {
                currentHP += amount;
                healthBars[i].enabled = true;
                i--;
                iSpriteEffect.HealEffect();
                iSoundEffect.PlayHealSound();
                taco.Healed();
            }
        }

        private void DisableAbilities()
        {
            PlayerAbility[] abilities = GetComponents<PlayerAbility>();

            foreach (PlayerAbility ability in abilities)
            {
                ability.CancelAbility();
                ability.CanUseAbility = false;
            }
        }
    }
}
