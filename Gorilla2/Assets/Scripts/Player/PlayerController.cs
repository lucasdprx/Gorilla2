using System;
using System.Collections.Generic;
using Platformer;
using Player.PlayerStates;
using UnityEngine;
using static Player.InputManager;

namespace Player
{
    [RequireComponent(typeof(PlayerAttack))]
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
        public MeleeState meleeState { get; private set; }
        public bool isGrounded { get; private set; }
        private float ungroundTime;
        [SerializeField] private LayerMask walkableLayerMask;
        private Collider2D playerCollider;
        [field: SerializeField] public float attackCooldown { get; private set; } = 0.5f;
        private Coroutine resetAttackCoroutine;
        [field: SerializeField] public float comboWindow { get; private set; }
        private PlayerAttack playerAttack;

        #region InputBools

        public Dictionary<InputActionType, bool> isInputsPressed { get; private set; }

        public void SetInputState(InputActionType actionType, bool state)
        {
            isInputsPressed[actionType] = state;
        }

        #endregion
        public event Action onJump;

        private void Awake()
        {
            playerAttack = GetComponent<PlayerAttack>();
            SetupInputs();
            playerCollider = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = defaultGravityScale;
            stateMachine = new StateMachine();
            walkState = new WalkState(this);
            idleState = new IdleState(this);
            sprintState = new SprintState(this);
            crouchState = new CrouchState(this);
            jumpingState = new JumpingState(this);
            meleeState = new MeleeState(this);
        }

        private void SetupInputs()
        {
            isInputsPressed ??= new Dictionary<InputActionType, bool>();
            foreach (InputActionType actionType in Enum.GetValues(typeof(InputActionType)))
            {
                isInputsPressed[actionType] = false;
            }
        }

        private void Start()
        {
            Any(idleState, new FuncPredicate(ReturnToIdle));
            At(idleState, walkState, new FuncPredicate(ShouldMove));
            At(walkState, sprintState, new FuncPredicate(() => isInputsPressed[InputActionType.Sprint]));
            At(sprintState, walkState, new FuncPredicate(() => !isInputsPressed[InputActionType.Sprint]));
            At(walkState, crouchState, new FuncPredicate(() => isInputsPressed[InputActionType.Crouch]));
            At(crouchState, walkState, new FuncPredicate(() => !isInputsPressed[InputActionType.Crouch]));
            Any(jumpingState, new FuncPredicate(() => !isGrounded && !playerAttack.isAttacking && !isInputsPressed[InputActionType.Attack]));
            Any(meleeState, new FuncPredicate(() => isInputsPressed[InputActionType.Attack]));
            At(meleeState, idleState, new FuncPredicate(() => !playerAttack.isAttacking));
            
            stateMachine.SetState(idleState);
        }

        private bool ReturnToIdle()
        {
            return !ShouldMove() && isGrounded && !playerAttack.isAttacking && !IsInAir() && !isInputsPressed[InputActionType.Attack];
        }

        private void At(BaseState from, BaseState to, IPredicate predicate)
        {
            stateMachine.AddTransition(from, to, predicate);
        }

        private void Any(BaseState to, IPredicate predicate)
        {
            stateMachine.AddAnyTransition(to, predicate);
        }

        private bool ShouldMove()
        {
            return Mathf.Abs(moveInput.x) > moveSpeedThreshold;
        }

        public void SetCurrentSpeed(float speed)
        {
            currentSpeed = speed;
        }

        private void Update()
        {
            stateMachine.Update();
            bool isGroundHit = Physics2D.CircleCast(transform.position, playerCollider.bounds.extents.x,
                Vector2.down, raycastGroundDistance, walkableLayerMask);
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

        private bool IsInAir()
        {
            return Mathf.Abs(rb.linearVelocity.y) > 0.1f;
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Stop vertical velocity
            rb.AddForce(Vector2.up * jumpForce);
            rb.gravityScale = jumpGravityScale;
            onJump?.Invoke();
        }

        public bool TryStartJump()
        {
            if (isGrounded && !IsInAir())
            {
                Jump();
                return true;
            }

            return false;
        }

        public bool TryStopJump()
        {
            if (IsInAir())
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
            
            if(moveInput != Vector2.zero)
                transform.right = moveInput;
        }

        public void Attack()
        {
            playerAttack.Attack();
            Debug.Log("Hit! with combo count: " + playerAttack.comboCount);
        }

        public void ResetAttack()
        {
            playerAttack.ResetAttack();
        }

        public void SetAttackDirection(Vector2 attackInput)
        {
            playerAttack.SetAttackDirection(attackInput);
        }
    }
}