using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game.Arena.UI
{

    public class DeathScreen : MonoBehaviour
    {
        public Image blackScreen;
        public Text endText;
        public GameObject[] textToActive;

        private bool _canColor = false;
        private Color _blackScreenMainColor = Color.black;
        private Color _fontNormalColor;
        private Color _alphaColor = new Color(0f, 0f, 0f, 0f);
        private float _t;
        private bool _canPressButtons = false;

        private const string Menu = "Menu";
        private const string Arena = "Arena";

        private void OnEnable()
        {
            ArenaEvents.OnPlayerDeath += PlayerDeath;
        }

        private void Start()
        {
            _fontNormalColor = endText.color;
            endText.color = _alphaColor;
        }

        private void Update()
        {
            if (_canColor)
            {
                _t += Time.deltaTime / 1f;
                blackScreen.color = Color.Lerp(_alphaColor, _blackScreenMainColor, _t);
                endText.color = Color.Lerp(_alphaColor, _fontNormalColor, _t);
            }

            if (_canPressButtons == true)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(Arena), 2f);
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    CameraManager.CameraFade.Instance.FadeIn(() =>SceneManager.LoadScene(Menu), 2f);
                }
            }
        }

        [ContextMenu("Show death screen")]
        private void PlayerDeath()
        {
            StartCoroutine(BlackScreenShow(1f));
        }

        private IEnumerator BlackScreenShow(float time)
        {
            _canColor = true;
            yield return new WaitForSeconds(time);
            _canColor = false;
            foreach (GameObject button in textToActive)
            {
                button.SetActive(true);
            }
            _canPressButtons = true;
        }

        private void OnDestroy()
        {
            ArenaEvents.OnPlayerDeath -= PlayerDeath;
        }
    }
}
