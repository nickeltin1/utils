using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace nickeltin.StateMachine
{
    public sealed class StateMachine : TypedStateMachine<State>
    {
        public StateMachine(Transform parent, bool updateEnabled, bool fixedUpdateEnabled, 
            Action<State> onInitialization = null, params State[] states) 
            : base(parent, updateEnabled, fixedUpdateEnabled, onInitialization, states)
        {
        }

        public StateMachine(Transform parent, Action<State> onInitialization, 
            params State[] states) : base(parent, onInitialization, states)
        {
        }

        public StateMachine(Transform parent, params State[] states) : base(parent, states)
        {
        }
    }
    
    public class EnumStateMachine<T> : TypedStateMachine<State<T>> where T : Enum
    {
        public EnumStateMachine(Transform parent, bool updateEnabled, bool fixedUpdateEnabled, 
            Action<State<T>> onInitialization = null, params State<T>[] states) 
            : base(parent, updateEnabled, fixedUpdateEnabled, onInitialization, states)
        {
        }

        public EnumStateMachine(Transform parent, Action<State<T>> onInitialization, 
            params State<T>[] states) : base(parent, onInitialization, states)
        {
        }

        public EnumStateMachine(Transform parent, params State<T>[] states) : base(parent, states)
        {
        }
    }
    
    /// <summary>
    /// Inherit form it to create custom state machine
    /// </summary>
    /// <typeparam name="T">State type that machine using</typeparam>
    public class TypedStateMachine<T> : StateMachineBase where T : StateBase
    {
        public T MainState { get; private set; }
        
        private T m_currentState;
        public T CurrentState
        {
            get => m_currentState;
            private set
            {
                m_currentState = value;
                #if UNITY_EDITOR
                m_engine.LogState(m_currentState);
                #endif
            }
        }
        
        private readonly List<Enum> m_statesOrder = new List<Enum>();
        private readonly Dictionary<Enum, T> m_states = new Dictionary<Enum, T>();
        
        /// <param name="onInitialization">Executes on StateMachine creation for each state, before <see cref="State.OnStateStart"/></param>
        public TypedStateMachine(Transform parent, bool updateEnabled, bool fixedUpdateEnabled, 
            [Optional] Action<T> onInitialization, [Optional] params T[] states)
        {
            m_engine = parent.gameObject.AddComponent<StateMachineEngine>();
            m_engine.onStateTransition += SwitchState;
            this.updateEnabled = updateEnabled;
            this.fixedUpdateEnabled = fixedUpdateEnabled;
            if (states != null)
            {
                foreach (var state in states)
                {
                    onInitialization?.Invoke(state);
                    AddState(state);
                }
            }
        }
        
       
        public TypedStateMachine(Transform parent, Action<T> onInitialization, [Optional] params T[] states) 
            : this(parent, true, true,onInitialization, states) { }

        public TypedStateMachine(Transform parent, [Optional] params T[] states) 
            : this(parent, null, states) { }
        
        /// <summary>
        /// Switches to next state, in order you added them. If its the last state, then starts form begining.
        /// </summary>
        public void SwitchToNextState() => SwitchState(GetNextState());


        /// <summary>
        /// Gets state by index, in order you added them.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetStateAtIndex(int index) => m_states[m_statesOrder[index]];

        public T GetNextState()
        {
            int currentIndex = 0;
            if (CurrentState != null)
            {
                currentIndex = m_statesOrder.IndexOf(CurrentState.implicitType);
                if (++currentIndex >= m_statesOrder.Count) currentIndex = 0;
            }

            return m_states[m_statesOrder[currentIndex]];
        }

        /// <summary>
        /// Ends current state, and starts new.
        /// </summary>
        public override void SwitchState(Enum type) => SwitchState(GetState(type));
        
        /// <summary>
        /// Ends current state, and starts new.
        /// </summary>
        /// <returns>Is switching successful</returns>
        public bool SwitchState(T to)
        {
            if (to != null && m_states.ContainsValue(to))
            {
                if (to == CurrentState) return false;
                PassStateToEngine(CurrentState, DataMode.Remove);
                if(enabled) CurrentState?.OnStateEnd();
                CurrentState = to;
                if(enabled) CurrentState.OnStateStart();
                PassStateToEngine(CurrentState, DataMode.Add);
                return true;
            }
            
            Debug.LogError($"State" + to + " doesn't exist, and cannot be switched to");
            return false;
        }

        public void AddState(T newState)
        {
            if (!m_states.ContainsKey(newState.implicitType))
            {
                m_states.Add(newState.implicitType, newState);
                m_statesOrder.Add(newState.implicitType);
                if (CurrentState == null) SwitchState(newState);
            }
            else Debug.LogError("You're trying to add state " + newState.implicitType + ", but it already exists");
        }

        public void AddStates(params T[] states)
        {
            foreach (var state in states) AddState(state);
        }
        
        /// <summary>
        /// Main state runs all the time, end executes only its Update/FixedUpdate/etc methods.
        /// </summary>
        public void SetMainState(T newMainState)
        {
            PassStateToEngine(MainState, DataMode.Remove);
            MainState?.OnStateEnd();
            MainState = newMainState;
            MainState?.OnStateStart();
            PassStateToEngine(MainState, DataMode.Add);
        }

        public void OverrideMainState(T newMainState)
        {
            PassStateToEngine(MainState, DataMode.Remove);
            MainState = newMainState;
            PassStateToEngine(MainState, DataMode.Add);
        }

        /// <summary>
        /// Exits currently running state.
        /// </summary>
        public T ExitState()
        {
            PassStateToEngine(CurrentState, DataMode.Remove);
            if(enabled)CurrentState?.OnStateEnd();
            var exitState = CurrentState;
            CurrentState = null;
            return exitState;
        }
        
        /// <summary>
        /// Returns state if its exists, else returns null.
        /// </summary>
        public T GetState(Enum type)
        {
            if (m_states.TryGetValue(type, out var requestedState)) return requestedState;
            Debug.LogError($"Requested state " + type + " doesn't exist");
            return null;
        }

        public void OverrideState(Enum oldStateType, T newState) => OverrideState(GetState(oldStateType), newState);
        public void OverrideState(T oldState, T newState)
        {
            if (!oldState.implicitType.Equals(newState.implicitType))
            {
                Debug.LogError($"{oldState} and {newState} has different types");
                return;
            }
            
            if (oldState == CurrentState)
            {
                PassStateToEngine(CurrentState, DataMode.Remove);
                CurrentState = newState;
                PassStateToEngine(CurrentState, DataMode.Add);
            }
            else if (m_states.ContainsValue(newState)) m_states[oldState.implicitType] = newState;
            else Debug.LogError($"{oldState} doesn't exist, and cannot be overwritten");
        }
        
        protected override StateBase CurrentState_Internal => m_currentState;
        protected override StateBase MainState_Internal => MainState;
    }
}