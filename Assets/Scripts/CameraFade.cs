using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.CameraManager
{
    public class CameraFade : MonoBehaviour
    {
        public static CameraFade Instance;

        [SerializeField] private float _fadeTime = 5.0f;

        [SerializeField] private Color _fadeColor = new Color(255.0f, 255.0f, 255.0f, 1.0f);


        private float _alpha = 1.0f;
        private Texture2D _texture;

        private bool _isFadingIn = false;
        private bool _isFadingOut = false;

        public Action FadedScreen;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            _texture = new Texture2D(1, 1);
            _texture.SetPixel(0, 0, new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _alpha));
            _texture.Apply();
            FadeOut(null, 2);
        }

        [ContextMenu("FadeIn")]
        public void FadeIn(Action action, float time, float alpha = 1.0f)
        {
            _isFadingIn = true;
            _isFadingOut = false;
            _alpha = alpha;
            StartCoroutine(StartFading(action, 0, 1, time));
        }

        [ContextMenu("FadeOut")]
        public void FadeOut(Action action, float time, float alpha = 0f)
        {
            _isFadingIn = false;
            _isFadingOut = true;
            _alpha = alpha;
            StartCoroutine(StartFading(action, 1, 0, time));
        }

        public void OnGUI()
        {
            if (_isFadingIn || _isFadingOut)
            {
                ShowBlackScreen();
            }
        }

        private IEnumerator StartFading( Action action, float min, float max, float duration)
        {
            var timer = 0f;
            while (timer < duration)
            {
                _alpha = Mathf.Lerp(min, max, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            if (action != null)
            {
                action.Invoke();
            }
            FadedScreen?.Invoke();
        }

        private void ShowBlackScreen()
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
            CalculateTexture();
        }

        private void CalculateTexture()
        {
            _texture.SetPixel(0, 0, new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _alpha));
            _texture.Apply();
        }
    }
}
