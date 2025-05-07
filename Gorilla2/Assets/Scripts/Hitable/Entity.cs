using System;
using UnityEngine;

namespace Hitable
{
    public class Entity : MonoBehaviour, IHitable
    {
        public Rigidbody2D rb { get; protected set; }
        public bool isAlive { get; protected set;}
        public bool isInvincible { get; protected set;}
        public float health { get; protected set;}
        [field:SerializeField] public float maxHealth { get; private set;}


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            health = maxHealth;
        }


        public void Hit(float damage, Vector3 direction, float force)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            health = Mathf.Max(0, health - damage);
            if (health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Debug.Log("die");
        }
    }
}