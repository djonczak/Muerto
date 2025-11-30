using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu 
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private Button[] buttons;

        public void ActivateButtons()
        {
            foreach(var button in buttons)
            {
                button.interactable = true;
            }
        }

        public void DeactivateButtons()
        {
            foreach (var button in buttons)
            {
                button.interactable = false;
            }
        }
    }
}
