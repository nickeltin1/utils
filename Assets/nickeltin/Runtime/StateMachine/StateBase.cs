using System;
using System.Collections.Generic;

namespace nickeltin.Runtime.StateMachine
{
    public abstract class StateBase
    {
        [Serializable]
        public enum Type { NotAssigned, Main, Disabled, Enabled }
        
        public Func<bool> onStateStart { get; protected set; }
        public Func<bool> onUpdate { get; protected set; }
        public Func<bool> onFixedUpdate { get; protected set; }
        public Func<bool> onStateEnd { get; protected set; }

        protected Action _onGizmosDraw; 

        protected List<Transition> _transitions;

        public IReadOnlyList<Transition> transitions => _transitions;

        public abstract Enum implicitType { get; }

        public StateBase() { }

        /// <summary>
        /// Use this before passing to State Machine
        /// </summary>
        public void Override(Func<bool> onStateStart = null, Func<bool> onUpdate = null, Func<bool> onFixedUpdate = null, 
            Func<bool> onStateEnd = null)
        {
            this.onStateStart = onStateStart ?? this.onStateStart;
            this.onUpdate = onUpdate ?? this.onUpdate;
            this.onFixedUpdate = onFixedUpdate ?? this.onFixedUpdate;
            this.onStateEnd = onStateEnd ?? this.onStateEnd;
        }

        public void AddGizmos(Action onGizmosDraw) => _onGizmosDraw = onGizmosDraw;

        public virtual bool OnStateStart() => ExecuteAction(onStateStart);
        public virtual bool OnUpdate() => ExecuteAction(onUpdate);
        public virtual bool OnFixedUpdate() => ExecuteAction(onFixedUpdate);
        public virtual bool OnStateEnd() => ExecuteAction(onStateEnd);

        public virtual void OnGizmosDraw() => _onGizmosDraw?.Invoke();
        
        private static bool ExecuteAction(Func<bool> action)
        {
            if (action != null) return action();
            return false;
        }
    }
}