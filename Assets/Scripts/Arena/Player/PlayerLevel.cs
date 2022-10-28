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
        private bool _ability1unlock;
        private bool _ability2unlock;

        public Text levelText;
        public Animator levelUpAnim;

        private bool _unlockFirstAbility;
        private bool _unlockSecondAbility;

        private void OnEnable()
        {
            DeathEvent.OnDeathExp += AddExp;
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
                GetComponent<ISoundEffect>().PlayLevelUpSound();
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
            if (_unlockFirstAbility == false && playerLevel == 4)
            {
                GetComponent<DivingElbowAbility>().disable = false;
                _unlockFirstAbility = true;
                Game.UI.PlayerUI.instance.UnlockAbility1(GetComponent<DivingElbowAbility>().abilityCooldown);
            }

            if (_unlockSecondAbility == false && playerLevel == 9)
            {
                GetComponent<TableChargeAbility>().disable = false;
                _unlockSecondAbility = true;
                Game.UI.PlayerUI.instance.UnlockAbility2(GetComponent<TableChargeAbility>().abilityCooldown);
            }
        }
        [ContextMenu("AddLevel")]
        public void AddLevel()
        {
            playerLevel++;
            UnlockAbility();
        }

        [ContextMenu("Unlock first ability")]
        void Unlock1Ability()
        {
            GetComponent<DivingElbowAbility>().disable = false;
            _unlockFirstAbility = true;
            Game.UI.PlayerUI.instance.UnlockAbility1(GetComponent<DivingElbowAbility>().abilityCooldown);
        }

        [ContextMenu("Unlock second ability")]
        void Unlock2Ability()
        {
            GetComponent<TableChargeAbility>().disable = false;
            _unlockSecondAbility = true;
            Game.UI.PlayerUI.instance.UnlockAbility2(GetComponent<TableChargeAbility>().abilityCooldown);
        }

        private void OnDestroy()
        {
            DeathEvent.OnDeathExp -= AddExp;
        }
    }
}
