using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        PlayerController playerController;
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Vector2 moveInput = ctx.ReadValue<Vector2>();
                moveInput.y = 0;
                playerController.SetMoveInput(moveInput);
            }
            else if (ctx.canceled)
            {
                playerController.SetMoveInput(Vector2.zero);
            }
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerController.StartSprint();
            }

            if (ctx.canceled)
            {
                playerController.StopSprint();
            }
        }
        
        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerController.StartCrouch();
            }

            if (ctx.canceled)
            {
                playerController.StopCrouch();
            }
        }
        
        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerController.StartJump();
            }

            if (ctx.canceled)
            {
                playerController.StopJump();
            }
        }
    }
}