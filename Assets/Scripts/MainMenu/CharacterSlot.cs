using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    public string characterName = "name";
    public GameObject unlockInfo;
    [SerializeField] private Color normalColor = new Color(255,255,255,255);

    public bool isUnlocked;

    public void Start()
    {
        if(PlayerPrefs.GetString("Boss") == "Yes")
        {
            isUnlocked = true;
        }

        if(isUnlocked == true)
        {
            unlockInfo.SetActive(false);
            GetComponent<UnityEngine.UI.Image>().color = normalColor;
        }
    }
}
