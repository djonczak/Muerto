using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Arena.Player
{

    public abstract class PlayerAbility : MonoBehaviour
    {
        public bool Disabled
        {
            set => disable = value;
            get => disable;
        }
        private bool disable = true;


        public float AbilityCooldown
        {
            get => abilityCooldown;
            set => abilityCooldown = value;
        }
        private float abilityCooldown = 15f;

        public bool CanUseAbility
        {
            set => canUseAbility = value;
            get => canUseAbility;
        }

        private bool canUseAbility = true;

        public virtual void CancelAbility()
        {

        }

    }
}
