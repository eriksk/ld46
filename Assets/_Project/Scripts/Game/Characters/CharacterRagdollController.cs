
using System;
using System.Linq;
using UnityEngine;

namespace LD46.Game.Characters
{
    public class CharacterRagdollController : MonoBehaviour
    {
        [Header("In Control")]
        public float InControlDrag = 2f;
        public float InControlAngularDrag = 2f;

        [Header("Release Control")]
        public float ReleaseControlDrag = 0f;
        public float ReleaseControlAngularDrag = 0.05f;

        private Collider[] _colliders;
        private Rigidbody[] _rigidbodies;
        private CharacterRagdollControllerState _state = CharacterRagdollControllerState.InControl;

        public void Start()
        {
            _colliders = transform.ChildrenDeep()
                .Select(x => x.GetComponent<Collider>())
                .Where(x => x != null)
                .ToArray();

            _rigidbodies = transform.ChildrenDeep()
                .Select(x => x.GetComponent<Rigidbody>())
                .Where(x => x != null)
                .ToArray();

            SetState(CharacterRagdollControllerState.InControl);
        }

        private void SetState(CharacterRagdollControllerState state)
        {
            _state = state;

            if (_state == CharacterRagdollControllerState.InControl)
            {
                SetDrag(InControlDrag, InControlAngularDrag);
            }
            else
            {
                SetDrag(ReleaseControlDrag, ReleaseControlAngularDrag);
            }
        }

        private void SetDrag(float drag, float angularDrag)
        {
            foreach (var rb in _rigidbodies)
            {
                rb.drag = drag;
                rb.angularDrag = angularDrag;
            }
        }

        void Update()
        {
        }
    }

    public enum CharacterRagdollControllerState
    {
        InControl,
        Released
    }
}