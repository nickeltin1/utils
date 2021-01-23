using System;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Interactions
{
   [RequireComponent(typeof(Collider))]
   public class CollidableObject : MonoBehaviour
   {
      public MonoBehaviour owner;
      protected Collider m_collider;
      
      [SerializeField] private UnityEvent<Collider> m_onTriggerEnter;
      public event Action<Collider> onTriggerEnter;
      
      [SerializeField] private UnityEvent<Collision> m_onCollisionEnter;
      public event Action<Collision> onCollisionEnter;

      private void Awake() => m_collider = GetComponent<Collider>();

      protected virtual void OnTriggerEnter(Collider other)
      {
         onTriggerEnter?.Invoke(other);
         m_onTriggerEnter?.Invoke(other);
      }

      protected virtual void OnCollisionEnter(Collision other)
      {
         onCollisionEnter?.Invoke(other);
         m_onCollisionEnter?.Invoke(other);
         Debug.Log("collision");
      }

      public bool TryGetOwner<T>(out T owner) where T : MonoBehaviour
      {
         if (this.owner is T requestedObject)
         {
            owner = requestedObject;
            return true;
         }

         owner = null;
         return false;
      }
   }
}
