using SimpleMan.Extensions.UnityEditor;
using UnityEngine;


namespace SimpleMan.EventSystem.Demo
{
    public class EventSender : MonoBehaviour
    {
        public GameEvent gameEvent;

        [Button]
        public void InvokeGameEvent()
        {
            gameEvent.Invoke();
        }
    }
}