using System;

namespace nickeltin.Runtime.StateMachine
{
    public class Transition
    {
        public readonly UpdateType conditionValidationMode;
        public readonly Enum transitionTo;
        
        private readonly Func<bool> _condition;

        public Transition(UpdateType conditionValidationMode, Enum transitionTo, Func<bool> condition)
        {
            this.conditionValidationMode = conditionValidationMode;
            this.transitionTo = transitionTo;
            this._condition = condition;
        }
        
        public virtual bool Condition()
        {
            if (_condition != null) return _condition();
            return false;
        }
    }
}