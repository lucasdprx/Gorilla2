using System.Collections;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Player
{
    public class ParticleManager : MonoBehaviour
    {
        public enum particleType
        {
            Hurt,
            Moving,
        }

        [SerializeField] private SerializedDictionary<particleType, ParticleSystem> particleDictionary;

        public void EmitParticle(particleType particleType)
        {
            ParticleSystem particle = Instantiate(particleDictionary[particleType], transform.position, transform.rotation);
            StartCoroutine(DeleteParticle(particle));
        }

        private IEnumerator DeleteParticle(ParticleSystem particle)
        {
            yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
            Destroy(particle.gameObject);
        }
    }
}