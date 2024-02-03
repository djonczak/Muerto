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
        public AudioSource _sceneAmbient;
        private AudioSource _audioSource;
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

        private Color _thunderColor = new Color(255f, 255f, 255f, 255f);
        private Color _deadColor = new Color(0f, 0f, 0f, 255f);
        private Color _fadeColor = new Color(0f, 0f, 0f, 0f);
        private Color _textColor = new Color(171f, 45f, 45f, 255f);

        private bool _showed = false;
        private bool _isDead = false;
        private bool _showText = false;

        private const string AttackKey = "Attack";
        private const string SpeedKey = "Speed";

        private const string Menu = "Menu";
        private const string PlayerTag = "Player";

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            SceneEffect();
        }

        private void SceneEffect()
        {
            if (_showed)
            {
                thunder.color = Color.Lerp(thunder.color, _fadeColor, 7f * Time.deltaTime);
            }

            if (_isDead)
            {
                deathScreen.color = Color.Lerp(deathScreen.color, _deadColor, Time.deltaTime / 200f);
            }

            if (_showText)
            {
                endText.color = Color.Lerp(endText.color, _textColor, Time.deltaTime * 300f);
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
            thunder.color = _thunderColor;
            _audioSource.PlayOneShot(monster);
            var player = collision.gameObject;
            SetPlayer(player);
            StartCoroutine(Action(timeForThuner, timeToSlash));
        }

        private IEnumerator Action(float timer, float timers)
        {
            yield return new WaitForSeconds(timer);
            _showed = true;
            bloodMoon.SetActive(true);
            normalMoon.SetActive(false);
            reaper.SetActive(true);
            _audioSource.PlayOneShot(soul);
            yield return new WaitForSeconds(timers);
            _isDead = true;
            reaper.GetComponent<Animator>().SetTrigger(AttackKey);
            yield return new WaitForSeconds(timer);
            _showText = true;
            _audioSource.PlayOneShot(slash);
            yield return new WaitForSeconds(2f);
            StartCoroutine(FadeAudio());
            CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(Menu), 2);
        }

        private IEnumerator FadeAudio()
        {
            var time = 0f;
            var startVolume = _sceneAmbient.volume;
            while (time < 2f)
            {
                var value = Mathf.Lerp(startVolume, 0, time / 2f);
                _sceneAmbient.volume = value;
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
