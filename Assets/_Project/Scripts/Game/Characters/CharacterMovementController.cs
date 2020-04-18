using System;
using LD46.Game.Characters.Grabbing;
using UnityEngine;

namespace LD46.Game.Characters
{
    public class CharacterMovementController : MonoBehaviour
    {
        [Header("References")]
        public Rigidbody ControllerRigidbody;
        public Rigidbody Hinge;
        public Transform TargetsRoot;
        public Animation Animator;

        public Transform LeftHandEffector;
        public Transform RightHandEffector;

        public Transform LeftHandClickTarget;
        public Transform RightHandClickTarget;

        [Header("Movement")]
        public float MovementSpeed = 1f;
        public float MovementDamping = 1f;
        public float RotationSpeed = 1f;
        public float JumpForce = 5f;
        public float ExtraGravity = 30f;

        [Header("Input")]
        public InputController InputController;

        [Header("Grabbing")]
        public GrabHandle LeftHandGrabHandle;
        public GrabHandle RightHandGrabHandle;

        private Vector3 _movement;
        private string _currentAnimation;
        private bool _leftHandUse, _rightHandUse;

        private void PlayAnim(string name)
        {
            if (_currentAnimation == name) return;
            _currentAnimation = name;

            if (_currentAnimation == "swing")
            {
                return;
            }
            Animator.CrossFade(_currentAnimation, 0.3f);
        }

        public void UpdateInput(Vector3 movement, bool jump, bool leftHandUse, bool rightHandUse)
        {
            if (movement.magnitude > 0.3f)
            {
                _movement = movement.normalized * Mathf.Clamp01(movement.magnitude);
            }
            else
            {
                _movement = Vector3.zero;
            }

            if (jump)
            {
                Jump();
            }

            _leftHandUse = leftHandUse;
            _rightHandUse = rightHandUse;
        }

        private void Jump()
        {
            var velocity = ControllerRigidbody.velocity;
            velocity.y = 0f;
            ControllerRigidbody.velocity = velocity;
            ControllerRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        void Update()
        {
            InputController.OnUpdate(this);

            if (_movement.magnitude > 0.3f)
            {
                var movementDirection = _movement.normalized;
                var lookRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                TargetsRoot.rotation = Quaternion.Slerp(
                    TargetsRoot.rotation,
                    lookRotation,
                    RotationSpeed * Time.deltaTime
                );

                PlayAnim("walk");
            }
            else
            {
                PlayAnim("idle");
            }
        }

        void LateUpdate()
        {
            if (_leftHandUse)
            {
                LeftHandEffector.position = LeftHandClickTarget.position;
                LeftHandGrabHandle.TryGrab();
            }
            else
            {
                LeftHandGrabHandle.Release();
            }
            if (_rightHandUse)
            {
                RightHandEffector.position = RightHandClickTarget.position;
                RightHandGrabHandle.TryGrab();
            }
            else
            {
                RightHandGrabHandle.Release();
            }
        }

        void FixedUpdate()
        {
            if (_movement.magnitude > 0.3f)
            {
                ControllerRigidbody.AddForce(_movement.normalized * MovementSpeed);
            }

            var flatVelocity = ControllerRigidbody.velocity;
            flatVelocity.y = 0f;
            ControllerRigidbody.AddForce(-flatVelocity * MovementDamping);
            ControllerRigidbody.AddForce(Vector3.down * ExtraGravity);
        }
    }
}