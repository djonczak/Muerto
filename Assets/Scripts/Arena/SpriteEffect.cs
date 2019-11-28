using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    [Header("Damage effect options:")]
    public float effectDuration;
    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color damageColor = Color.white;

    [Header("Heal effect options:")]
    [SerializeField]
    private Color healColor = Color.green;
    [SerializeField]
    private bool isDamaged, canSwitchColor, isHealed;

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
            if (isHealed)
            {
                sprite.color = Color.Lerp(healColor, normalColor, t);
            }
        }
    }

    public void DamageEffect()
    {
        StartCoroutine("DamageEffectCooldown", effectDuration);
    }

    public void HealEffect()
    {
        StartCoroutine("HealCooldown", effectDuration);
    }

    IEnumerator DamageEffectCooldown(float time)
    {
        isDamaged = true;
        canSwitchColor = true;
        yield return new WaitForSeconds(time);
        isDamaged = false;
        if (isHealed == false)
        {
            canSwitchColor = false;
            t = 0f;
        }
    }

    IEnumerator HealCooldown(float time)
    {
        isHealed = true;
        canSwitchColor = true;
        yield return new WaitForSeconds(time);
        isHealed = false;
        if (isDamaged == false)
        {
            canSwitchColor = false;
            t = 0f;
        }
    }
}
