using System;
using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.StateMachine
{
    public abstract class StateMachineBase
    {
        protected class StateMachineEngine : MonoBehaviour
        {
#if UNITY_EDITOR
            private const int historyLength = 10;
            [SerializeField, ReadOnly] private string[] m_statesHistory = new string[historyLength];
            private float m_lastStateTime;
            private string m_lastStateName;
            
            private readonly int lastIndex = (historyLength - 1).Clamp0();
            private string timePassed => " lasted for " + Mathf.Abs(-Time.time + m_lastStateTime).ToString("F2") + "sec.";

            public void LogState(StateBase state)
            {
                m_statesHistory[lastIndex] = !m_lastStateName.IsNullOrEmpty() ? m_lastStateName + timePassed : "";
                m_statesHistory.ShiftLeft(1);
                string stateName = state != null ? state.implicitType.ToString() : "NULL";
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
                        if (transitions[i].conditionValidationMode == StateMachineBase.UpdateType.Update)
                            m_updateTransitions.Add(transitions[i]);
                        else if (transitions[i].conditionValidationMode == StateMachineBase.UpdateType.FixedUpdate) 
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
                        if (transitions[i].conditionValidationMode == StateMachineBase.UpdateType.Update)
                            m_updateTransitions.Remove(transitions[i]);
                        else if (transitions[i].conditionValidationMode == StateMachineBase.UpdateType.FixedUpdate) 
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
        
        [Flags] public enum UpdateType { Update, FixedUpdate }
        protected enum DataMode { Add, Remove }
        
        private bool m_enabled = true;
        public bool enabled
        {
            get => m_enabled;
            set => m_enabled = m_engine.enabled = value;
        }
        
        private bool m_updateEnabled = true;
        public bool updateEnabled
        {
            get => m_updateEnabled;
            set
            {
                ApplySettings(m_updateEnabled, value, UpdateType.Update);
                m_updateEnabled = value;
            }
        }

        private bool m_fixedUpdateEnabled;
        public bool fixedUpdateEnabled
        {
            get => m_fixedUpdateEnabled;
            set
            {
                ApplySettings(m_fixedUpdateEnabled, value, UpdateType.FixedUpdate);
                m_fixedUpdateEnabled = value;
            }
        }
        
        protected StateMachineEngine m_engine;
        
        protected void PassStateToEngine(StateBase state, DataMode mode, 
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
        
        protected void ApplySettings(bool oldValue, bool newValue, UpdateType type)
        {
            //Disable
            if (oldValue && !newValue)
            {
                PassStateToEngine(MainState_Internal, DataMode.Remove, type);
                PassStateToEngine(CurrentState_Internal, DataMode.Remove, type);
            }
            
            //Enable
            else if (!oldValue && newValue) 
            { 
                PassStateToEngine(MainState_Internal, DataMode.Add, type); 
                PassStateToEngine(CurrentState_Internal, DataMode.Add, type);
            }
        }
        
        public abstract void SwitchState(Enum type);
        protected abstract StateBase CurrentState_Internal { get; }
        protected abstract StateBase MainState_Internal { get; }
        
        /// <summary>
        /// Merges two state machines, by combining their engines, increases performance.
        /// </summary>
        public static void Merge(StateMachineBase a, StateMachineBase b)
        {
            Object.Destroy(b.m_engine);
            StateMachineEngine engine = b.m_engine = a.m_engine;
            engine.ClearData();
            engine.onStateTransition += b.SwitchState;
            a.PassStateToEngine(a.MainState_Internal, DataMode.Add);
            b.PassStateToEngine(b.MainState_Internal, DataMode.Add);
            a.PassStateToEngine(a.CurrentState_Internal, DataMode.Add);
            b.PassStateToEngine(b.CurrentState_Internal, DataMode.Add);
        }
    }
}