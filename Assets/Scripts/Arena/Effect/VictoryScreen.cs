using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {

    public class VictoryScreen : MonoBehaviour
    {
        public Image blackScreen;
        public Text[] endTexts;
        public GameObject menuText;

        private bool _canColor = false;
        private Color _blackScreenMainColor;
        private Color _fontNormalColor;
        private Color _alphaColor = new Color(0f, 0f, 0f, 0f);
        private float _t;
        private bool _canPressButtons = false;

        private const string Menu = "Menu";

        private void OnEnable()
        {
            ArenaEvents.OnPlayerVictory += PlayerVictory;
        }

        private void Start()
        {
            _blackScreenMainColor = blackScreen.color;
            _fontNormalColor = endTexts[0].color;
            foreach (Text text in endTexts)
            {
                text.color = _alphaColor;
            }
        }

        private void Update()
        {
            if (_canColor)
            {
                _t += Time.deltaTime / 1f;
                blackScreen.color = Color.Lerp(_alphaColor, _blackScreenMainColor, _t);
                foreach (Text text in endTexts)
                {
                    text.color = Color.Lerp(_alphaColor, _fontNormalColor, _t);
                }
            }

            if (_canPressButtons == true)
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(Menu);
                }
            }
        }

        [ContextMenu("Show victory screen")]
        private void PlayerVictory()
        {
            StartCoroutine(BlackScreenShow(1f));
        }

        private IEnumerator BlackScreenShow(float time)
        {
            _canColor = true;
            yield return new WaitForSeconds(time);
            menuText.SetActive(true);
            _canColor = false;
            _canPressButtons = true;
        }

        private void OnDestroy()
        {
            ArenaEvents.OnPlayerVictory -= PlayerVictory;
        }
    }
}
