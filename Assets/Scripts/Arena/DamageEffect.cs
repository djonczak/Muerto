using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [Header("Damage effect options")]
    public float effectDuration;
    public ParticleSystem damageParticle;
    public Color normalColor;

    [SerializeField]
    private Color damageColor = Color.white;
    [SerializeField]
    private bool isDamaged, canSwitchColor;

    private SpriteRenderer sprite;
    private float t = 0f;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        normalColor = sprite.color;
    }

    public void Update()
    {
        if (canSwitchColor)
        {
            t += Time.deltaTime / 1f;
            if (isDamaged)
            {
                sprite.color = Color.Lerp(damageColor, normalColor, t);
            }
        }
    }

    public void ShowEffect()
    {
        StartCoroutine("EffectCooldown", effectDuration);
    }

    IEnumerator EffectCooldown(float time)
    {
        if (damageParticle != null)
        {
            damageParticle.Play();
        }
        isDamaged = true;
        canSwitchColor = true;
        yield return new WaitForSeconds(time);
        isDamaged = false;
        canSwitchColor = false;
        t = 0f;
    }
}
