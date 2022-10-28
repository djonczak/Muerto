using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scene 
{
    public class BossScene : MonoBehaviour
    {
        [Header("Boss cutscene elements")]
        public GameObject bossText;
        public GameObject bloodCircle;
        public GameObject boss;

        [Header("Player to freeze")]
        [SerializeField] private GameObject player;

        [Header("Sound change")]
        public AudioSource sceneSound;
        public AudioClip bossFightClip;

        [Header("Objects to hide")]
        public GameObject[] hudToHide;

        [Header("Light source")]
        public Light sunLight;
        public Color colorToSwitch;
        private float _t;
        private bool _canSwitchColor;
        private Color _oldColor;

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
            _oldColor = sunLight.color;
            player = PlayerObject.GetPlayerObject();
        }

        private void Update()
        {
            if (_canSwitchColor == true)
            {
                _t += Time.deltaTime / 2f;
                sunLight.color = Color.Lerp(_oldColor, colorToSwitch, _t);
            }
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
        }

        private void ThirdPhaseOfScene()
        {
            UI.BlackBars.Instance.ShowBar(250, 2f);
            player.GetComponent<Arena.Player.ArenaMovement>().enabled = true;
            player.GetComponent<Arena.Player.PlayerAttack>().enabled = true;
            sceneSound.clip = bossFightClip;
            sceneSound.Play();
            boss.SetActive(true);
            this.enabled = false;
        }

        private void SecondPhaseOfScene()
        {
            bossText.SetActive(true);
            _canSwitchColor = false;
            bloodCircle.GetComponent<Animator>().SetTrigger(FillKey);
        }

        private void FirstPhaseOfScene()
        {
            player.GetComponent<Arena.Player.ArenaMovement>().enabled = false;
            player.GetComponent<Arena.Player.PlayerAttack>().enabled = false;
            player.GetComponent<Animator>().SetBool(IdleKey, true);
            player.GetComponent<Animator>().SetBool(RunKey, false);
            UI.BlackBars.Instance.HideBar(2f);
            sunLight.color = colorToSwitch;
            _canSwitchColor = true;
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
