using System.Collections.Generic;
using Hitable;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Collider2D hitBox;
        [SerializeField] private float knockbackForce;
        [SerializeField] private float attackRange;
        public bool isAttacking { get; private set; }
        private List<Collider2D> collidersDamaged = new();
        public int comboCount { get; private set; }

        public void Attack()
        {
            isAttacking = true;
            collidersDamaged.Clear();
            Collider2D[] collidersToDamage = new Collider2D[10];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            int colliderCount = Physics2D.OverlapCollider(hitBox, filter, collidersToDamage);
            for (int i = 0; i < colliderCount; i++)
            {
                if (!collidersDamaged.Contains(collidersToDamage[i]))
                {
                    IHitable hitable = collidersToDamage[i].GetComponentInChildren<IHitable>();

                    // Only check colliders with a valid Team Componnent attached
                    if (hitable != null)
                    {
                        collidersDamaged.Add(collidersToDamage[i]);
                        hitable.Hit(0, (hitable.rb.transform.position - transform.position).normalized, knockbackForce);
                    }
                }
            }
            
            comboCount++;
        }
        
        

        public void ResetAttack()
        {
            isAttacking = false;
            comboCount = 0;
        }

        public void SetAttackDirection(Vector2 attackInput)
        {
            if (attackInput != Vector2.zero)
            {
                hitBox.transform.localPosition = attackInput * attackRange;
            }
            else
            {
                hitBox.transform.position = transform.position + transform.right;
            }
        }
    }
}