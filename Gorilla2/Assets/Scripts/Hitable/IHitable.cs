
using System;
using UnityEngine;

namespace Hitable
{
    public interface IHitable
    {
        Rigidbody2D rb { get; }
        bool isAlive { get; }
        bool isInvincible { get; }
        bool stunned { get;  }
        float health { get; }
        float maxHealth { get; }
        int playerId { get; }
        event Action onHit;
        void Hit(float damage, Vector3 direction, float force, bool stun = false, float stunTime = 0);
        
        void Die();
    }
}