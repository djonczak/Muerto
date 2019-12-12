using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Normal camera shake")]
    public float shakeTime;
    public float shakeStrenght;

    [Header("Boss camera shake")]
    public float bossShakeTime;
    public float bossShakeStrenght;

    private Vector3 oldPosition;
    private bool canShake = true;

    private void OnEnable()
    {
        DeathEvent.OnEnemyDeath += Shake;
        ArenaEvents.OnBossShow += BossShake;
        ArenaEvents.OnCameraStop += StopCamerShake;
    }

    private void Start()
    {
        oldPosition = transform.position;
    }

    public void Shake()
    {
        if (canShake == true)
        {
            StartCoroutine("ShakeDuration");
        }
    }

    public void BossShake()
    {
        StartCoroutine("BossShakeDuration");
    }

    private IEnumerator ShakeDuration()
    {
        var timer = 0f;
        while (timer < shakeTime)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Random.insideUnitSphere * 0.1f, shakeStrenght);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        transform.position = oldPosition;
    }

    private IEnumerator BossShakeDuration()
    {
        var timer = 0f;
        while (timer < shakeTime)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Random.insideUnitSphere * 0.1f, shakeStrenght);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        transform.position = oldPosition;
    }

    private void StopCamerShake()
    {
        Debug.Log("Stop");
        transform.position = oldPosition;
        StopAllCoroutines();
        canShake = false;
        StartCoroutine("ShakeCooldown", 5f);
    }

    private IEnumerator ShakeCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canShake = true;
    }

    private void OnDestroy()
    {
        DeathEvent.OnEnemyDeath -= Shake;
        ArenaEvents.OnBossShow -= BossShake;
        ArenaEvents.OnCameraStop -= StopCamerShake;
    }
}
