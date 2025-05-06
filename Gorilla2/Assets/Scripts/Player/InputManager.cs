using MadeYellow.InputBuffer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        PlayerController playerController;
        
        [SerializeField] private float jumpBufferTime = 0.2f;

        private SimpleInputBuffer startJumpBuffer = new();
        private SimpleInputBuffer stopJumpBuffer = new();
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            startJumpBuffer = new SimpleInputBuffer(jumpBufferTime);
            stopJumpBuffer = new SimpleInputBuffer(jumpBufferTime);
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
                startJumpBuffer.Reset();
                stopJumpBuffer.Reset();
                startJumpBuffer.Set();
            }

            if (ctx.canceled)
            {
                stopJumpBuffer.Reset();
                stopJumpBuffer.Set();
            }
        }

        private void Update()
        {
            if (startJumpBuffer.hasBuffer && playerController.TryStartJump()) 
            {
                startJumpBuffer.Reset();
            }
            
            if (stopJumpBuffer.hasBuffer && !startJumpBuffer.hasBuffer && playerController.TryStopJump()) 
            {
                stopJumpBuffer.Reset();
            }
        }
    }
}