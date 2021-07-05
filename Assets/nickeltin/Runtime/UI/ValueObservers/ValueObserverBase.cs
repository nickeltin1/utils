using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Runtime.UI
{
    public abstract class ValueObserverBase : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onValueChanged;

        protected void InvokeUnityEvent() => _onValueChanged.Invoke();
    }
}