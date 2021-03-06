﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public Image blackScreen;
    public Text[] endTexts;
    public GameObject menuText;

    private bool canColor = false;
    private Color blackScreenMainColor;
    private Color fontNormalColor;
    private Color alphaColor = new Color(0f, 0f, 0f, 0f);
    private float t;
    private bool canPressButtons = false;

    private void OnEnable()
    {
        ArenaEvents.OnPlayerVictory += PlayerVictory;
    }

    private void Start()
    {
        blackScreenMainColor = blackScreen.color;
        fontNormalColor = endTexts[0].color;
        foreach (Text text in endTexts)
        { 
            text.color = alphaColor;
        }
    }

    private void Update()
    {   
        if (canColor)
        {
            t += Time.deltaTime / 1f;
            blackScreen.color = Color.Lerp(alphaColor, blackScreenMainColor, t);
            foreach (Text text in endTexts)
            {
                text.color = Color.Lerp(alphaColor, fontNormalColor, t);
            }
        }

        if (canPressButtons == true)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }

    [ContextMenu("Show victory screen")]
    public void PlayerVictory()
    {
        StartCoroutine("BlackScreenShow", 1f);
    }

    private IEnumerator BlackScreenShow(float time)
    {
        canColor = true;
        yield return new WaitForSeconds(time);
        menuText.SetActive(true);
        canColor = false;
        canPressButtons = true;
    }

    private void OnDestroy()
    {
        ArenaEvents.OnPlayerVictory -= PlayerVictory;
    }
}
