using MadeYellow.InputBuffer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        public enum InputActionType
        {
            Move,
            Sprint,
            Crouch,
            Jump,
            Attack
        }

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
                Vector2 attackInput = ctx.ReadValue<Vector2>();
                attackInput.x = 0;
                moveInput.y = 0;
                playerController.SetAttackDirection(attackInput);
                playerController.SetMoveInput(moveInput);
            }
            else if (ctx.canceled)
            {
                playerController.SetMoveInput(Vector2.zero);
                playerController.SetAttackDirection(Vector2.zero);
            }
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerController.SetInputState(InputActionType.Sprint, true);
            }

            if (ctx.canceled)
            {
                playerController.SetInputState(InputActionType.Sprint, false);
            }
        }

        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerController.SetInputState(InputActionType.Crouch, true);
            }

            if (ctx.canceled)
            {
                playerController.SetInputState(InputActionType.Crouch, false);
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

        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerController.SetInputState(InputActionType.Attack, true);
            }
            if (ctx.canceled)
            {
                playerController.SetInputState(InputActionType.Attack, false);
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