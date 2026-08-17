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

        private UnityEngine.UI.Image characterSprite;

        public bool IsUnlocked;

        public enum UnlockType
        {
            Story = 0,
            Arena = 1,
            Rest = 2,
        }

        private const string BossKey = "Boss";
        private const string StoryKey = "Story";
        private const string YesKey = "Yes";


        private void Awake()
        {
            characterSprite = GetComponent<UnityEngine.UI.Image>();
        }

        private void Start()
        {
            UpdateCharacter();
        }

        public void UpdateCharacter()
        {
            switch (unlockType)
            {
                case UnlockType.Story:
                    if (PlayerPrefs.GetString(StoryKey) == YesKey)
                    {
                        IsUnlocked = true;
                    }
                    else
                    {
                        IsUnlocked = false;
                    }
                    break;

                case UnlockType.Arena:
                    if (PlayerPrefs.GetString(BossKey) == YesKey)
                    {
                        IsUnlocked = true;
                    }
                    else
                    {
                        IsUnlocked = false;
                    }
                    break;
                case UnlockType.Rest:
                    IsUnlocked = true;
                    break;
            }

            if (IsUnlocked == true)
            {
                unlockInfo.SetActive(false);
                characterSprite.color = normalColor;
            }
            else
            {
                unlockInfo.SetActive(true);
                characterSprite.color = Color.black;
            }
        }
    }
}
