using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    [Header("Boss cutscene elements")]
    public GameObject bossText;
    public GameObject blackBars;

    [Header("Player to freeze")]
    public GameObject player;

    [Header("Sound change")]
    public AudioSource sceneSound;
    public AudioClip bossFightClip;
    public AudioClip darkLordClip;

    [Header("Objects to hide")]
    public GameObject[] hudToHide;

    [Header("Light source")]
    public Light sunLight;
    public Color colorToSwitch;

    private void OnEnable()
    {
        ArenaEvents.OnBossShow += StartScene;
    }

    private void Start()
    {
        blackBars.SetActive(false);
        bossText.SetActive(false);
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
    }

    private void SecondPhaseOfScene()
    {
        bossText.SetActive(true);
        foreach (GameObject stuff in hudToHide)
        {
            stuff.SetActive(false);
        }
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
    }

    private void OnDestroy()
    {
        ArenaEvents.OnBossShow -= StartScene;
    }
}
