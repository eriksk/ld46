using UnityEngine;

namespace LD46.Game.Characters
{
    public abstract class InputController : ScriptableObject
    {
        internal abstract void OnUpdate(CharacterMovementController controller);
    }
}