using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour {

    public GameObject reaper;
    AudioSource source;
    public AudioClip monster;
    public AudioClip slash;
    public AudioClip soul;
    public float timeForThuner = 2f;
    public float timeToSlash = 4f;

    public Image thunder;
    public Image deathScreen;
    public GameObject bloodMoon;
    public Text endText;

    Color thunderColor = new Color(255f, 255f, 255f, 255f);
    Color deadColor = new Color(0f, 0f, 0f, 255f);
    Color FadeColor = new Color(0f, 0f, 0f, 0f);
    Color textColor = new Color(171f, 45f, 45f, 255f);

    bool showed = false;
    bool isDead = false;
    bool showText = false;

    void Start ()
    {
        source = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        SceneEffect();
    }

    private void SceneEffect()
    {
        if (showed)
        {
            thunder.color = Color.Lerp(thunder.color, FadeColor, 7f * Time.deltaTime);
        }

        if (isDead)
        {
            deathScreen.color = Color.Lerp(deathScreen.color, deadColor, Time.deltaTime / 200f);
        }

        if (showText)
        {
            endText.color = Color.Lerp(endText.color, textColor, Time.deltaTime * 300f);
        }
    }

    IEnumerator Action(float timer, float timers)
    {
        yield return new WaitForSeconds(timer);
        showed = true;
        bloodMoon.SetActive(true);
        reaper.SetActive(true);
        source.PlayOneShot(soul);
        yield return new WaitForSeconds(timers);
        isDead = true;
        reaper.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(timer);
        showText = true;
        source.PlayOneShot(slash);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Menu");       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartScene(collision);
        }
    }

    private void StartScene(Collider2D collision)
    {
        thunder.color = thunderColor;
        source.PlayOneShot(monster);
        var player = collision.gameObject;
        SetPlayer(player);
        StartCoroutine(Action(timeForThuner, timeToSlash));
    }

    private static void SetPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovement>().Scene = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<SpriteRenderer>().flipX = true;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
    }
}
