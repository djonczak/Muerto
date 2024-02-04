using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.UI
{
    public class HealthBarFollow : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [Range(-1, 0)]
        [SerializeField] private float offset;

        private void Start()
        {
            target = PlayerObject.GetPlayerObject();
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offset, target.transform.position.z);
        }
    }
}
