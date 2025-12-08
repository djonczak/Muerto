using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Game.Arena.Player;

namespace Game.Scene 
{
    public class BossScene : MonoBehaviour
    {
        [Header("Boss cutscene elements")]
        public GameObject bossText;
        public GameObject bloodCircle;
        public GameObject boss;
        [SerializeField] private GameObject bloodPilar;
        [Header("Player to freeze")]
        [SerializeField] private GameObject player;

        [Header("Sound change")]
        public AudioSource sceneSound;
        public AudioSource sceneSound2;

        [Header("Objects to hide")]
        public GameObject[] hudToHide;

        [Header("Light source")]
        public Light2D sunLight;
        public Color colorToSwitch;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string FillKey = "Fill";

        private void OnEnable()
        {
            ArenaEvents.OnBossShow += StartScene;
        }

        private void Start()
        {
            bossText.SetActive(false);
            player = PlayerObject.GetPlayerObject();
        }

        private IEnumerator ChangeSunColor()
        {
            var oldColor = sunLight.color;
            var timer = 0f;
            while(timer < 3f)
            {
                sunLight.color = Color.Lerp(oldColor, colorToSwitch, timer / 3f);
                timer += Time.deltaTime;
                yield return null;
            }
            sunLight.color = colorToSwitch;
        }

        [ContextMenu("Test boss scene")]
        private void StartScene()
        {
            StartCoroutine(Scene());
        }

        private IEnumerator Scene()
        {
            FirstPhaseOfScene();
            yield return new WaitForSeconds(2f);
            SecondPhaseOfScene();
            yield return new WaitForSeconds(2f);
            ThirdPhaseOfScene();
            yield return new WaitForSeconds(2f);
            bloodPilar.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            boss.SetActive(true);
        }

        private void FirstPhaseOfScene()
        {
            UI.BlackBars.Instance.ShowBar(250, 2f);
            DisableAbilities();
            player.GetComponent<ArenaMovement>().enabled = false;
            player.GetComponent<PlayerAttack>().enabled = false;
            player.GetComponent<Animator>().SetBool(IdleKey, true);
            player.GetComponent<Animator>().SetBool(RunKey, false);
            StartCoroutine(ChangeSunColor());
            foreach (GameObject stuff in hudToHide)
            {
                stuff.SetActive(false);
            }
            StartCoroutine(AudioFade(sceneSound, 0f, 4f));
        }

        private void SecondPhaseOfScene()
        {
            bossText.SetActive(true);
            bloodCircle.GetComponent<Animator>().SetTrigger(FillKey);
            sceneSound2.Play();
            StartCoroutine(AudioFade(sceneSound2, 0.25f, 3f));
        }

        private void ThirdPhaseOfScene()
        {
            UI.BlackBars.Instance.HideBar(2f);
            EnableAbilities();
            player.GetComponent<ArenaMovement>().enabled = true;
            player.GetComponent<PlayerAttack>().enabled = true;
        }

        private IEnumerator AudioFade(AudioSource audioSource, float endValue, float duration)
        {
            var timer = 0f;
            var startValue = audioSource.volume;
            while(timer < duration)
            {
                var value = Mathf.Lerp(startValue, endValue, timer / duration);
                audioSource.volume = value;
                timer += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = endValue;
        }


        private void DisableAbilities()
        {
            PlayerAbility[] abilities = player.GetComponents<PlayerAbility>();

            foreach (PlayerAbility ability in abilities)
            {
                ability.CancelAbility();
                ability.enabled = false;
            }
        }

        private void EnableAbilities()
        {
            PlayerAbility[] abilities = player.GetComponents<PlayerAbility>();

            foreach (PlayerAbility ability in abilities)
            {
                ability.enabled = true;
            }
        }

        private void OnDestroy()
        {
            ArenaEvents.OnBossShow -= StartScene;
        }
    }
}
