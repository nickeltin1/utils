using System;

namespace Characters
{
    public class State
    {
        public Enum Type { get; }
        
        public Action OnStateEnd { get; }
        public Action OnUpdate { get; }
        public Action OnFixedUpdate { get; }
        public Action OnStateStart { get; }

        public State(Enum type,  Action onStateStart = null, Action onUpdate = null, Action onFixedUpdate = null, 
            Action onStateEnd = null)
        {
            Type = type;
            OnStateStart = onStateStart;
            OnUpdate = onUpdate;
            OnFixedUpdate = onFixedUpdate;
            OnStateEnd = onStateEnd;
        }
    }
}