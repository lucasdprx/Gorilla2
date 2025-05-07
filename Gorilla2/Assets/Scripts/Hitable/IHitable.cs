
using UnityEngine;

namespace Hitable
{
    public interface IHitable
    {
        Rigidbody2D rb { get; }
        bool isAlive { get; }
        bool isInvincible { get; }
        float health { get; }
        float maxHealth { get; }
        void Hit(float damage, Vector3 direction, float force);
        
        void Die();
    }
}