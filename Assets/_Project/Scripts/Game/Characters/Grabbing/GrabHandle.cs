using System.Linq;
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

        private Transform[] _parentHierarchy;

        void Start()
        {
            _parentHierarchy = transform.root.ChildrenDeep().ToArray();
        }

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

                if(_parentHierarchy.Contains(rb.transform)) continue;

                var direction = (rb.position - transform.position).normalized;

                RaycastHit hit;
                if(_cache[i].Raycast(new Ray(transform.position, direction), out hit, GrabRange))
                {
                    Grab(rb, hit.point);
                    return true;
                }
            }

            for(var i = 0; i < _cache.Length; i++)
            {
                _cache[i] = null;
            }

            return false;
        }

        private void Grab(Rigidbody target, Vector3 connectionPoint)
        {
            // var directionToTarget = (target.position - transform.position).normalized;

            GrabbedRigidbody = target;
            target.velocity = Vector3.zero;
            target.angularVelocity = Vector3.zero;
            GrabJoint = gameObject.AddComponent<FixedJoint>();
            GrabJoint.autoConfigureConnectedAnchor = false;
            GrabJoint.connectedBody = target;
            GrabJoint.connectedAnchor = target.transform.InverseTransformPoint(connectionPoint);
            GrabJoint.anchor = Vector3.zero;
            GrabJoint.breakForce = 450f;
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

        void OnJointBreak(float breakForce)
        {
            Release();
        }
    }
}