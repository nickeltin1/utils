using System;
using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.StateMachine
{
    public sealed class State : State<Enum>
    {
        public State(Enum type) : base(type) { }

        public State(Enum type, Func<bool> onStateStart = null, Func<bool> onUpdate = null,
            Func<bool> onFixedUpdate = null, Func<bool> onStateEnd = null, params Transition[] transitions) :
            base(type, onStateStart, onUpdate, onFixedUpdate, onStateEnd, transitions)
        { }
    }
    
    /// <summary>
    /// Inherit from it if you want to create custom state
    /// </summary>
    /// <typeparam name="T">State Type - enum</typeparam>
    public class State<T> : StateBase where T : Enum
    {
        [SerializeField] protected T _type;

        public T explicitType => _type;
        public override Enum implicitType => _type;

        public State() { }

        public State(T type) => this._type = type;

        public State(T type, Func<bool> onStateStart = null, Func<bool> onUpdate = null, Func<bool> onFixedUpdate = null, 
            Func<bool> onStateEnd = null, params Transition[] transitions) : this(type)
        {
            this.onStateStart = onStateStart;
            this.onUpdate = onUpdate;
            this.onFixedUpdate = onFixedUpdate;
            this.onStateEnd = onStateEnd;
            this._transitions = new List<Transition>(transitions); 
        }
    }
}