using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player 
{
    public class SettingCharacter : MonoBehaviour
    {
        public GameObject Huan;
        public GameObject Ricardo;

        private const string NameKey = "Name";
        private const string RicardoKey = "Ricardo";
        private const string HuanKey = "Huan";

        private void Awake()
        {
            var name = PlayerPrefs.GetString(NameKey);
            if (name == HuanKey)
            {
                Huan.SetActive(true);
            }
            if (name == RicardoKey)
            {
                Ricardo.SetActive(true);
            }
        }
    }
}
