using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Arena.AI
{
    public class BossHP : MonoBehaviour, IDamage
    {
        [SerializeField] private float maxHP = 15f;
        [SerializeField] private float currentHP = 0f;
        [SerializeField] private float damageCooldown = 0.4f;

        private bool isAlive = true;
        private bool isSecondPhase = false;
        private bool isHurt = false;

        public Image healthBar;
        private ISpriteEffect iSpriteEffect;

        private const string BossKey = "Boss";
        private const string YesKey = "Yes";

        private void Awake()
        {
            iSpriteEffect = GetComponent<ISpriteEffect>();
        }

        private void Start()
        {
            currentHP = maxHP;
            healthBar.fillAmount = currentHP / maxHP;
        }

        public void TakeDamage(float amount, DamageType type)
        {
            if (isAlive == true)
            {
                if (isHurt == false)
                {
                    currentHP -= amount;
                    healthBar.fillAmount = currentHP / maxHP;
                    iSpriteEffect.DamageEffect();
                    CheckSecondPhase();
                    if (currentHP <= 0)
                    {
                        Death();
                    }
                    else
                    {
                        StartCoroutine(DamageCooldown(damageCooldown));
                    }
                }
            }
        }

        private void Death()
        {
            healthBar.transform.parent.gameObject.SetActive(false);
            GetComponent<BossMeleeAttack>().enabled = false;
            GetComponent<BossMovement>().enabled = false;
            GetComponent<BossFirstAbility>().enabled = false;
            GetComponent<BossSecondAbility>().enabled = false;
            ArenaEvents.PlayerVictory();
            PlayerPrefs.SetString(BossKey, YesKey);
            enabled = false;
            this.gameObject.SetActive(false);
        }

        private void CheckSecondPhase()
        {
            if (currentHP <= 8 && isSecondPhase == false)
            {
                GetComponent<BossSecondAbility>().unlock = true;
                ArenaEvents.SpawnTaco();
                isSecondPhase = true;
            }
        }

        private IEnumerator DamageCooldown(float time)
        {
            isHurt = true;
            yield return new WaitForSeconds(time);
            isHurt = false;
        }
    }
}
