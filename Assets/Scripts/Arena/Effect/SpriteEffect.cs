using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.VFX 
{
    public class SpriteEffect : MonoBehaviour, ISpriteEffect
    {
        [Header("Damage effect options:")]
        [SerializeField] private float effectDuration = 1;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color damageColor = Color.white;

        [Header("Heal effect options:")]
        [SerializeField] private Color healColor = Color.green;
        [SerializeField] private bool isDamaged, canSwitchColor, isHealed;

        private SpriteRenderer _spriteRenderer;
        private float _t = 0f;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            normalColor = _spriteRenderer.color;
        }

        private void FixedUpdate()
        {
            if (canSwitchColor)
            {
                _t += Time.deltaTime / 1f;
                if (isDamaged)
                {
                    _spriteRenderer.color = Color.Lerp(damageColor, normalColor, _t);
                }
                if (isHealed)
                {
                    _spriteRenderer.color = Color.Lerp(healColor, normalColor, _t);
                }
            }
        }

        public void DamageEffect()
        {
            StartCoroutine(DamageEffectCooldown(effectDuration));
        }

        public void HealEffect()
        {
            StartCoroutine(HealCooldown(effectDuration));
        }

        private IEnumerator DamageEffectCooldown(float time)
        {
            isDamaged = true;
            canSwitchColor = true;
            yield return new WaitForSeconds(time);
            isDamaged = false;
            if (isHealed == false)
            {
                canSwitchColor = false;
                _t = 0f;
            }
        }

        private IEnumerator HealCooldown(float time)
        {
            isHealed = true;
            canSwitchColor = true;
            yield return new WaitForSeconds(time);
            isHealed = false;
            if (isDamaged == false)
            {
                canSwitchColor = false;
                _t = 0f;
            }
        }
    }
}
