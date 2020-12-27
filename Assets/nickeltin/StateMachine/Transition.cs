using System;

namespace nickeltin.StateMachine
{
    public sealed class Transition
    {
        public readonly StateMachine.UpdateType conditionValidationMode;
        public readonly Enum transitionTo;
        public readonly Func<bool> condition;

        public Transition(StateMachine.UpdateType conditionValidationMode, Enum transitionTo, Func<bool> condition)
        {
            this.conditionValidationMode = conditionValidationMode;
            this.transitionTo = transitionTo;
            this.condition = condition;
        }
    }
}