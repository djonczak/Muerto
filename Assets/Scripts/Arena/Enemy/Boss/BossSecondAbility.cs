using UnityEngine;

public class BossSecondAbility : MonoBehaviour
{
    public bool unlock;

    [SerializeField] private float abilityCooldown = 5f;

    public AudioClip abilitySound;

    private AudioSource sound;
    private float timer;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckTimer();
    }

    private void CheckTimer()
    {
        if (unlock == true)
        {
            timer += Time.deltaTime;
            if (timer >= abilityCooldown)
            {
                timer = 0f;
                CastAbility();
                sound.PlayOneShot(abilitySound);
            }
        }
    }

    private void CastAbility()
    {
        GameObject waveAttack = ObjectPooler.instance.GetPooledObject("BossPoolAttack");
        if (waveAttack != null)
        {
            waveAttack.SetActive(true);
            waveAttack.GetComponent<BloodPillar>().UsePillar(PlayerObject.GetPlayerObject().transform);
        }
    }
}
