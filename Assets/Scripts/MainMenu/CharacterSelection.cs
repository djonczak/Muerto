using System.Collections.Generic;
using UnityEngine;

namespace Game.Menu
{

    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private List<GameObject> characterList = new List<GameObject>();
        private int index = 0;
        [SerializeField] private AudioSource audioSourcePress;
        public GameObject message;

        private const string NameKey = "Name";
        private const string ShowKey = "Show";

        private void Start()
        {
            foreach (Transform child in transform)
            {
                characterList.Add(child.gameObject);
            }

            characterList[index].SetActive(true);

            Debug.Log(PlayerPrefs.GetString(NameKey));
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
            var character = characterList[index].GetComponent<CharacterSlot>();
            if (character.IsUnlocked == true)
            {
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                message.GetComponentInChildren<UnityEngine.UI.Text>().text = "You have choosen " + character.characterName;
                PlayerPrefs.SetString(NameKey, character.name);
            }
            else
            {
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                message.GetComponentInChildren<UnityEngine.UI.Text>().text = "This character is locked !";
            }
        }

        [ContextMenu("Test")]
        private void SetRicardo()
        {
            PlayerPrefs.SetString(NameKey, "Ricardo");
        }
    }
}
