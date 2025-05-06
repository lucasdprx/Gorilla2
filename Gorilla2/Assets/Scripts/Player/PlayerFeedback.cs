using UnityEngine;

namespace Player
{
    public class PlayerFeedback : MonoBehaviour
    {
        [SerializeField] private Color32 idleColor;
        [SerializeField] private Color32 runColor;
        [SerializeField] private Color32 jumpColor;
        [SerializeField] private Color32 fallColor;
        [SerializeField] private Color32 crouchColor;
        [SerializeField] private Color32 walkColor;
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            PlayerController playerController = GetComponent<PlayerController>();
            playerController.idleState.onStateEnter += () => SetColor(idleColor);
            playerController.walkState.onStateEnter += () => SetColor(walkColor);
            playerController.sprintState.onStateEnter += () => SetColor(runColor);
            playerController.crouchState.onStateEnter += () => SetColor(crouchColor);
            playerController.jumpingState.onStateEnter += () => SetColor(jumpColor);
            playerController.fallState.onStateEnter += () => SetColor(fallColor);
        }
        
        private void SetColor(Color32 color)
        {
            spriteRenderer.color = color;
        }
    }
}