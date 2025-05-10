using UnityEngine;
using static Player.ParticleManager.particleType;

namespace Player
{
    [RequireComponent(typeof(ParticleManager))]
    public class PlayerFeedback : MonoBehaviour
    {
        [SerializeField] private Color32 idleColor;
        [SerializeField] private Color32 runColor;
        [SerializeField] private Color32 jumpColor;
        [SerializeField] private Color32 fallColor;
        [SerializeField] private Color32 crouchColor;
        [SerializeField] private Color32 walkColor;
        [SerializeField] private Color32 attackColor;
        [SerializeField] private Color32 stuntColor;
        private SpriteRenderer spriteRenderer;  
        private ParticleManager particleManager;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            particleManager = GetComponent<ParticleManager>();
        }

        private void Start()
        {
            PlayerController playerController = GetComponent<PlayerController>();
            playerController.idleState.onStateEnter += () => SetColor(idleColor);
            playerController.walkState.onStateEnter += onWalk;
            playerController.sprintState.onStateEnter += () => SetColor(runColor);
            playerController.crouchState.onStateEnter += () => SetColor(crouchColor);
            playerController.jumpingState.onStateEnter += () => SetColor(jumpColor);
            playerController.fallingState.onStateEnter += () => SetColor(fallColor);
            playerController.meleeState.onStateEnter += () => SetColor(attackColor);
            playerController.stuntState.onStateEnter += OnStunt;
        }

        private void onWalk()
        {
            SetColor(walkColor);
            particleManager.EmitParticle(Moving);
        }

        private void OnStunt()
        {
            SetColor(stuntColor);
            CameraShake.instance.Shake(CameraShake.Strength.mediumShake);
        }

        private void SetColor(Color32 color)
        {
            spriteRenderer.color = color;
        }
    }
}