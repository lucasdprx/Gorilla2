using UnityEngine;

namespace Hitable
{
    public class Dummy : Entity
    {
        private Collider2D dummyCollider;
        private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            dummyCollider = GetComponent<Collider2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public override void Die()
        {
            base.Die();
            Debug.Log($"Dummy {name} broke");
            spriteRenderer.gameObject.SetActive(false);
            dummyCollider.enabled = false;
        }
    }
}