using UnityEngine;
using UnityEngine.UI;

namespace Game.Arena.Player
{
    public class PlayerHP : MonoBehaviour, IDamage, IHeal
    {
        [SerializeField] private float currentHP = 0;
        [SerializeField] private float maxHP = 2;
        public Image[] healthBars;
        private int _i = -1;

        public bool isAlive = true;
        public bool canBeHurt;

        private const string RunKey = "Run";
        private const string IdleKey = "Idle";
        private const string DeathKey = "Death";

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
                    _i++;
                    healthBars[_i].enabled = false;
                    GetComponent<ISpriteEffect>().DamageEffect();
                }

                if (currentHP <= 0)
                {
                    Death();
                }
            }
        }

        private void Death()
        {
            isAlive = false;
            var animator = GetComponent<Animator>();
            animator.SetTrigger(DeathKey);
            animator.SetBool(IdleKey, false);
            animator.SetBool(RunKey, false);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<ArenaMovement>().enabled = false;
            GetComponent<TableChargeAbility>().enabled = false;
            GetComponent<DivingElbowAbility>().enabled = false;
            GetComponent<ISoundEffect>().PlayDeathSound();
            this.enabled = false;
            ArenaEvents.PlayerDeath();
        }

        public void Heal(float amount, TacoHeal taco)
        {
            if (currentHP < maxHP)
            {
                currentHP += amount;
                healthBars[_i].enabled = true;
                _i--;
                GetComponent<ISpriteEffect>().HealEffect();
                GetComponent<ISoundEffect>().PlayHealSound();
                taco.Healed();
            }
        }
    }
}
