using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Normal camera shake")]
    [SerializeField] private float shakeTime = 0.1f;
    [SerializeField] private float shakeStrenght = 20f;

    [Header("Boss camera shake")]
    [SerializeField] private float bossShakeTime = 0.4f;

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
        transform.position = oldPosition;
    }

    private IEnumerator BossShakeDuration()
    {
        var timerBoss = 0f;
        while (timerBoss < bossShakeTime)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Random.insideUnitSphere * 0.05f, shakeStrenght);
            timerBoss += Time.deltaTime;
            yield return null;
        }
        transform.position = oldPosition;
    }

    private void StopCamerShake()
    {
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
