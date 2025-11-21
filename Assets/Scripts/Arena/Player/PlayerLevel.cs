using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Arena.Player {

    public class PlayerLevel : MonoBehaviour
    {
        public float expPoints = 0;
        public float expPointMaxCap = 100;
        public int playerLevel = 0;

        public Text levelText;
        public Animator levelUpAnim;

        private DivingElbowAbility divineElbowAbility;
        private TableChargeAbility tableChargeAbility;
        private ISoundEffect iSoundEffect;
        private bool unlockFirstAbility;
        private bool unlockSecondAbility;

        private void OnEnable()
        {
            DeathEvent.OnDeathExp += AddExp;
        }

        private void Awake()
        {
            divineElbowAbility = GetComponent<DivingElbowAbility>();
            tableChargeAbility = GetComponent<TableChargeAbility>();
            iSoundEffect = GetComponent<ISoundEffect>();
        }

        private void Start()
        {
            levelText.text = "Level " + playerLevel;
        }

        public void AddExp(float amount)
        {
            expPoints += amount;
            CheckLevel();
        }

        void LateUpdate()
        {
            levelUpAnim.transform.rotation = Quaternion.identity;
        }

        void CheckLevel()
        {
            if (expPoints >= expPointMaxCap)
            {
                levelUpAnim.Play(0);
                iSoundEffect.PlayLevelUpSound();
                CalculateAdditionalExperience();
                levelText.text = "Level " + playerLevel;
                UnlockAbility();
            }
        }

        private void CalculateAdditionalExperience()
        {
            var restExp = expPoints - expPointMaxCap;
            playerLevel++;
            expPointMaxCap += 100;
            expPoints = restExp;
        }

        void UnlockAbility()
        {
            if (unlockFirstAbility == false && playerLevel == 4)
            {
                divineElbowAbility.disable = false;
                unlockFirstAbility = true;
                Game.UI.PlayerUI.instance.UnlockAbility1(divineElbowAbility.abilityCooldown);
            }

            if (unlockSecondAbility == false && playerLevel == 9)
            {
                tableChargeAbility.disable = false;
                unlockSecondAbility = true;
                Game.UI.PlayerUI.instance.UnlockAbility2(tableChargeAbility.abilityCooldown);
            }
        }
        [ContextMenu("AddLevel")]
        public void AddLevel()
        {
            playerLevel++;
            UnlockAbility();
        }

        [ContextMenu("Unlock first ability")]
        private void Unlock1Ability()
        {
            divineElbowAbility.disable = false;
            unlockFirstAbility = true;
            Game.UI.PlayerUI.instance.UnlockAbility1(divineElbowAbility.abilityCooldown);
        }

        [ContextMenu("Unlock second ability")]
        private void Unlock2Ability()
        {
            tableChargeAbility.disable = false;
            unlockSecondAbility = true;
            Game.UI.PlayerUI.instance.UnlockAbility2(tableChargeAbility.abilityCooldown);
        }

        private void OnDestroy()
        {
            DeathEvent.OnDeathExp -= AddExp;
        }
    }
}
