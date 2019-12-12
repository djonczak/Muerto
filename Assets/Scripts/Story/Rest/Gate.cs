using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gate : MonoBehaviour {

    GameObject player;
    PlayerData data;
    public Text text;

    private void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Manager");
        data = player.GetComponent<PlayerData>();
    }

    private void Start()
    {
        text.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            if (data.canPass)
            {
                SceneManager.LoadScene("03_Cementery");
            }
            else
            {
                text.enabled = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            text.enabled = false;
        }
    }
}
