using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    public string Lvl;
    public GameObject button;

    private bool isColliding = false;

	void Update ()
    {
        if (isColliding == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                SceneManager.LoadScene(Lvl);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding = true;
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isColliding = false;
            button.SetActive(false);
        }
    }
}
