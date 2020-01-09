using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterList = new List<GameObject>();
    private int index = 0;
    public GameObject message;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            characterList.Add(child.gameObject);
        }

        characterList[index].SetActive(true);
    }

    public void IndexDown()
    {
        characterList[index].gameObject.SetActive(false);
        index--;
        if (index < 0)
        {
            index = characterList.Count - 1;
        }
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
        characterList[index].gameObject.SetActive(true);
    }

    public void SelectCharacter()
    {
        var character = characterList[index].GetComponent<CharacterSlot>();
        if (character.isUnlocked == true)
        {
            message.GetComponent<Animator>().SetTrigger("Show");
            message.GetComponentInChildren<UnityEngine.UI.Text>().text = "You have chosen " + character.characterName + ".";
            PlayerPrefs.SetString("Name", character.name);
        }
        else
        {
            message.GetComponent<Animator>().SetTrigger("Show");
            message.GetComponentInChildren<UnityEngine.UI.Text>().text = "This character is locked !";
        }
    }

}
