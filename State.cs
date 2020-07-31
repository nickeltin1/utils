using System;
using UnityEngine;

namespace Characters
{
    public class State
    {
        public Enum Type { get; private set; }
        
        public Action OnStateEnd { get; private set; }
        public Action OnUpdate { get; private set; }
        public Action OnFixedUpdate { get; private set; }
        public Action OnStateStart { get; private set; }
        
        public State(Enum type, Action onStateEnd = null, Action onUpdate = null, Action onFixedUpdate = null, Action onStateStart = null)
        {
            Type = type;
            OnStateEnd = onStateEnd;
            OnUpdate = onUpdate;
            OnFixedUpdate = onFixedUpdate;
            OnStateStart = onStateStart;
        }
    }
}