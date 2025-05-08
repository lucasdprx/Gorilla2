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
        [SerializeField] private float stunTime = 1f;
        [SerializeField] private float dashDistance = 2f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float bigDamage = 15f;

        public bool isAttacking { get; private set; }
        private List<Collider2D> collidersDamaged = new();
        public int comboCount { get; private set; }
        private Rigidbody2D rb;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Attack()
        {
            isAttacking = true;
            collidersDamaged.Clear();
            Collider2D[] collidersToDamage = new Collider2D[10];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            int colliderCount = Physics2D.OverlapCollider(hitBox, filter, collidersToDamage);
            if (comboCount == 2)
            {
                DashForward();
            }
            for (int i = 0; i < colliderCount; i++)
            {
                if (!collidersDamaged.Contains(collidersToDamage[i]))
                {
                    IHitable hitable = collidersToDamage[i].GetComponentInChildren<IHitable>();

                    // Only check colliders with a valid Team Componnent attached
                    if (hitable != null)
                    {
                        collidersDamaged.Add(collidersToDamage[i]);
                        if (comboCount == 2)
                        {
                            hitable.Hit(bigDamage, (hitable.rb.transform.position - transform.position).normalized,
                                knockbackForce, true, stunTime);
                        }
                        else
                        {
                            hitable.Hit(damage, (hitable.rb.transform.position - transform.position).normalized, 0);
                        }
                    }
                }
            }

            comboCount = (comboCount + 1) % 3;
        }

        private void DashForward()
        {
            Vector3 dashDirection = transform.TransformDirection(hitBox.transform.localPosition.normalized);
            Vector2 targetPosition = (Vector2)transform.position + (Vector2)dashDirection * dashDistance;
            rb.MovePosition(targetPosition);
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