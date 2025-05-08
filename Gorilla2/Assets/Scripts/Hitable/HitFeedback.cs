using Player;
using UnityEngine;
using static Player.ParticleManager.particleType;

namespace Hitable
{
    [RequireComponent(typeof(IHitable))]
    [RequireComponent(typeof(ParticleManager))]
    public class HitFeedback : MonoBehaviour
    {
        private IHitable hitable;
        private ParticleManager particleManager;

        private void Awake()
        {
            hitable = GetComponent<IHitable>();
            particleManager = GetComponent<ParticleManager>();
        }

        private void Start()
        {
            hitable.onHit += OnHit;
        }

        private void OnHit()
        {
            CameraShake.instance.Shake(CameraShake.Strength.weakShake);
            particleManager.EmitParticle(Hurt);
        }
    }
}