﻿using System;

namespace nickeltin.StateMachine
{
    public class Transition
    {
        public readonly StateMachine.UpdateType conditionValidationMode;
        public readonly Enum transitionTo;
        
        private readonly Func<bool> m_condition;

        public Transition(StateMachine.UpdateType conditionValidationMode, Enum transitionTo, Func<bool> condition)
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