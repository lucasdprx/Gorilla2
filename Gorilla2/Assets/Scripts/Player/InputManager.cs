using System;
using System.Collections;
using MadeYellow.InputBuffer;
using Managers;
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
        private bool inputEnabled = true;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            GameManager.onGameFinished += OnGameFinished;
        }

        private void OnDestroy()
        {
            GameManager.onGameFinished -= OnGameFinished;
        }

        #region InputsEnabling

        public void EnableInputs()
        {
            inputEnabled = true;
        }
        
        public void DisableInputs()
        {
            inputEnabled = false;
            playerController.SetMoveInput(Vector2.zero);
            playerController.SetAttackDirection(Vector2.zero, Vector2.zero);
            playerController.SetInputState(InputActionType.Sprint, false);
            playerController.SetInputState(InputActionType.Crouch, false);
            playerController.SetInputState(InputActionType.Jump, false);
            playerController.SetInputState(InputActionType.Attack, false);
        }

        #endregion

        private void OnGameFinished()
        {
            DisableInputs();
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (!inputEnabled)
            {
                return;
            }
            if (ctx.performed)
            {
                Vector2 moveInput = ctx.ReadValue<Vector2>();
                Vector2 attackInput = ctx.ReadValue<Vector2>();
                moveInput.y = 0;
                playerController.SetAttackDirection(attackInput, moveInput);
                playerController.SetMoveInput(moveInput);
            }
            else if (ctx.canceled)
            {
                playerController.SetMoveInput(Vector2.zero);
                playerController.SetAttackDirection(Vector2.zero, Vector2.zero);
            }
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (!inputEnabled)
            {
                return;
            }
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
            if (!inputEnabled)
            {
                return;
            }
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
            if (!inputEnabled)
            {
                return;
            }
            if (ctx.performed)
            {
                playerController.SetInputState(InputActionType.Jump, true);
            }

            if (ctx.canceled)
            {
                playerController.SetInputState(InputActionType.Jump, false);
            }
        }

        #region Attack

        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if (!inputEnabled)
            {
                return;
            }
            if (!ctx.performed)
            {
                return;
            }
            playerController.SetInputState(InputActionType.Attack, true);
            StartCoroutine(ResetAttackInput());
        }

        private IEnumerator ResetAttackInput()
        {
            yield return new WaitForEndOfFrame();
            playerController.SetInputState(InputActionType.Attack, false);
        }

        #endregion
    }
}