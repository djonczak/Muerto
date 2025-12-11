using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.CameraManager
{
    public class CameraFade : MonoBehaviour
    {
        public static CameraFade Instance;

        [SerializeField] private float fadeTime = 5.0f;

        [SerializeField] private Color fadeColor = new Color(255.0f, 255.0f, 255.0f, 1.0f);


        private float alpha = 1.0f;
        private Texture2D texture;

        private bool isFadingIn = false;
        private bool isFadingOut = false;

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
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            FadeOut(null, 2);
        }

        [ContextMenu("FadeIn")]
        public void FadeIn(Action action, float time, float alpha = 1.0f)
        {
            isFadingIn = true;
            isFadingOut = false;
            this.alpha = alpha;
            StartCoroutine(StartFading(action, 0, 1, time));
        }

        [ContextMenu("FadeOut")]
        public void FadeOut(Action action, float time, float alpha = 0f)
        {
            isFadingIn = false;
            isFadingOut = true;
            this.alpha = alpha;
            StartCoroutine(StartFading(action, 1, 0, time));
        }

        public void OnGUI()
        {
            if (isFadingIn || isFadingOut)
            {
                ShowBlackScreen();
            }
        }

        private IEnumerator StartFading( Action action, float min, float max, float duration)
        {
            var timer = 0f;
            while (timer < duration)
            {
                alpha = Mathf.Lerp(min, max, timer / duration);
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
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
            CalculateTexture();
        }

        private void CalculateTexture()
        {
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
        }
    }
}
