using LD46.Game.Characters;
using UnityEngine;

namespace  LD46.Game.Weapons
{
    public class SharpnessWeapon : MonoBehaviour
    {
        public int Damage = 1;
        public Rigidbody Rigidbody;

        public void OnCollisionEnter(Collision collision)
        {
            var health = collision.transform.GetComponentInAnyParent<Health>();
            if(health == null) return;

            health.Deal(Damage, transform.position, Rigidbody == null ? Vector3.forward : Rigidbody.velocity);

            Debug.Log(gameObject.name +  " Damaged "  + health.gameObject.name);
        }
    }
}