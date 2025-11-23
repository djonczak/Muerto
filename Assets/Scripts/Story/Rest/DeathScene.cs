using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game.Scene
{
    public class DeathScene : MonoBehaviour
    {
        public GameObject reaper;
        public AudioSource sceneAmbient;
        public AudioSource leafAmbient;
        private AudioSource audioSource;
        public AudioClip monster;
        public AudioClip slash;
        public AudioClip soul;
        public float timeForThuner = 2f;
        public float timeToSlash = 4f;

        public Image thunder;
        public Image deathScreen;
        public GameObject bloodMoon;
        public GameObject normalMoon;
        public Text endText;

        private Color thunderColor = new Color(255f, 255f, 255f, 255f);
        private Color deadColor = new Color(0f, 0f, 0f, 255f);
        private Color fadeColor = new Color(0f, 0f, 0f, 0f);
        private Color textColor = new Color(171f, 45f, 45f, 255f);

        private bool showed = false;
        private bool isDead = false;
        private bool showText = false;

        private const string AttackKey = "Attack";
        private const string SpeedKey = "Speed";

        private const string Menu = "Menu";
        private const string PlayerTag = "Player";

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            SceneEffect();
        }

        private void SceneEffect()
        {
            if (showed)
            {
                thunder.color = Color.Lerp(thunder.color, fadeColor, 7f * Time.deltaTime);
            }

            if (isDead)
            {
                deathScreen.color = Color.Lerp(deathScreen.color, deadColor, Time.deltaTime / 200f);
            }

            if (showText)
            {
                endText.color = Color.Lerp(endText.color, textColor, Time.deltaTime * 300f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == PlayerTag)
            {
                StartScene(collision);
            }
        }

        private void StartScene(Collider2D collision)
        {
            thunder.color = thunderColor;
            audioSource.PlayOneShot(monster);
            var player = collision.gameObject;
            SetPlayer(player);
            StartCoroutine(Action(timeForThuner, timeToSlash));
        }

        private IEnumerator Action(float timer, float timers)
        {
            yield return new WaitForSeconds(timer);
            showed = true;
            bloodMoon.SetActive(true);
            normalMoon.SetActive(false);
            reaper.SetActive(true);
            audioSource.PlayOneShot(soul);
            yield return new WaitForSeconds(timers);
            isDead = true;
            reaper.GetComponent<Animator>().SetTrigger(AttackKey);
            yield return new WaitForSeconds(timer);
            showText = true;
            audioSource.PlayOneShot(slash);
            yield return new WaitForSeconds(2f);
            StartCoroutine(FadeAudio(sceneAmbient));
            StartCoroutine(FadeAudio(leafAmbient));
            CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(Menu), 2);
        }

        private IEnumerator FadeAudio(AudioSource audioSource)
        {
            var time = 0f;
            var startVolume = audioSource.volume;
            while (time < 2f)
            {
                var value = Mathf.Lerp(startVolume, 0, time / 2f);
                audioSource.volume = value;
                time += Time.deltaTime;
                yield return null;
            }
        }

        private void SetPlayer(GameObject player)
        {
            player.GetComponent<Player.PlayerMovement>().CanMove = false;
            player.GetComponent<SpriteRenderer>().flipX = true;
            player.GetComponent<Animator>().SetFloat(SpeedKey, 0f);
        }
    }
}
