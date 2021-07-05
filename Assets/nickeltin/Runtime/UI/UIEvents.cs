using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace nickeltin.Runtime.UI
{
    [RequireComponent(typeof(Graphics))]
    public class UIEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private UnityEvent<PointerEventData> m_onPointerDown;
        [SerializeField] private UnityEvent<PointerEventData> m_onPointerUp;
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            m_onPointerDown.Invoke(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            m_onPointerUp.Invoke(eventData);
        }
    }
}