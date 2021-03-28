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
        [SerializeField] protected T m_type;

        public T explicitType => m_type;
        public override Enum implicitType => m_type;

        public State() { }

        public State(T type) => this.m_type = type;

        public State(T type, Func<bool> onStateStart = null, Func<bool> onUpdate = null, Func<bool> onFixedUpdate = null, 
            Func<bool> onStateEnd = null, params Transition[] transitions) : this(type)
        {
            m_onStateStart = onStateStart;
            m_onUpdate = onUpdate;
            m_onFixedUpdate = onFixedUpdate;
            m_onStateEnd = onStateEnd;
            this.m_transitions = new List<Transition>(transitions); 
        }
    }
}