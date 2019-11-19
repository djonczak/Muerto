using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeTime;
    public float shakeStrenght;

    private Vector3 oldPosition;

    public void Start()
    {
        DeathEvent.OnEnemyDeath += Shake;
        oldPosition = transform.position;
    }

    public void Shake()
    {
        StartCoroutine("ShakeDuration");
    }

    public IEnumerator ShakeDuration()
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
}
