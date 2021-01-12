using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.StateMachine
{
    public sealed class StateMachine
    {
        private class StateMachineEngine : MonoBehaviour
        {
#if UNITY_EDITOR
            private const int historyLength = 10;
            [SerializeField, ReadOnly] private string[] m_statesHistory = new string[historyLength];
            private float m_lastStateTime;
            private string m_lastStateName;
            
            private readonly int lastIndex = (historyLength - 1).Clamp0();
            private string timePassed => " lasted for " + Mathf.Abs(-Time.time + m_lastStateTime).ToString("F2") + "sec.";

            public void LogState(State state)
            {
                m_statesHistory[lastIndex] = !m_lastStateName.IsNullOrEmpty() ? m_lastStateName + timePassed : "";
                m_statesHistory.ShiftLeft(1);
                string stateName = state != null ? state.type.ToString() : "NULL";
                m_statesHistory[lastIndex] = stateName;
                m_lastStateName = stateName;
                m_lastStateTime = Time.time;
            }
#endif
            public event Action<Enum> onStateTransition; 
            
            private readonly List<Func<bool>> m_updateList = new List<Func<bool>>();
            private readonly List<Func<bool>> m_fixedUpdateList = new List<Func<bool>>();
            private readonly List<Transition> m_updateTransitions = new List<Transition>();
            private readonly List<Transition> m_fixedUpdateTransitions = new List<Transition>();

            public void AddData(Func<bool> onFixedUpdate = null, Func<bool> onUpdate = null)
            {
                if (onFixedUpdate != null && !m_fixedUpdateList.Contains(onFixedUpdate)) 
                    m_fixedUpdateList.Add(onFixedUpdate);
                if (onUpdate != null && !m_updateList.Contains(onUpdate)) m_updateList.Add(onUpdate);
            }
            
            public void RemoveData(Func<bool> onFixedUpdate = null, Func<bool> onUpdate = null)
            {
                if (onFixedUpdate != null) m_fixedUpdateList.Remove(onFixedUpdate);
                if (onUpdate != null) m_updateList.Remove(onUpdate);
            }

            public void AddTransitions(IReadOnlyList<Transition> transitions = null)
            {
                if (transitions != null && transitions.Count > 0)
                {
                    for (int i = 0; i < transitions.Count; i++)
                    {
                        if (transitions[i].conditionValidationMode == UpdateType.Update)
                            m_updateTransitions.Add(transitions[i]);
                        else if (transitions[i].conditionValidationMode == UpdateType.FixedUpdate) 
                            m_fixedUpdateTransitions.Add(transitions[i]);
                    }
                }
            }

            public void RemoveTransitions(IReadOnlyList<Transition> transitions = null)
            {
                if (transitions != null && transitions.Count > 0)
                {
                    for (int i = 0; i < transitions.Count; i++)
                    {
                        if (transitions[i].conditionValidationMode == UpdateType.Update)
                            m_updateTransitions.Remove(transitions[i]);
                        else if (transitions[i].conditionValidationMode == UpdateType.FixedUpdate) 
                            m_fixedUpdateTransitions.Remove(transitions[i]);
                    }
                }
            }
            
            public void ClearData()
            {
                m_updateList.Clear();
                m_fixedUpdateList.Clear();
                m_updateTransitions.Clear();
                m_fixedUpdateTransitions.Clear();
            }
            
            private void Update()
            {
                if (m_updateTransitions.Count > 0)
                {
                    for (int i = m_updateTransitions.Count - 1; i >= 0; i--)
                    {
                        if (m_updateTransitions[i].Condition())
                        {
                            onStateTransition?.Invoke(m_updateTransitions[i].transitionTo);
                        }
                    }
                }
                
                if (m_updateList.Count > 0)
                {
                    for (int i = m_updateList.Count - 1; i >= 0; i--)
                    {
                        m_updateList[i]?.Invoke();
                    }
                }
            }
            private void FixedUpdate()
            {
                if (m_fixedUpdateTransitions.Count > 0)
                {
                    for (int i = m_fixedUpdateTransitions.Count - 1; i >= 0; i--)
                    {
                        if (m_fixedUpdateTransitions[i].Condition())
                        {
                            onStateTransition?.Invoke(m_fixedUpdateTransitions[i].transitionTo);
                        }
                    }
                }
                
                if (m_fixedUpdateList.Count > 0)
                {
                    for (int i = m_fixedUpdateList.Count - 1; i >= 0; i--)
                    {
                        m_fixedUpdateList[i]?.Invoke();
                    }
                }
            }
        }
        
        private enum DataMode { Add, Remove }
        [Flags] public enum UpdateType { Update, FixedUpdate }

        public State MainState { get; private set; }

        private State m_currentState;
        public State CurrentState
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

        private bool m_enabled = true;
        public bool Enabled
        {
            get => m_enabled;
            set => m_enabled = m_engine.enabled = value;
        }
        
        private bool m_updateEnabled = true;
        public bool UpdateEnabled
        {
            get => m_updateEnabled;
            set
            {
                ApplySettings(m_updateEnabled, value, UpdateType.Update);
                m_updateEnabled = value;
            }
        }

        private bool m_fixedUpdateEnabled;
        public bool FixedUpdateEnabled
        {
            get => m_fixedUpdateEnabled;
            set
            {
                ApplySettings(m_fixedUpdateEnabled, value, UpdateType.FixedUpdate);
                m_fixedUpdateEnabled = value;
            }
        }

        
        private readonly Dictionary<Enum, State> m_states = new Dictionary<Enum, State>();
        private StateMachineEngine m_engine;
        
        public StateMachine(Transform parent, bool updateEnabled, bool fixedUpdateEnabled, [Optional] params State[] states)
        {
            m_engine = parent.gameObject.AddComponent<StateMachineEngine>();
            m_engine.onStateTransition += SwitchState;
            this.UpdateEnabled = updateEnabled;
            this.FixedUpdateEnabled = fixedUpdateEnabled;
            if (states != null) foreach (var state in states) AddState(state);
        }

        public StateMachine(Transform parent, [Optional] params State[] states) 
            : this(parent, true, true, states) { }
        
        
        /// <summary>
        /// Merges two state machines, by combining their engines, increases performance.
        /// </summary>
        public static void Merge(StateMachine a, StateMachine b)
        {
            Object.Destroy(b.m_engine);
            StateMachineEngine engine = b.m_engine = a.m_engine;
            engine.ClearData();
            engine.onStateTransition += b.SwitchState;
            a.PassStateToEngine(a.MainState, DataMode.Add);
            b.PassStateToEngine(b.MainState, DataMode.Add);
            a.PassStateToEngine(a.m_currentState, DataMode.Add);
            b.PassStateToEngine(b.m_currentState, DataMode.Add);
        }

        /// <summary>
        /// Ends current state, and starts new.
        /// </summary>
        public void SwitchState(Enum type) => SwitchState(GetState(type));
        
        /// <summary>
        /// Ends current state, and starts new.
        /// </summary>
        /// <returns>Is switching successful</returns>
        public bool SwitchState(State to)
        {
            if (to != null && m_states.ContainsValue(to))
            {
                if (to == CurrentState) return false;
                PassStateToEngine(CurrentState, DataMode.Remove);
                if(m_enabled) CurrentState?.OnStateEnd();
                CurrentState = to;
                if(m_enabled) CurrentState.OnStateStart();
                PassStateToEngine(CurrentState, DataMode.Add);
                return true;
            }
            
            Debug.LogError($"State " + to + " doesn't exist, and cannot be switched to");
            return false;
        }

        public void AddState(State newState)
        {
            if (!m_states.ContainsKey(newState.type))
            {
                m_states.Add(newState.type, newState);
                if (CurrentState == null) SwitchState(newState);
            }
            else Debug.LogError("You're trying to add state " + newState.type + ", but it already exists");
        }
        
        /// <summary>
        /// Main state runs all the time, end executes only its Update/FixedUpdate/etc methods.
        /// </summary>
        public void SetMainState(State newMainState)
        {
            PassStateToEngine(MainState, DataMode.Remove);
            MainState?.OnStateEnd();
            MainState = newMainState;
            MainState?.OnStateStart();
            PassStateToEngine(MainState, DataMode.Add);
        }

        public void OverrideMainState(State newMainState)
        {
            PassStateToEngine(MainState, DataMode.Remove);
            MainState = newMainState;
            PassStateToEngine(MainState, DataMode.Add);
        }

        /// <summary>
        /// Exits currently running state.
        /// </summary>
        public State ExitState()
        {
            PassStateToEngine(CurrentState, DataMode.Remove);
            if(m_enabled)CurrentState?.OnStateEnd();
            var exitState = CurrentState;
            CurrentState = null;
            return exitState;
        }
        
        /// <summary>
        /// Returns state if its exists, else returns null.
        /// </summary>
        public State GetState(Enum type)
        {
            if (m_states.TryGetValue(type, out var requestedState)) return requestedState;
            Debug.LogError($"Requested state " + type + " doesn't exist");
            return null;
        }

        public void OverrideState(Enum oldStateType, State newState) => OverrideState(GetState(oldStateType), newState);
        public void OverrideState(State oldState, State newState)
        {
            if (!oldState.type.Equals(newState.type))
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
            else if (m_states.ContainsValue(newState)) m_states[oldState.type] = newState;
            else Debug.LogError($"{oldState} doesn't exist, and cannot be overwritten");
        }
        
        private void PassStateToEngine(State state, DataMode mode, 
            UpdateType updateType = UpdateType.Update | UpdateType.FixedUpdate)
        {
            if (state == null) return;

            if (mode == DataMode.Add) m_engine.AddTransitions(state.transitions);
            else if (mode == DataMode.Remove) m_engine.RemoveTransitions(state.transitions);

            if (updateType == UpdateType.Update)
            {
                if (mode == DataMode.Add) m_engine.AddData(null, state.OnUpdate);
                else if (mode == DataMode.Remove) m_engine.RemoveData(null, state.OnUpdate);
            }
                
            if (updateType == UpdateType.FixedUpdate)
            {
                if (mode == DataMode.Add) m_engine.AddData(state.OnFixedUpdate, null);
                else if (mode == DataMode.Remove) m_engine.RemoveData(state.OnFixedUpdate, null);
            }
        }
        
        private void ApplySettings(bool oldValue, bool newValue, UpdateType type)
        {
            //Disable
            if (oldValue && !newValue)
            {
                PassStateToEngine(MainState, DataMode.Remove, type);
                PassStateToEngine(m_currentState, DataMode.Remove, type);
            }
            
            //Enable
            else if (!oldValue && newValue) 
            { 
                PassStateToEngine(MainState, DataMode.Add, type); 
                PassStateToEngine(m_currentState, DataMode.Add, type);
            }
        }
    }
}