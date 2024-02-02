using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Game.Story;

namespace Game.UI
{
    public class StoryUI : MonoBehaviour
    {
        public Color color;
        public Text flowerText;
        public Text itemText;
        public Text cemeteryText;

        private void Start()
        {
            PlayerData.Instance.PickedItem += SetText;
        }

        private void SetText()
        {
            if (PlayerData.Instance.HasFlower == true)
            {
                flowerText.color = color;
                flowerText.text = "Got flowers !";
            }

            if (PlayerData.Instance.HasMask == true)
            {
                itemText.color = color;
                itemText.text = "Got mask !";
            }

            CheckIfGatheredAll();
        }

        private void CheckIfGatheredAll()
        {
            if (PlayerData.Instance.HasFlower && PlayerData.Instance.HasMask)
            {
                PlayerData.Instance.PickedItem -= SetText;
                StartCoroutine(ChangeMissionText());
            }
        }

        private IEnumerator ChangeMissionText()
        {
            yield return new WaitForSeconds(3f);
            flowerText.text = "";
            itemText.text = "";
            cemeteryText.text = "Visit cementery !";
        }
    }
}