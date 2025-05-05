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
            PlayerStateMachine playerStateMachine = GetComponent<PlayerStateMachine>();
            playerStateMachine.playerIdleState.OnStateEnter += () => SetColor(idleColor);
            playerStateMachine.playerRunState.OnStateEnter += () => SetColor(runColor);
            playerStateMachine.playerJumpState.OnStateEnter += () => SetColor(jumpColor);
            playerStateMachine.playerFallState.OnStateEnter += () => SetColor(fallColor);
            playerStateMachine.playerCrouchState.OnStateEnter += () => SetColor(crouchColor);
            playerStateMachine.playerWalkState.OnStateEnter += () => SetColor(walkColor);
        }
        
        private void SetColor(Color32 color)
        {
            spriteRenderer.color = color;
        }
    }
}