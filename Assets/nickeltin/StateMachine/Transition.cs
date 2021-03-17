using System;

namespace nickeltin.StateMachine
{
    public class Transition
    {
        public readonly UpdateType conditionValidationMode;
        public readonly Enum transitionTo;
        
        private readonly Func<bool> m_condition;

        public Transition(UpdateType conditionValidationMode, Enum transitionTo, Func<bool> condition)
        {
            this.conditionValidationMode = conditionValidationMode;
            this.transitionTo = transitionTo;
            this.m_condition = condition;
        }
        
        public virtual bool Condition()
        {
            if (m_condition != null) return m_condition();
            return false;
        }
    }
}