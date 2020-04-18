using UnityEngine;

namespace LD46.Game.Characters
{
    public class Health : MonoBehaviour
    {
        private int _health;
        public const int MaxHealth = 100;

        public float Percentage => _health / (float)MaxHealth;

        public delegate void Damaged(int damage, Vector3 position, Vector3 velocity);

        public event Damaged OnDamaged;
        public event Damaged OnDeath;

        public bool Alive => _health > 0;
        public bool Dead => !Alive;

        void Start()
        {
            _health = MaxHealth;
        }

        public void Deal(int damage, Vector3 position, Vector3 velocity)
        {
            if (Dead) return;

            _health -= damage;
            _health = Mathf.Clamp(_health, 0, MaxHealth);

            OnDamaged?.Invoke(damage, position, velocity);

            if (Dead)
            {
                OnDeath?.Invoke(damage, position, velocity);
            }
        }
    }
}