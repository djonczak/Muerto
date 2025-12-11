using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI 
{
    public class BlackBars : MonoBehaviour
    {
        public static BlackBars Instance;
        [SerializeField] private RectTransform topBar, bottomBar;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            SetBars();
        }

        private void SetBars()
        {
            GameObject newGameObject = new GameObject("_topBar", typeof(Image));
            newGameObject.transform.SetParent(transform, false);
            newGameObject.GetComponent<Image>().color = Color.black;

            topBar = newGameObject.GetComponent<RectTransform>();
            topBar.anchorMin = new Vector2(0, 1);
            topBar.anchorMax = new Vector2(1, 1);
            topBar.sizeDelta = new Vector2(0, 0);

            newGameObject = new GameObject("_bottomBar", typeof(Image));
            newGameObject.transform.SetParent(transform, false);
            newGameObject.GetComponent<Image>().color = Color.black;

            bottomBar = newGameObject.GetComponent<RectTransform>();
            bottomBar.anchorMin = new Vector2(0, 0);
            bottomBar.anchorMax = new Vector2(1, 0);
            bottomBar.sizeDelta = new Vector2(0, 0);
        }

        public void ShowBar(float targetSize = 100f, float duration = 3f)
        {
            StartCoroutine(BlackBarCoroutine(targetSize, duration));
        }

        public void HideBar(float duration = 3f)
        {
            StartCoroutine(BlackBarCoroutine(0f, duration));
        }

        private IEnumerator BlackBarCoroutine(float targetSize, float duration)
        {
            var timer = 0f;
            Vector2 sizeDelta = topBar.sizeDelta;
            float startSize = sizeDelta.y;
            while (timer < duration)
            {
                sizeDelta.y = Mathf.Lerp(startSize, targetSize, timer / duration);
                topBar.sizeDelta = sizeDelta;
                bottomBar.sizeDelta = sizeDelta;
                timer += Time.deltaTime;
                yield return null;
            }
        }

    }
}
