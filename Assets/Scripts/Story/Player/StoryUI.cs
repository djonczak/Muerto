using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StoryUI : MonoBehaviour
    {
        public Color color;
        public Text flowerText;
        public Text itemText;

        private GameObject _manager;
        private Story.PlayerData _data;

        void Start()
        {
            _manager = GameObject.FindGameObjectWithTag("Manager");
            _data = _manager.GetComponent<Story.PlayerData>();
        }

        void Update()
        {
            SetText();
        }

        public void SetText()
        {
            if (_data.hasFlower == true)
            {
                flowerText.color = color;
                flowerText.text = "You got flowers !";
            }
            else
            {
                flowerText.text = "Find flowers.";
            }
            if (_data.hasMask == true)
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
}