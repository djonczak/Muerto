using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class ActivateParticleOnEnable : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleToActivate;

        private void OnEnable()
        {
            particleToActivate.Play();
        }
    }
}
