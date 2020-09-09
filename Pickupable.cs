using Interfaces;
using Other;
using UnityEngine;

namespace Items
{    
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Pickupable : TriggerArea, IDestructable
    {
        [Header("Components")] 
        [SerializeField] private SpriteRenderer Renderer; 
        
        public Item item;
        public int quantity = 1;

        private Rigidbody2D Rigidbody;
                    

        private void Start()
        {
            Renderer.sprite = item.GetSprite();
        }

        
        protected override void Awake()
        {
            base.Awake();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void AddForce(Vector2 dir, float force)
        {
            Rigidbody.AddForce(dir * force, ForceMode2D.Impulse);
        }
        
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}