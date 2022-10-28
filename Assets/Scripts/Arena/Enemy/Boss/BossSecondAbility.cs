using UnityEngine;


namespace Game.Arena.AI 
{
    public class BossSecondAbility : MonoBehaviour
    {
        public bool unlock;

        [SerializeField] private float abilityCooldown = 5f;

        public AudioClip abilitySound;

        private AudioSource _audioSource;
        private float _timer;

        private const string BossPoolAttackKey = "BossPoolAttack";

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            CheckTimer();
        }

        private void CheckTimer()
        {
            if (unlock == true)
            {
                _timer += Time.deltaTime;
                if (_timer >= abilityCooldown)
                {
                    _timer = 0f;
                    CastAbility();
                    _audioSource.PlayOneShot(abilitySound);
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
