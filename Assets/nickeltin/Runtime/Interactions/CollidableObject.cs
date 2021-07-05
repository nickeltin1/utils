using System;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Runtime.Interactions
{
   public class CollidableObject : MonoBehaviour
   {
      [SerializeField] private UnityEvent<Collider> _onTriggerEnter;
      public event Action<Collider> onTriggerEnter;
      
      [SerializeField] private UnityEvent<Collision> m_onCollisionEnter;
      public event Action<Collision> onCollisionEnter;

      private void OnTriggerEnter(Collider other)
      {
         onTriggerEnter?.Invoke(other);
         _onTriggerEnter.Invoke(other);
      }

      private void OnCollisionEnter(Collision other)
      {
         onCollisionEnter?.Invoke(other);
         m_onCollisionEnter.Invoke(other);
      }
   }
}
