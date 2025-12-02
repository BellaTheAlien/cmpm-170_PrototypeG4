using System;
using UnityEditor;
using UnityEngine;

namespace ithappy.Animals_FREE
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class CreatureMover : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float m_WalkSpeed = 1f;
        [SerializeField] private float m_RunSpeed = 4f;

        [Header("Rotation")]
        [SerializeField, Range(0f, 360f)] private float m_RotateSpeed = 90f;
        [SerializeField] private float m_TurnAcceleration = 6f;
        [SerializeField] private float m_TurnDeceleration = 10f;

        [SerializeField] private Space m_Space = Space.Self;
        [SerializeField] private float m_JumpHeight = 0.8f;

        [Header("Gravity")]
        [SerializeField] private float m_FallMultiplier = 2.5f;
        [SerializeField] private float m_TerminalVelocity = 50f;

        [Header("Animator")]
        [SerializeField] private string m_VerticalID = "Vert";
        [SerializeField] private string m_StateID = "State";
        [SerializeField] private LookWeight m_LookWeight = new(1f, 0.3f, 0.7f, 1f);

        private Transform m_Transform;
        private CharacterController m_Controller;
        private Animator m_Animator;

        private MovementHandler m_Movement;
        private AnimationHandler m_Animation;

        private Vector2 m_Axis;
        private Vector3 m_Target;
        private bool m_IsRun;
        private bool m_IsJump;
        private bool m_IsMoving;

        public Vector2 Axis => m_Axis;
        public Vector3 Target => m_Target;
        public bool IsRun => m_IsRun;

        private void OnValidate()
        {
            m_WalkSpeed = Mathf.Max(m_WalkSpeed, 0f);
            m_RunSpeed = Mathf.Max(m_RunSpeed, m_WalkSpeed);
            m_JumpHeight = Mathf.Clamp(m_JumpHeight, 0f, 1.5f);

            m_Movement?.SetStats(
                m_WalkSpeed / 3.6f,
                m_RunSpeed / 3.6f,
                m_RotateSpeed,
                m_JumpHeight,
                m_Space,
                m_FallMultiplier,
                m_TerminalVelocity,
                m_TurnAcceleration,
                m_TurnDeceleration
            );
        }

        private void Awake()
        {
            m_Transform = transform;
            m_Controller = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();

            m_Movement = new MovementHandler(
                m_Controller,
                m_Transform,
                m_WalkSpeed,
                m_RunSpeed,
                m_RotateSpeed,
                m_JumpHeight,
                m_Space,
                m_FallMultiplier,
                m_TerminalVelocity,
                m_TurnAcceleration,
                m_TurnDeceleration
            );

            m_Animation = new AnimationHandler(m_Animator, m_VerticalID, m_StateID);
        }

        private void Update()
        {
            m_Movement.Move(
                Time.deltaTime,
                in m_Axis,
                in m_Target,
                m_IsRun,
                m_IsMoving,
                m_IsJump,
                out var animAxis,
                out var isAir
            );

            m_Animation.Animate(in animAxis, m_IsRun ? 1f : 0f, Time.deltaTime);
        }

        private void OnAnimatorIK()
        {
            m_Animation.AnimateIK(in m_Target, in m_LookWeight);
        }

        public void SetInput(in Vector2 axis, in Vector3 target, in bool isRun, in bool isJump)
        {
            m_Axis = axis;
            m_Target = target;
            m_IsRun = isRun;
            m_IsJump = isJump;

            if (m_Axis.sqrMagnitude < Mathf.Epsilon)
            {
                m_Axis = Vector2.zero;
                m_IsMoving = false;
            }
            else
            {
                m_Axis = Vector2.ClampMagnitude(m_Axis, 1f);
                m_IsMoving = true;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.normal.y > m_Controller.stepOffset)
                m_Movement.SetSurface(hit.normal);
        }

        [Serializable]
        private struct LookWeight
        {
            public float weight, body, head, eyes;
            public LookWeight(float weight, float body, float head, float eyes)
            {
                this.weight = weight;
                this.body = body;
                this.head = head;
                this.eyes = eyes;
            }
        }

        #region Handlers
        private class MovementHandler
        {
            private readonly CharacterController m_Controller;
            private readonly Transform m_Transform;

            private float m_WalkSpeed;
            private float m_RunSpeed;
            private float m_RotateSpeed;
            private float m_TurnAcceleration;
            private float m_TurnDeceleration;
            private float m_JumpHeight;

            private Space m_Space;

            private Vector3 m_Normal;
            private float m_VerticalVelocity = 0f;
            private float m_FallMultiplier = 2.5f;
            private float m_TerminalVelocity = 50f;

            private Vector3 m_SmoothedMovement = Vector3.zero;
            private float m_Acceleration = 8f;
            private float m_Deceleration = 10f;

            private float m_CurrentTurnSpeed = 0f;

            public MovementHandler(CharacterController controller, Transform transform,
                float walkSpeed, float runSpeed, float rotateSpeed, float jumpHeight,
                Space space, float fallMultiplier, float terminalVelocity,
                float turnAccel, float turnDecel)
            {
                m_Controller = controller;
                m_Transform = transform;

                m_WalkSpeed = walkSpeed;
                m_RunSpeed = runSpeed;
                m_RotateSpeed = rotateSpeed;
                m_JumpHeight = jumpHeight;

                m_Space = space;
                m_FallMultiplier = fallMultiplier;
                m_TerminalVelocity = Mathf.Abs(terminalVelocity);

                m_TurnAcceleration = turnAccel;
                m_TurnDeceleration = turnDecel;
            }

            public void SetStats(float walkSpeed, float runSpeed, float rotateSpeed,
                float jumpHeight, Space space, float fallMultiplier,
                float terminalVelocity, float turnAccel, float turnDecel)
            {
                m_WalkSpeed = walkSpeed;
                m_RunSpeed = runSpeed;
                m_RotateSpeed = rotateSpeed;
                m_JumpHeight = jumpHeight;
                m_Space = space;
                m_FallMultiplier = fallMultiplier;
                m_TerminalVelocity = Mathf.Abs(terminalVelocity);
                m_TurnAcceleration = turnAccel;
                m_TurnDeceleration = turnDecel;
            }

            public void SetSurface(in Vector3 normal)
            {
                m_Normal = normal;
            }

            public void Move(float deltaTime, in Vector2 axis, in Vector3 target,
                bool isRun, bool isMoving, bool isJump,
                out Vector2 animAxis, out bool isAir)
            {
                ConvertMovement(in axis, out var rawMovement);

                if (rawMovement.sqrMagnitude > 0.01f)
                    m_SmoothedMovement = Vector3.Lerp(m_SmoothedMovement, rawMovement, m_Acceleration * deltaTime);
                else
                    m_SmoothedMovement = Vector3.Lerp(m_SmoothedMovement, Vector3.zero, m_Deceleration * deltaTime);

                TurnTowardMovementSmooth(m_SmoothedMovement, deltaTime);

                CaculateGravity(deltaTime, out isAir);

                if (isJump && m_Controller.isGrounded)
                    m_VerticalVelocity = Mathf.Sqrt(-2f * Physics.gravity.y * m_JumpHeight);

                Displace(deltaTime, in m_SmoothedMovement, isRun);

                GenAnimationAxis(in m_SmoothedMovement, out animAxis);
            }

            private void ConvertMovement(in Vector2 axis, out Vector3 movement)
            {
                Vector3 forward = m_Transform.forward;
                Vector3 right = m_Transform.right;

                forward.y = 0;
                right.y = 0;

                forward.Normalize();
                right.Normalize();

                movement = axis.x * right + axis.y * forward;
            }

            private void TurnTowardMovementSmooth(Vector3 movementDirection, float deltaTime)
            {
                if (movementDirection.sqrMagnitude < 0.001f)
                {
                    m_CurrentTurnSpeed = Mathf.Lerp(m_CurrentTurnSpeed, 0f, m_TurnDeceleration * deltaTime);
                    return;
                }

                float forwardAmount = Vector3.Dot(movementDirection.normalized, m_Transform.forward);

                if (forwardAmount < -0.6f)
                    return;

                m_CurrentTurnSpeed = Mathf.Lerp(m_CurrentTurnSpeed, m_RotateSpeed, m_TurnAcceleration * deltaTime);

                Quaternion targetRot = Quaternion.LookRotation(new Vector3(
                    movementDirection.x, 0f, movementDirection.z));

                m_Transform.rotation = Quaternion.RotateTowards(
                    m_Transform.rotation,
                    targetRot,
                    m_CurrentTurnSpeed * deltaTime
                );
            }

            private void Displace(float deltaTime, in Vector3 movement, bool isRun)
            {
                float speed = isRun ? m_RunSpeed : m_WalkSpeed;
                Vector3 horizontal = new Vector3(movement.x, 0f, movement.z);
                Vector3 displacement = speed * horizontal * deltaTime;

                displacement += Vector3.up * (m_VerticalVelocity * deltaTime);
                m_Controller.Move(displacement);
            }

            private void CaculateGravity(float deltaTime, out bool isAir)
            {
                if (m_Controller.isGrounded)
                {
                    if (m_VerticalVelocity < 0f)
                        m_VerticalVelocity = -2f;
                    isAir = false;
                    return;
                }

                isAir = true;

                if (m_VerticalVelocity > 0f)
                    m_VerticalVelocity += Physics.gravity.y * deltaTime;
                else
                    m_VerticalVelocity += Physics.gravity.y * m_FallMultiplier * deltaTime;

                m_VerticalVelocity = Mathf.Max(m_VerticalVelocity, -Mathf.Abs(m_TerminalVelocity));
            }

            private void GenAnimationAxis(in Vector3 movement, out Vector2 animAxis)
            {
                animAxis = new Vector2(
                    Vector3.Dot(movement, m_Transform.right),
                    Vector3.Dot(movement, m_Transform.forward)
                );
            }
        }

        private class AnimationHandler
        {
            private readonly Animator m_Animator;
            private readonly string m_VerticalID;
            private readonly string m_StateID;

            private readonly float k_InputFlow = 4.5f;
            private float m_FlowState;
            private Vector2 m_FlowAxis;

            public AnimationHandler(Animator animator, string verticalID, string stateID)
            {
                m_Animator = animator;
                m_VerticalID = verticalID;
                m_StateID = stateID;
            }

            public void Animate(in Vector2 axis, float state, float deltaTime)
            {
                m_Animator.SetFloat(m_VerticalID, m_FlowAxis.magnitude);
                m_Animator.SetFloat(m_StateID, Mathf.Clamp01(m_FlowState));

                m_FlowAxis = Vector2.ClampMagnitude(
                    m_FlowAxis + k_InputFlow * deltaTime * (axis - m_FlowAxis).normalized,
                    1f
                );

                m_FlowState = Mathf.Clamp01(
                    m_FlowState + k_InputFlow * deltaTime * Mathf.Sign(state - m_FlowState)
                );
            }

            public void AnimateIK(in Vector3 target, in LookWeight lookWeight)
            {
                m_Animator.SetLookAtPosition(target);
                m_Animator.SetLookAtWeight(
                    lookWeight.weight,
                    lookWeight.body,
                    lookWeight.head,
                    lookWeight.eyes
                );
            }
        }
        #endregion
    }
}
