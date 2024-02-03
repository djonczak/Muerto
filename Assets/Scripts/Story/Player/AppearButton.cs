using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class AppearButton : MonoBehaviour
    {
        [SerializeField] private Vector3 baseScale;
        [SerializeField] private Vector3 endScale;
        [SerializeField] private float appearDuration;

        private Coroutine coroutine;
        private float currentDuration;
        private float timer;
        private Vector3 oldPosition;

        private void Start()
        {
            currentDuration = appearDuration;
            transform.localScale = Vector3.zero;
            oldPosition = transform.position;
        }

        public void ShowButton()
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
                currentDuration -= timer;
                timer = 0f;
                transform.position = oldPosition;
            }
            coroutine = StartCoroutine(ButtonAppearing(endScale));
        }

        private IEnumerator ButtonAppearing(Vector3 endValue)
        {
            Vector3 startValue = transform.localScale;
            timer = 0f;
            while (timer < currentDuration)
            {
                var value = Vector3.Lerp(startValue, endValue, timer / currentDuration);
                transform.localScale = value;
                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0f;
            currentDuration = appearDuration;
        }

        public void HideButton()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                currentDuration -= timer;
                timer = 0f;
                transform.localScale = endScale;
            }

            coroutine = StartCoroutine(ButtonAppearing(baseScale));
        }
    }
}
