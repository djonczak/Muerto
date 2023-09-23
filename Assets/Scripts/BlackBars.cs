using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI 
{
    public class BlackBars : MonoBehaviour
    {
        public static BlackBars Instance;
        [SerializeField] private RectTransform _topBar, _bottomBar;

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

            _topBar = newGameObject.GetComponent<RectTransform>();
            _topBar.anchorMin = new Vector2(0, 1);
            _topBar.anchorMax = new Vector2(1, 1);
            _topBar.sizeDelta = new Vector2(0, 0);

            newGameObject = new GameObject("_bottomBar", typeof(Image));
            newGameObject.transform.SetParent(transform, false);
            newGameObject.GetComponent<Image>().color = Color.black;

            _bottomBar = newGameObject.GetComponent<RectTransform>();
            _bottomBar.anchorMin = new Vector2(0, 0);
            _bottomBar.anchorMax = new Vector2(1, 0);
            _bottomBar.sizeDelta = new Vector2(0, 0);
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
            Vector2 sizeDelta = _topBar.sizeDelta;
            float startSize = sizeDelta.y;
            while (timer < duration)
            {
                sizeDelta.y = Mathf.Lerp(startSize, targetSize, timer / duration);
                _topBar.sizeDelta = sizeDelta;
                _bottomBar.sizeDelta = sizeDelta;
                timer += Time.deltaTime;
                yield return null;
            }
        }

    }
}
