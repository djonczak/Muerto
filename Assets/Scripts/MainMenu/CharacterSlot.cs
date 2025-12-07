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

        [SerializeField] private UnlockType unlockType;

        public bool IsUnlocked;

        public enum UnlockType
        {
            Story = 0,
            Arena = 1,
        }

        private const string BossKey = "Boss";
        private const string StoryKey = "Story";
        private const string YesKey = "Yes";


        public void Start()
        {
            switch (unlockType)
            {
                case UnlockType.Story:
                    if (PlayerPrefs.GetString(StoryKey) == YesKey)
                    {
                        IsUnlocked = true;
                    }
                    break;

                case UnlockType.Arena:
                    if (PlayerPrefs.GetString(BossKey) == YesKey)
                    {
                        IsUnlocked = true;
                    }
                    break;
            }

            if (IsUnlocked == true)
            {
                unlockInfo.SetActive(false);
                GetComponent<UnityEngine.UI.Image>().color = normalColor;
            }
        }
    }
}
