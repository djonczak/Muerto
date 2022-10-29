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
        public Text _versionText;

        [SerializeField] private AudioSource _audioSource;

        private bool _canInteract = true;

        private const string StoryLevel = "01_Room";
        private const string ArenaLevel = "Arena";
        private const string ShowKey = "Show";

        private const string NameKey = "Name";

        private void Start()
        {
            credtitsWindow.SetActive(false);
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            _versionText.text = Application.version;
        }

        public void Story()
        {
            if (_canInteract)
            {
                _canInteract = false;
                StartCoroutine(FadeAudio());
                CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(StoryLevel), 2f);
            }
        }

        private IEnumerator FadeAudio()
        {
            var time = 0f;
            var startVolume = _audioSource.volume;
            while(time < 2f)
            {
                var value = Mathf.Lerp(startVolume, 0, time / 2f);
                _audioSource.volume = value;
                time += Time.deltaTime;
                yield return null;
            }
        }

        public void LoadArena()
        {
            if (PlayerPrefs.GetString(NameKey) == "")
            {
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                message.GetComponentInChildren<UnityEngine.UI.Text>().text = "Choose character !";
            }
            else
            {
                if (_canInteract)
                {
                    StartCoroutine(FadeAudio());
                    CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(ArenaLevel), 2);
                    _canInteract = false;
                }
            }
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
            if (_canInteract)
            {
                CameraManager.CameraFade.Instance.FadeIn(() => Application.Quit(), 2);
                _canInteract = false;
            }
        }

        public void GoBackToMainMenu()
        {
            if (_canInteract)
            {
                arenaWindow.SetActive(false);
                credtitsWindow.SetActive(false);
            }
        }
    }
}

