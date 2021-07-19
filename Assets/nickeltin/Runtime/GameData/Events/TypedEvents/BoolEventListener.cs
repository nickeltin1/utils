using nickeltin.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Runtime.GameData.Events
{
    public sealed class BoolEventListener : EventListener<bool>
    {
        private const string SPECIAL_EVENTS_NAME = "Specail Events";
        
        [SerializeField, Foldout(SPECIAL_EVENTS_NAME)] private UnityEvent _onTure;
        [SerializeField, Foldout(SPECIAL_EVENTS_NAME)] private UnityEvent _onFalse;
        
        public override void OnInvoke(bool data)
        {
            base.OnInvoke(data);
            if(data) _onTure.Invoke();
            else _onFalse.Invoke();
        }
    }
}