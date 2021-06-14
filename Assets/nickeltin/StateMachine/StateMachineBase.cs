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
            [SerializeField] private string[] _statesHistory = new string[historyLength];
            private float _lastStateTime;
            private string _lastStateName;
            
            private readonly int lastIndex = (historyLength - 1).Clamp0NoRef();
            private string timePassed => " lasted for " + Mathf.Abs(-Time.time + _lastStateTime).ToString("F2") + "sec.";

            public void LogState(StateBase state)
            {
                _statesHistory[lastIndex] = !_lastStateName.IsNullOrEmpty() ? _lastStateName + timePassed : "";
                _statesHistory.ShiftLeft(1);
                string stateName = state != null ? state.implicitType.ToString() : "NULL";
                _statesHistory[lastIndex] = stateName;
                _lastStateName = stateName;
                _lastStateTime = Time.time;
            }
            
            private readonly List<Action> _gizmos = new List<Action>();
            
            private void OnDrawGizmos()
            {
                for (var i = 0; i < _gizmos.Count; i++) _gizmos[i]?.Invoke();
            }
            
            public void AddGizmos(Action gizmoAction) => _gizmos.Add(gizmoAction);

            public void RemoveGizmos(Action gizmoAction) => _gizmos.Remove(gizmoAction);
#endif
            public event Action<Enum> onStateTransition; 
            
            private readonly List<Func<bool>> _updateList = new List<Func<bool>>();
            private readonly List<Func<bool>> _fixedUpdateList = new List<Func<bool>>();
            private readonly List<Transition> _updateTransitions = new List<Transition>();
            private readonly List<Transition> _fixedUpdateTransitions = new List<Transition>();
           

            public void AddData(Func<bool> onFixedUpdate = null, Func<bool> onUpdate = null)
            {
                if (onFixedUpdate != null && !_fixedUpdateList.Contains(onFixedUpdate)) 
                    _fixedUpdateList.Add(onFixedUpdate);
                if (onUpdate != null && !_updateList.Contains(onUpdate)) _updateList.Add(onUpdate);
            }
            
            public void RemoveData(Func<bool> onFixedUpdate = null, Func<bool> onUpdate = null)
            {
                if (onFixedUpdate != null) _fixedUpdateList.Remove(onFixedUpdate);
                if (onUpdate != null) _updateList.Remove(onUpdate);
            }
            

            public void AddTransitions(IReadOnlyList<Transition> transitions = null)
            {
                if (transitions != null && transitions.Count > 0)
                {
                    for (int i = 0; i < transitions.Count; i++)
                    {
                        if (transitions[i].conditionValidationMode == UpdateType.Update)
                            _updateTransitions.Add(transitions[i]);
                        else if (transitions[i].conditionValidationMode == UpdateType.FixedUpdate) 
                            _fixedUpdateTransitions.Add(transitions[i]);
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
                            _updateTransitions.Remove(transitions[i]);
                        else if (transitions[i].conditionValidationMode == UpdateType.FixedUpdate) 
                            _fixedUpdateTransitions.Remove(transitions[i]);
                    }
                }
            }
            
            public void ClearData()
            {
                _updateList.Clear();
                _fixedUpdateList.Clear();
                _updateTransitions.Clear();
                _fixedUpdateTransitions.Clear();
            }
            
            private void Update() => Iterate(_updateTransitions, _updateList);

            private void FixedUpdate() => Iterate(_fixedUpdateTransitions, _fixedUpdateList);
            
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
        
        private bool _enabled = true;
        public bool enabled
        {
            get => _enabled;
            set => _enabled = m_engine.enabled = value;
        }
        
        private bool _updateEnabled = true;
        public bool updateEnabled
        {
            get => _updateEnabled;
            set
            {
                ApplySettings(_updateEnabled, value, UpdateType.Update);
                _updateEnabled = value;
            }
        }

        private bool _fixedUpdateEnabled;
        public bool fixedUpdateEnabled
        {
            get => _fixedUpdateEnabled;
            set
            {
                ApplySettings(_fixedUpdateEnabled, value, UpdateType.FixedUpdate);
                _fixedUpdateEnabled = value;
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