using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.VFX 
{
    public class DustParticles : MonoBehaviour
    {
        public Transform dustStartPoint;
        public GameObject dustPrefab;
        public int amountToSpawn;
        private List<GameObject> dustParticles = new List<GameObject>();

        private void Start()
        {
            InstantiateDirt();
        }

        private void InstantiateDirt()
        {
            for (int i = 0; i <= amountToSpawn; i++)
            {
                var dirt = Instantiate(dustPrefab);
                dustParticles.Add(dirt);
            }
        }

        public void SpawnParticle()
        {
            GameObject dust = ReturnDirt();
            if (dust != null)
            {
                dust.transform.position = dustStartPoint.position;
                dust.SetActive(true);
                dust.GetComponent<ParticleSystem>().Play();
            }
        }

        private GameObject ReturnDirt()
        {
            for (int i = 0; i <= dustParticles.Count; i++)
            {
                if (dustParticles[i].GetComponent<ParticleSystem>().isPlaying == false)
                {
                    return dustParticles[i];
                }
            }
            return null;
        }
    }
}
