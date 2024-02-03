using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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
        public AudioClip bossFightClip;

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

        IEnumerator Scene()
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

        private void ThirdPhaseOfScene()
        {
            UI.BlackBars.Instance.HideBar(2f);
            player.GetComponent<Arena.Player.ArenaMovement>().enabled = true;
            player.GetComponent<Arena.Player.PlayerAttack>().enabled = true;
            sceneSound.clip = bossFightClip;
            sceneSound.Play();
        }

        private void SecondPhaseOfScene()
        {
            bossText.SetActive(true);
            bloodCircle.GetComponent<Animator>().SetTrigger(FillKey);
        }

        private void FirstPhaseOfScene()
        {
            UI.BlackBars.Instance.ShowBar(250, 2f);
            player.GetComponent<Arena.Player.TableChargeAbility>().CancelTableCharge();
            player.GetComponent<Arena.Player.ArenaMovement>().enabled = false;
            player.GetComponent<Arena.Player.PlayerAttack>().enabled = false;
            player.GetComponent<Animator>().SetBool(IdleKey, true);
            player.GetComponent<Animator>().SetBool(RunKey, false);
            StartCoroutine(ChangeSunColor());
            foreach (GameObject stuff in hudToHide)
            {
                stuff.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            ArenaEvents.OnBossShow -= StartScene;
        }
    }
}
