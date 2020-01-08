using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCharacter : MonoBehaviour
{
    private Animator characterAnimator;

    public RuntimeAnimatorController Huan;
    public RuntimeAnimatorController Ricardo;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();

        var name = PlayerPrefs.GetString("Name");
        if (name == "Huan")
        {
            GetComponent<SpriteRenderer>().color = new Color(207, 207, 207, 255);
            characterAnimator.runtimeAnimatorController = Huan;
        }
        if (name == "Ricardo")
        {
            GetComponent<SpriteRenderer>().color = new Color(181, 181, 181, 255);
            characterAnimator.runtimeAnimatorController = Ricardo;
        }
    }
}
