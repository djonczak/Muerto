using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public Image blackScreen;
    public Text endText;
    public GameObject[] textToActive;

    private bool canColor = false;
    private Color blackScreenMainColor;
    private Color fontNormalColor;
    private Color alphaColor = new Color(0f, 0f, 0f, 0f);
    private float t;
    private bool canPressButtons = false;

    private void OnEnable()
    {
        ArenaEvents.OnPlayerDeath += PlayerDeath;
    }

    private void Start()
    {
        blackScreenMainColor = blackScreen.color;
        fontNormalColor = endText.color;
        endText.color = alphaColor;
    }

    private void Update()
    {
        if (canColor)
        {
            t += Time.deltaTime / 1f;
            blackScreen.color = Color.Lerp(alphaColor, blackScreenMainColor, t);
            endText.color = Color.Lerp(alphaColor, fontNormalColor, t);
        }
        
        if (canPressButtons == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Arena");
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }

    public void PlayerDeath()
    {
        StartCoroutine("BlackScreenShow", 1f);
    }

    private IEnumerator BlackScreenShow(float time)
    {
        canColor = true;
        yield return new WaitForSeconds(time);
        canColor = false;
        foreach(GameObject button in textToActive)
        {
            button.SetActive(true);
        }
        canPressButtons = true;
    }

    private void OnDestroy()
    {
        ArenaEvents.OnPlayerDeath -= PlayerDeath;
    }
}
