using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game.Effect 
{
    public class LightFlick : MonoBehaviour
    {
        [SerializeField] private Light2D _lightFlick;

        [SerializeField] private float _minIntensity = 0f;

        [SerializeField] private float _maxIntensity = 1f;
        [SerializeField] private float _flickTime;

        private Coroutine _coroutine;

        private void Start()
        {
            Flick();
        }

        private void Flick()
        {
            _coroutine = StartCoroutine(FlickLight());
        }

        private IEnumerator FlickLight()
        {
            float newVal = Random.Range(_minIntensity, _maxIntensity);
            var time = 0f;
            var start = _lightFlick.intensity;
            while (time < _flickTime)
            {
                _lightFlick.intensity = Mathf.Lerp(start, newVal, time / _flickTime);
                time += Time.deltaTime;
                yield return null;
            }
            Flick();
        }

        private void OnDisable()
        {
            if (_coroutine == null) return;
            StopCoroutine(_coroutine);
        }
    }
}
