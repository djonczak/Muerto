using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCharacter : MonoBehaviour
{ 
    public GameObject Huan;
    public GameObject Ricardo;

    private void Awake()
    {
        var name = PlayerPrefs.GetString("Name");
        if (name == "Huan")
        {
            Huan.SetActive(true);
        }
        if (name == "Ricardo")
        {
            Ricardo.SetActive(true);
        }
    }

}
