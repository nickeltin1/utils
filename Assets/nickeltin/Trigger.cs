using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
   [SerializeField] private UnityEvent<Collider> m_onEnter;
   protected Collider m_collider;

   private event Action<Collider> onEnter;

   private void Awake()
   {
      m_collider = GetComponent<Collider>();
      m_collider.isTrigger = true;
   }

   protected virtual void OnTriggerEnter(Collider other)
   {
      onEnter?.Invoke(other);
      m_onEnter?.Invoke(other);
   }
}
