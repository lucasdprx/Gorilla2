using System;
using Platformer;
using Player.PlayerStates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float walkSpeed { get; private set; }

        [field: SerializeField, Min(0)] public float sprintSpeed { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float crouchSpeed { get; private set; } = 10;

        [field: SerializeField, Min(0)] public float jumpForce { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float decelerationForce { get; private set; } = 10;

        [field: SerializeField, Min(0)] public float raycastGroundDistance { get; private set; } = 10;
        [SerializeField] private float coyoteTime = .2f;

        [field: SerializeField] public float moveSpeedThreshold { get; private set; } = 0.1f;
        [SerializeField] private float jumpGravityScale = 2f;
        [SerializeField] private float defaultGravityScale = 5f;
        public Rigidbody2D rb { get; private set; }
        public Vector2 moveInput { get; private set; }
        public StateMachine stateMachine { get; private set; }
        public float currentSpeed { get; private set; }
        public IdleState idleState { get; private set; }
        public WalkState walkState { get; private set; }
        public SprintState sprintState { get; private set; }
        public CrouchState crouchState { get; private set; }
        public JumpingState jumpingState { get; private set; }
        public FallState fallState { get; private set; }
        public bool isSprinting { get; private set; }
        public bool isCrouching { get; private set; }
        public bool isGrounded { get; private set; }
        private float ungroundTime;
        [SerializeField] private LayerMask walkableLayerMask;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = defaultGravityScale;
            stateMachine = new StateMachine();
        }

        private void Start()
        {
            walkState = new WalkState(this);
            idleState = new IdleState(this);
            sprintState = new SprintState(this);
            crouchState = new CrouchState(this);
            jumpingState = new JumpingState(this);
            fallState = new FallState(this);

            stateMachine.AddAnyTransition(idleState, new FuncPredicate(ReturnToIdle));
            stateMachine.AddTransition(idleState, walkState,
                new FuncPredicate(() => IsMoving() && isGrounded));
            stateMachine.AddTransition(walkState, sprintState, new FuncPredicate(() => isSprinting && IsMoving()));
            stateMachine.AddTransition(idleState, sprintState, new FuncPredicate(() => isSprinting && IsMoving()));
            stateMachine.AddTransition(sprintState, walkState, new FuncPredicate(() => !isSprinting && IsMoving()));
            stateMachine.AddTransition(idleState, crouchState, new FuncPredicate(() => IsMoving() && isCrouching));
            stateMachine.AddTransition(walkState, crouchState, new FuncPredicate(() => IsMoving() && isCrouching));
            stateMachine.AddTransition(crouchState, walkState, new FuncPredicate(() => IsMoving() && !isCrouching));
            stateMachine.AddTransition(jumpingState, idleState, new FuncPredicate(() => isGrounded));
            stateMachine.AddTransition(fallState, idleState, new FuncPredicate(() => isGrounded));
            stateMachine.AddAnyTransition(jumpingState, new FuncPredicate(IsJumping));
            stateMachine.AddAnyTransition(fallState, new FuncPredicate(IsFalling));

            stateMachine.SetState(idleState);
        }

        private bool ReturnToIdle()
        {
            return !IsMoving() && isGrounded;
        }

        private bool IsMoving()
        {
            return Mathf.Abs(moveInput.x) > moveSpeedThreshold;
        }

        public void SetCurrentSpeed(float speed)
        {
            currentSpeed = speed;
        }

        public void StartSprint()
        {
            isSprinting = true;
        }

        public void StopSprint()
        {
            isSprinting = false;
        }

        public void StartCrouch()
        {
            isCrouching = true;
        }

        public void StopCrouch()
        {
            isCrouching = false;
        }

        private void Update()
        {
            stateMachine.Update();
            bool isGroundHit = Physics2D.Raycast(transform.position, Vector2.down, raycastGroundDistance,
                walkableLayerMask);
            if (isGroundHit)
            {
                isGrounded = true;
                ungroundTime = Time.time;
            }
            else if (Time.time > ungroundTime + coyoteTime)
            {
                isGrounded = false;
            }
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public bool IsJumping()
        {
            return rb.linearVelocity.y > 0.1f;
        }

        public bool IsFalling()
        {
            return rb.linearVelocity.y < -0.1f;
        }

        public bool TryStartJump()
        {
            if (isGrounded && !IsJumping())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Stop vertical velocity
                rb.AddForce(Vector2.up * jumpForce);
                rb.gravityScale = jumpGravityScale;
                return true;
            }

            return false;
        }

        public bool TryStopJump()
        {
            if (IsJumping())
            {
                rb.gravityScale = defaultGravityScale;
                return true;
            }

            return false;
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
    }
}