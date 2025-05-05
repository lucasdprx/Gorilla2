using System;
using Player.PlayerStates;
using UnityEngine;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [field: SerializeField, Min(0)]
        public float walkSpeed { get; private set; }

        [field: SerializeField, Min(0)]
        public float runSpeed { get; private set; }

        [field: SerializeField, Min(0)]
        public float crouchSpeed { get; private set; }

        [field: SerializeField, Min(0)]
        public float jumpForce { get; private set; } = 10;

        [field: SerializeField, Min(0)]
        public float raycastGroundDistance { get; private set; } = 10;

        [field: SerializeField] public float moveSpeedThreshold { get; private set; } = 0.1f;
        [SerializeField] private float jumpGravityScale = 2f;
        [SerializeField] private float defaultGravityScale = 5f;
        public Rigidbody2D rb { get; private set; }
        public Vector2 moveInput { get; private set; }
        PlayerState currentState;
        public PlayerIdleState playerIdleState = new();
        public PlayerWalkState playerWalkState = new();
        public PlayerRunState playerRunState = new();
        public PlayerCrouchState playerCrouchState = new();
        public PlayerJumpState playerJumpState = new();
        public PlayerFallState playerFallState = new();
        public bool isRunning { get; private set; }
        public bool isCrouching { get; private set; }
        public float currentSpeed { get; private set; }

        public void SetCurrentSpeed(float newSpeed)
        {
            currentSpeed = newSpeed;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = defaultGravityScale;
        }

        public void ToggleRun(bool runState)
        {
            isRunning = runState;
        }

        public bool IsFalling()
        {
            return rb.linearVelocity.y < 0;
        }

        public bool IsJumping()
        {
            return rb.linearVelocity.y > 0.1f;
        }

        public void StartJump()
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector2.up * jumpForce);
                rb.gravityScale = jumpGravityScale;
            }
        }
        
        public void StopJump()
        {
            rb.gravityScale = defaultGravityScale;
        }

        public bool IsGrounded()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, raycastGroundDistance,
                LayerMask.GetMask("Ground"));
        }

        public void ToggleCrouch(bool crouchState)
        {
            isCrouching = crouchState;
        }

        public void SetMoveInput(Vector2 moveInput)
        {
            this.moveInput = moveInput;
        }

        public void Move()
        {
            Vector2 newVel = moveInput.normalized * currentSpeed;
            newVel.y = rb.linearVelocity.y;
            rb.linearVelocity = newVel;
        }
        
        public void AddSpeed(float speed)
        {
            Vector2 newVel = moveInput.normalized * speed;;
            rb.linearVelocity += newVel * Time.deltaTime;
        }

        private void Start()
        {
            
            currentState = playerIdleState;
            currentState.Enter(this);
        }

        public void ChangeState(PlayerState newState)
        {
            currentState = newState;
            currentState.Enter(this);
        }

        private void Update()
        {
            currentState.Update(this);
        }

        public bool IsMoving()
        {
            return moveInput.magnitude >= moveSpeedThreshold;
        }
    }
}