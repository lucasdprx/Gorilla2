using System;
using System.Collections;
using UnityEngine;

namespace Hitable
{
    public class Entity : MonoBehaviour, IHitable
    {
        public Rigidbody2D rb { get; protected set; }
        public bool isAlive { get; protected set;}
        public bool isInvincible { get; protected set;}
        public bool stunned { get; protected set; }
        public float health { get; protected set;}
        [field:SerializeField] public float maxHealth { get; private set;}
        public event Action onHit;
        private Coroutine stunCoroutine;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            health = maxHealth;
        }


        public void Hit(float damage, Vector3 direction, float force, bool stun = false, float stunTime = 0)
        {
            if (stun)
            {
                stunned = true;
                if (stunCoroutine != null)
                {
                    StopCoroutine(stunCoroutine);
                }
                stunCoroutine = StartCoroutine(ResetStun(stunTime));
            }
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            health = Mathf.Max(0, health - damage);
            onHit?.Invoke();
            if (health <= 0)
            {
                Die();
            }
        }

        private IEnumerator ResetStun(float stunTime)
        {
            yield return new WaitForSeconds(stunTime);
            stunned = false;
        }

        public void Die()
        {
            Debug.Log("die");
        }
    }
}