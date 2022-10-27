using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.VFX
{
    public class EyeRotation : MonoBehaviour
    {
        public float rotationAmount;

        private void FixedUpdate()
        {
            transform.Rotate(0, 0, rotationAmount * Time.fixedDeltaTime);
        }
    }
}