using System;
using System.Collections.Generic;

namespace nickeltin.StateMachine
{
    public class State
    {
        public enum Type { NotAssigned, Main, Disabled, Enabled }
        
        public readonly Enum type;

        private readonly Func<bool> m_onStateStart;
        private readonly Func<bool> m_onUpdate;
        private readonly Func<bool> m_onFixedUpdate;
        private readonly Func<bool> m_onStateEnd;

        public readonly List<Transition> transitions;
        
        public State(Enum type) => this.type = type;

        public State(Enum type, Func<bool> onStateStart = null, Func<bool> onUpdate = null, Func<bool> onFixedUpdate = null, 
            Func<bool> onStateEnd = null, params Transition[] transitions) : this(type)
        {
            m_onStateStart = onStateStart;
            m_onUpdate = onUpdate;
            m_onFixedUpdate = onFixedUpdate;
            m_onStateEnd = onStateEnd;
            this.transitions = new List<Transition>(transitions); 
        }
        
        public virtual bool OnStateStart() => ExecuteAction(m_onStateStart);
        public virtual bool OnUpdate() => ExecuteAction(m_onUpdate);
        public virtual bool OnFixedUpdate() => ExecuteAction(m_onFixedUpdate);
        public virtual bool OnStateEnd() => ExecuteAction(m_onStateEnd);
        
        private static bool ExecuteAction(Func<bool> action)
        {
            if (action != null) return action();
            return false;
        }
    }
}