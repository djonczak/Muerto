using UnityEngine;


namespace Game.Arena.AI 
{
    public class BossSecondAbility : MonoBehaviour
    {
        public bool unlock;

        [SerializeField] private float abilityCooldown = 5f;

        public AudioClip abilitySound;

        private AudioSource audioSource;
        private float timer;

        private const string BossPoolAttackKey = "BossPoolAttack";

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
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
                    audioSource.PlayOneShot(abilitySound);
                }
            }
        }

        private void CastAbility()
        {
            GameObject waveAttack = Pooler.ObjectPooler.instance.GetPooledObject(BossPoolAttackKey);
            if (waveAttack != null)
            {
                waveAttack.SetActive(true);
                waveAttack.GetComponent<BloodPillar>().UsePillar(PlayerObject.GetPlayerObject().transform);
            }
        }
    }
}
