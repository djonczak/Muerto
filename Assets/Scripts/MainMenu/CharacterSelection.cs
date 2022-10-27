using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Menu {

    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private List<GameObject> characterList = new List<GameObject>();
        private int _index = 0;
        public GameObject message;

        private const string NameKey = "Name";
        private const string ShowKey = "Show";

        private void Start()
        {
            foreach (Transform child in transform)
            {
                characterList.Add(child.gameObject);
            }

            characterList[_index].SetActive(true);
        }

        public void IndexDown()
        {
            characterList[_index].gameObject.SetActive(false);
            _index--;
            if (_index < 0)
            {
                _index = characterList.Count - 1;
            }
            characterList[_index].gameObject.SetActive(true);
        }

        public void IndexUp()
        {
            characterList[_index].gameObject.SetActive(false);
            _index++;
            if (_index == characterList.Count)
            {
                _index = 0;
            }
            characterList[_index].gameObject.SetActive(true);
        }

        public void SelectCharacter()
        {
            var character = characterList[_index].GetComponent<CharacterSlot>();
            if (character.IsUnlocked == true)
            {
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                message.GetComponentInChildren<UnityEngine.UI.Text>().text = "You have chosen " + character.characterName + ".";
                PlayerPrefs.SetString(NameKey, character.name);
            }
            else
            {
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                message.GetComponentInChildren<UnityEngine.UI.Text>().text = "This character is locked !";
            }
        }
    }
}
