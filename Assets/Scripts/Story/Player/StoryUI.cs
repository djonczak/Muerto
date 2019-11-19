using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUI : MonoBehaviour
{
    public Color color;
    public Text flowerText;
    public Text itemText;

    GameObject manager;
    PlayerData data;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        data = manager.GetComponent<PlayerData>();
    }

    void Update()
    {
        SetText();
    }

    public void SetText()
    {
        if (data.hasFlower == true)
        {
            flowerText.color = color;
            flowerText.text = "You got flowers !";
        }
        else
        {
            flowerText.text = "Find flowers.";
        }
        if (data.hasMask == true)
        {
            itemText.color = color;
            itemText.text = "You got mask !";
        }
        else
        {
            itemText.text = "Find mask.";
        }
    }
}


   
        

