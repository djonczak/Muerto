using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    [Header("Boss cutscene elements")]
    public GameObject bossText;
    public GameObject blackBars;
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
    private float t;
    private bool canSwitchColor;
    private Color oldColor;

    private void OnEnable()
    {
        ArenaEvents.OnBossShow += StartScene;
    }

    private void Start()
    {
        blackBars.SetActive(false);
        bossText.SetActive(false);
        oldColor = sunLight.color;
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject target in players)
        {
            if (target.activeSelf == true)
            {
                player = target;
            }
        }
    }

    private void Update()
    {
        if(canSwitchColor == true)
        {
            t += Time.deltaTime / 2f;
            sunLight.color = Color.Lerp(oldColor, colorToSwitch, t);
        }
    }

    [ContextMenu("Test boss scene")]
    private void StartScene()
    {
        StartCoroutine("Scene");
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
        blackBars.GetComponent<Animator>().SetTrigger("HideBars");
        player.GetComponent<ArenaMovement>().enabled = true;
        player.GetComponent<PlayerAttack>().enabled = true;
        sceneSound.clip = bossFightClip;
        sceneSound.Play();
        boss.SetActive(true);
        this.enabled = false;
    }

    private void SecondPhaseOfScene()
    {
        bossText.SetActive(true);
        canSwitchColor = false;
        bloodCircle.GetComponent<Animator>().SetTrigger("Fill");
    }

    private void FirstPhaseOfScene()
    {
        player.GetComponent<ArenaMovement>().enabled = false;
        player.GetComponent<PlayerAttack>().enabled = false;
        player.GetComponent<Animator>().SetBool("Idle", true);
        player.GetComponent<Animator>().SetBool("Run", false);
        blackBars.SetActive(true);
        blackBars.GetComponent<Animator>().SetTrigger("ShowBars");
        sunLight.color = colorToSwitch;
        canSwitchColor = true;
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
