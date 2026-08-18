using System.Collections.Generic;
using UnityEngine;

namespace Game.Menu
{

    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private List<CharacterSlot> characterList = new List<CharacterSlot>();
        private int index = 0;
        [SerializeField] private AudioSource audioSourcePress;
        public GameObject message;
        private UnityEngine.UI.Text messageText;

        private const string NameKey = "Name";
        private const string ShowKey = "Show";

        private void Awake()
        {
            messageText = message.GetComponentInChildren<UnityEngine.UI.Text>();
        }
        public void IndexDown()
        {
            characterList[index].gameObject.SetActive(false);
            index--;
            if (index < 0)
            {
                index = characterList.Count - 1;
            }
            audioSourcePress.Play();
            characterList[index].gameObject.SetActive(true);
        }

        public void IndexUp()
        {
            characterList[index].gameObject.SetActive(false);
            index++;
            if (index == characterList.Count)
            {
                index = 0;
            }
            audioSourcePress.Play();
            characterList[index].gameObject.SetActive(true);
        }

        public void SelectCharacter()
        {
            audioSourcePress.Play();
            if (characterList[index].IsUnlocked == true)
            {
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                messageText.text = "You have choosen " + characterList[index].characterName;
                PlayerPrefs.SetString(NameKey, characterList[index].name);
            }
            else
            {
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                messageText.text = "This character is locked !";
            }
        }

        public void UpdateCache()
        {
            foreach (var character in characterList)
            {
                character.UpdateCharacter();
            }

            if (characterList.Count == 0)
            {
                foreach (Transform child in transform)
                {
                    var character = child.GetComponent<CharacterSlot>();
                    if (character != null)
                    {
                        characterList.Add(character);
                    }
                }
            }

            for (int i = 0; i < characterList.Count; i++)
            {
                var name = PlayerPrefs.GetString(NameKey);
                if (name == characterList[i].name)
                {
                    index = i;
                    characterList[i].gameObject.SetActive(true);
                }
                else
                {
                    characterList[i].gameObject.SetActive(false);
                }
            }
        }

        [ContextMenu("Test")]
        private void SetRicardo()
        {
            PlayerPrefs.SetString(NameKey, "Ricardo");
        }
    }
}
