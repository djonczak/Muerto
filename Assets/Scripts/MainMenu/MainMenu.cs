using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Game.Menu 
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject credtitsWindow;
        public GameObject arenaWindow;
        public Texture2D cursorTexture;
        public GameObject message;
        public Text versionText;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource audioSourcePress;

        private bool canInteract = true;

        private const string StoryLevel = "01_Room";
        private const string ArenaLevel = "Arena";
        private const string ShowKey = "Show";

        private const string NameKey = "Name";

        private void Start()
        {
            credtitsWindow.SetActive(false);
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            versionText.text = Application.version;

            //Application.targetFrameRate = -1;
            Application.targetFrameRate = 91;
        }

        public void Story()
        {
            if (canInteract)
            {
                audioSourcePress.Play();
                canInteract = false;
                StartCoroutine(FadeAudio());
                CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(StoryLevel), 2f);
            }
        }

        private IEnumerator FadeAudio()
        {
            var time = 0f;
            var startVolume = audioSource.volume;
            while(time < 2f)
            {
                var value = Mathf.Lerp(startVolume, 0, time / 2f);
                audioSource.volume = value;
                time += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 0;
        }

        public void LoadArena()
        {
            if (PlayerPrefs.GetString(NameKey) == "")
            {
                audioSourcePress.Play();
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                message.GetComponentInChildren<UnityEngine.UI.Text>().text = "Choose character !";
            }
            else
            {
                if (canInteract)
                {
                    audioSourcePress.Play();
                    StartCoroutine(FadeAudio());
                    CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(ArenaLevel), 2);
                    canInteract = false;
                }
            }
        }

        public void OpenArenaWindow()
        {
            audioSourcePress.Play();
            arenaWindow.SetActive(true);
        }

        public void OpenCreditsWindow()
        {
            audioSourcePress.Play();
            credtitsWindow.SetActive(true);
        }

        public void Quit()
        {
            if (canInteract)
            {
                audioSourcePress.Play();
                CameraManager.CameraFade.Instance.FadeIn(() => Application.Quit(), 2);
                canInteract = false;
            }
        }

        public void GoBackToMainMenu()
        {
            if (canInteract)
            {
                audioSourcePress.Play();
                arenaWindow.SetActive(false);
                credtitsWindow.SetActive(false);
            }
        }
    }
}

