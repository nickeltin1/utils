using System;
using System.Collections.Generic;

namespace nickeltin.StateMachine
{
    public abstract class StateBase
    {
        [Serializable]
        public enum Type { NotAssigned, Main, Disabled, Enabled }
        
        protected Func<bool> m_onStateStart;
        protected Func<bool> m_onUpdate;
        protected Func<bool> m_onFixedUpdate;
        protected Func<bool> m_onStateEnd;

        protected Action m_onGizmosDraw; 

        protected List<Transition> m_transitions;

        public IReadOnlyList<Transition> transitions => m_transitions;

        public abstract Enum implicitType { get; }

        public StateBase() { }

        /// <summary>
        /// Use this before passing to State Machine
        /// </summary>
        public void Override(Func<bool> onStateStart = null, Func<bool> onUpdate = null, Func<bool> onFixedUpdate = null, 
            Func<bool> onStateEnd = null)
        {
            m_onStateStart = onStateStart ?? m_onStateStart;
            m_onUpdate = onUpdate ?? m_onUpdate;
            m_onFixedUpdate = onFixedUpdate ?? m_onFixedUpdate;
            m_onStateEnd = onStateEnd ?? m_onStateEnd;
        }

        public void AddGizmos(Action onGizmosDraw) => m_onGizmosDraw = onGizmosDraw;

        public virtual bool OnStateStart() => ExecuteAction(m_onStateStart);
        public virtual bool OnUpdate() => ExecuteAction(m_onUpdate);
        public virtual bool OnFixedUpdate() => ExecuteAction(m_onFixedUpdate);
        public virtual bool OnStateEnd() => ExecuteAction(m_onStateEnd);

        public virtual void OnGizmosDraw() => m_onGizmosDraw?.Invoke();
        
        private static bool ExecuteAction(Func<bool> action)
        {
            if (action != null) return action();
            return false;
        }
    }
}