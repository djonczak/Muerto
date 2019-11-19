using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject credtitsWindow;
    public GameObject arenaWindow;
    public Texture2D cursorTexture;

    private void Start()
    {
        credtitsWindow.SetActive(false);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void Story()
    {
        SceneManager.LoadScene("01_Room");
    }

    public void LoadArena()
    {
        SceneManager.LoadScene("Arena");
    }

    public void OpenArenaWindow()
    {
        arenaWindow.SetActive(true);
    }

    public void OpenCreditsWindow()
    {
        credtitsWindow.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoBackToMainMenu()
    {
        arenaWindow.SetActive(false);
        credtitsWindow.SetActive(false);
    }
}

