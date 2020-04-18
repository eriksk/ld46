
using UnityEngine;

namespace LD46.Game.Characters
{
    [CreateAssetMenu(menuName = "Custom/Input/Player")]
    public class PlayerInputController : InputController
    {
        internal override void OnUpdate(CharacterMovementController controller)
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

            controller.UpdateInput(
                movement, 
                Input.GetButtonDown("Jump"),
                Input.GetMouseButton(0),
                Input.GetMouseButton(1));
        }
    }
}