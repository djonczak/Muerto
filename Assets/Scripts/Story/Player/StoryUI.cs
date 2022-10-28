using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Game.UI
{
    public class StoryUI : MonoBehaviour
    {
        public Color color;
        public Text flowerText;
        public Text itemText;
        public Text cemeteryText;

        private GameObject _manager;
        private Story.PlayerData _data;

        private void Start()
        {
            _manager = GameObject.FindGameObjectWithTag("Manager");
            _data = _manager.GetComponent<Story.PlayerData>();

            _data.PickedItem += SetText;
        }

        private void SetText()
        {
            if (_data.hasFlower == true)
            {
                flowerText.color = color;
                flowerText.text = "You got flowers !";
                CheckIfGatheredAll();
            }
            else
            {
                flowerText.text = "Find flowers.";
            }
            if (_data.hasMask == true)
            {
                itemText.color = color;
                itemText.text = "You got mask !";
                CheckIfGatheredAll();
            }
            else
            {
                itemText.text = "Find mask.";
            }
        }

        private async void CheckIfGatheredAll()
        {
            if(_data.hasFlower && _data.hasMask)
            {
                _data.PickedItem -= SetText;
                await Task.Delay(3 * 1000);
                flowerText.text = "";
                itemText.text = "";
                cemeteryText.text = "Go to cementery !";
            }
        }
    }
}