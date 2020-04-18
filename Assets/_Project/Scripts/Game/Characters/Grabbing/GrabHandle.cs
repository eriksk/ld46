using UnityEngine;

namespace LD46.Game.Characters.Grabbing
{
    public class GrabHandle : MonoBehaviour
    {
        public LayerMask GrabbableMask;
        public float GrabRange = 1f;

        public bool GrabbedSomething;
        public Rigidbody GrabbedRigidbody;

        public FixedJoint GrabJoint;
        private static Collider[] _cache = new Collider[10];

        public bool TryGrab()
        {
            if(GrabbedSomething) return false;

            for(var i = 0; i < _cache.Length; i++)
            {
                _cache[i] = null;
            }

            var hits = Physics.OverlapSphereNonAlloc(
                transform.position,
                GrabRange,
                _cache,
                GrabbableMask, 
                QueryTriggerInteraction.Ignore);

            if(hits < 1) return false;

            for(var i = 0; i < hits; i++)
            {
                var rb = _cache[i].attachedRigidbody;
                if(rb == null) continue;

                Grab(rb);
                return true;
            }

            for(var i = 0; i < _cache.Length; i++)
            {
                _cache[i] = null;
            }

            return false;
        }

        private void Grab(Rigidbody target)
        {
            GrabbedRigidbody = target;
            GrabJoint = gameObject.AddComponent<FixedJoint>();
            GrabJoint.connectedBody = target;
            // Break force? 
            GrabbedSomething = true;
        }

        public void Release()
        {
            if(!GrabbedSomething) return;

            if(GrabJoint != null)
            {
                Destroy(GrabJoint);
                GrabJoint = null;
            }
            GrabbedRigidbody = null;
            GrabbedSomething = false;
        }
    }
}