using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Menu 
{
    public class CharacterSlot : MonoBehaviour
    {
        public string characterName = "name";
        public GameObject unlockInfo;
        [SerializeField] private Color normalColor = new Color(255, 255, 255, 255);

        public bool IsUnlocked;

        private const string BossKey = "Boss";
        private const string YesKey = "Yes";

        public void Start()
        {
            if (PlayerPrefs.GetString(BossKey) == YesKey)
            {
                IsUnlocked = true;
            }

            if (IsUnlocked == true)
            {
                unlockInfo.SetActive(false);
                GetComponent<UnityEngine.UI.Image>().color = normalColor;
            }
        }
    }
}
