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

        private bool _isAlive = true;
        private bool _isSecondPhase = false;
        private bool _isHurt = false;

        public Image healthBar;

        private const string BossKey = "Boss";

        private void Start()
        {
            currentHP = maxHP;
            healthBar.fillAmount = currentHP / maxHP;
        }

        public void TakeDamage(float amount, DamageType type)
        {
            if (_isAlive == true && _isHurt == false)
            {
                currentHP -= amount;
                healthBar.fillAmount = currentHP / maxHP;
                GetComponent<ISpriteEffect>().DamageEffect();
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

        private void Death()
        {
            healthBar.transform.parent.gameObject.SetActive(false);
            GetComponent<BossMeleeAttack>().enabled = false;
            GetComponent<BossMovement>().enabled = false;
            GetComponent<BossFirstAbility>().enabled = false;
            GetComponent<BossSecondAbility>().enabled = false;
            ArenaEvents.PlayerVictory();
            PlayerPrefs.SetString(BossKey, "Yes");
            enabled = false;
            this.gameObject.SetActive(false);
        }

        private void CheckSecondPhase()
        {
            if (currentHP <= 8 && _isSecondPhase == false)
            {
                GetComponent<BossSecondAbility>().unlock = true;
                ArenaEvents.SpawnTaco();
                _isSecondPhase = true;
            }
        }

        private IEnumerator DamageCooldown(float time)
        {
            _isHurt = true;
            yield return new WaitForSeconds(time);
            _isHurt = false;
        }
    }
}
