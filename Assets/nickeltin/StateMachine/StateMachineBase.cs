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
            [SerializeField] private string[] m_statesHistory = new string[historyLength];
            private float m_lastStateTime;
            private string m_lastStateName;
            
            private readonly int lastIndex = (historyLength - 1).Clamp0NoRef();
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
            
            private readonly List<Action> m_gizmos = new List<Action>();
            
            private void OnDrawGizmos()
            {
                for (var i = 0; i < m_gizmos.Count; i++) m_gizmos[i]?.Invoke();
            }
            
            public void AddGizmos(Action gizmoAction) => m_gizmos.Add(gizmoAction);

            public void RemoveGizmos(Action gizmoAction) => m_gizmos.Remove(gizmoAction);
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
                    for (int i = transitions.Count - 1; i >= 0; i--)
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
            
            private void Update() => Iterate(m_updateTransitions, m_updateList);

            private void FixedUpdate() => Iterate(m_fixedUpdateTransitions, m_fixedUpdateList);
            
            private void Iterate(IReadOnlyList<Transition> transitions, IReadOnlyList<Func<bool>> actions)
            {
                for (int i = 0; i < transitions.Count; i++)
                {
                    if (transitions[i].Condition()) onStateTransition?.Invoke(transitions[i].transitionTo);
                }
                for (int i = actions.Count - 1; i >= 0; i--) actions[i]?.Invoke();
            }
        }
        
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
            UpdateType updateType = UpdateType.Both)
        {
            if (state == null) return;

            if (mode == DataMode.Add)
            {
                m_engine.AddTransitions(state.transitions);
#if UNITY_EDITOR
                m_engine.AddGizmos(state.OnGizmosDraw);
#endif
            }
            else if (mode == DataMode.Remove)
            {
                m_engine.RemoveTransitions(state.transitions);
#if UNITY_EDITOR
                m_engine.RemoveGizmos(state.OnGizmosDraw);
#endif
            }

            if (updateType == UpdateType.Update || updateType == UpdateType.Both)
            {
                if (mode == DataMode.Add && updateEnabled) m_engine.AddData(null, state.OnUpdate);
                else if (mode == DataMode.Remove) m_engine.RemoveData(null, state.OnUpdate);
            }
                
            if (updateType == UpdateType.FixedUpdate || updateType == UpdateType.Both)
            {
                if (mode == DataMode.Add && fixedUpdateEnabled) m_engine.AddData(state.OnFixedUpdate, null);
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