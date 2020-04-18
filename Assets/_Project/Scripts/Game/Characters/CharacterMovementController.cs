using System;
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

        [Header("Movement")]
        public float MovementSpeed = 1f;
        public float MovementDamping = 1f;
        public float RotationSpeed = 1f;
        public float JumpForce = 5f;
        public float ExtraGravity = 30f;

        private Vector3 _movement;
        private string _currentAnimation;

        private void PlayAnim(string name)
        {
            if(_currentAnimation == name) return;
            _currentAnimation = name;
            Animator.CrossFade(_currentAnimation, 0.3f);
        }

        void UpdateInput(Vector3 movement, bool jump)
        {
            if (movement.magnitude > 0.3f)
            {
                _movement = movement.normalized * Mathf.Clamp01(movement.magnitude);
            }
            else
            {
                _movement = Vector3.zero;
            }

            if(jump)
            {
                Jump();
            }
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
            var stick = new Vector3(
                Input.GetAxis("Horizontal"),
                0f,
                Input.GetAxis("Vertical"));
            var movement = Camera.main.transform.TransformDirection(stick);
            movement.y = 0f;
            if (stick.magnitude > 0.3f)
            {
                movement = movement.normalized * Mathf.Clamp01(stick.magnitude);
            }
            else
            {
                movement = Vector3.zero;
            }
            UpdateInput(movement, Input.GetButtonDown("Jump"));

            if(_movement.magnitude > 0.3f)
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