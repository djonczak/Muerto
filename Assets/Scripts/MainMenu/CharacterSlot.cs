using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    public string characterName = "name";
    public RuntimeAnimatorController controllerData;
    public GameObject unlockInfo;
    [SerializeField] private Color normalColor = new Color(255,255,255,255);

    public bool isUnlocked;

    public void Start()
    {
        if(isUnlocked == true)
        {
            unlockInfo.SetActive(false);
            GetComponent<UnityEngine.UI.Image>().color = normalColor;
        }
    }
}
