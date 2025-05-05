using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        PlayerStateMachine playerStateMachine;
        private void Awake()
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Vector2 moveInput = ctx.ReadValue<Vector2>();
                moveInput.y = 0;
                playerStateMachine.SetMoveInput(moveInput);
            }
            else if (ctx.canceled)
            {
                playerStateMachine.SetMoveInput(Vector2.zero);
            }
        }

        public void OnRun(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerStateMachine.ToggleRun(true);
            }
            else if (ctx.canceled)
            {
                playerStateMachine.ToggleRun(false);
            }
        }
        
        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerStateMachine.ToggleCrouch(true);
            }
            else if (ctx.canceled)
            {
                playerStateMachine.ToggleCrouch(false);
            }
        }
        
        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerStateMachine.Jump();
            }
        }
    }
}