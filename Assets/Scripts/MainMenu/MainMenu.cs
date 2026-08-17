using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Game.Menu 
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject arenaWindow;
        public ButtonController menuButtons;
        public ButtonController arenaButton;
        public Texture2D cursorTexture;
        public GameObject message;
        public Text versionText;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource audioSourcePress;

        private const string StoryLevel = "01_Room";
        private const string ArenaLevel = "Arena";
        private const string ShowKey = "Show";

        private const string NameKey = "Name";

        private const string BossKey = "Boss";
        private const string StoryKey = "Story";
        private const string NoKey = "No";

        private void Start()
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            versionText.text = Application.version;

            //Application.targetFrameRate = -1;
            Application.targetFrameRate = 91;
        }

        public void Story()
        {
            menuButtons.DeactivateButtons();
            audioSourcePress.Play();
            StartCoroutine(FadeAudio());
            CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(StoryLevel), 2f);
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

        public void ClearPlayerPrefs()
        {
            audioSourcePress.Play();
            PlayerPrefs.SetString(BossKey, NoKey);
            PlayerPrefs.SetString(StoryKey, NoKey);
        }

        public void LoadArena()
        {
            if (PlayerPrefs.GetString(NameKey) == "")
            {
                audioSourcePress.Play();
                message.GetComponent<Animator>().SetTrigger(ShowKey);
                message.GetComponentInChildren<Text>().text = "Choose character !";
            }
            else
            {
                arenaButton.DeactivateButtons();
                audioSourcePress.Play();
                StartCoroutine(FadeAudio());
                CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(ArenaLevel), 2);
            }
        }

        public void OpenArenaWindow()
        {
            audioSourcePress.Play();
            menuButtons.DeactivateButtons();
            StartCoroutine(ShowArena());
        }

        private IEnumerator ShowArena()
        {
            yield return new WaitForSeconds(0.5F);
            arenaWindow.SetActive(true);
            arenaButton.ActivateButtons();
        }

        public void GoBackToMainMenu()
        {
            arenaButton.DeactivateButtons();
            audioSourcePress.Play();
            StartCoroutine(HideArena());
        }


        private IEnumerator HideArena()
        {
            yield return new WaitForSeconds(0.5F);
            arenaWindow.SetActive(false);
            menuButtons.ActivateButtons();
        }

    }
}

