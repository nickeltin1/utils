using System;
using System.Collections.Generic;
using nickeltin.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.StateMachine
{
    public class StateMachine : ILifeCycle
    {
        public enum StateType { Main, Disabled, Enabled }
        private enum DataMode { Add, Remove }
        
        private State mainState;
        public State MainState
        {
            get => mainState;
            private set
            {
                PassDataToEngine(mainState, DataMode.Remove);
                mainState = value;
                PassDataToEngine(mainState, DataMode.Add);
            }
        }
        
        private State currentState;
        public State CurrentState
        {
            get => currentState;
            private set
            {
                PassDataToEngine(currentState, DataMode.Remove);
                currentState = value;
                PassDataToEngine(currentState, DataMode.Add);
            }
        }
        
        
        private Dictionary<Enum, State> states = new Dictionary<Enum, State>();
        private bool updateEnabled;
        private bool fixedUpdateEnabled;
        private StateMachineEngine engine;

        public StateMachine(Transform parent, IEnumerable<State> states = null, bool updateEnabled = true, 
            bool fixedUpdateEnabled = true)
        {
            engine = parent.gameObject.AddComponent<StateMachineEngine>();
            SetSettings(updateEnabled, fixedUpdateEnabled);
            if (states != null) foreach (var state in states) AddState(state);
        }

        public void SetSettings(bool updateEnabled, bool fixedUpdateEnabled)
        {
            this.updateEnabled = updateEnabled;
            this.fixedUpdateEnabled = fixedUpdateEnabled;
        }
        public static void Merge(StateMachine a, StateMachine b)
        {
            Object.Destroy(b.engine);
            StateMachineEngine engine = b.engine = a.engine;
            engine.ClearData();
            a.PassDataToEngine(a.mainState, DataMode.Add);
            b.PassDataToEngine(b.mainState, DataMode.Add);
            a.PassDataToEngine(a.currentState, DataMode.Add);
            b.PassDataToEngine(b.currentState, DataMode.Add);
        }
        public State SwichtState(Enum type)
        {
            var requestedState = GetState(type);
            if (requestedState != null)
            {
                if (requestedState == CurrentState) return null;
                CurrentState?.OnStateEnd?.Invoke();
                CurrentState = requestedState;
                CurrentState.OnStateStart?.Invoke();
                return CurrentState;
            }
            
            return null;
        }
        public void AddState(State newState)
        {
            if (!states.ContainsKey(newState.Type))
            {
                states.Add(newState.Type, newState);
                if (CurrentState == null)
                {
                    CurrentState = newState;
                    CurrentState.OnStateStart?.Invoke();
                } 
            }
        }
        public void SetMainState(State newMainState)
        {
            this.MainState = newMainState;
        }
        public State ExitState()
        {
            CurrentState?.OnStateEnd?.Invoke();
            var exitState = CurrentState;
            CurrentState = null;
            return exitState;
        }
        public void OverrideState(State newOverridedState)
        {
            if (states.ContainsKey(newOverridedState.Type)) states[newOverridedState.Type] = newOverridedState;
            else Debug.LogWarning($"State " + newOverridedState.Type + " doesn't exist, and can't be overrided");
        }
        public State GetState(Enum type)
        {
            if (states.TryGetValue(type, out var requestedState)) return requestedState;
            Debug.LogWarning($"Requested state " + type + " doesn't exist");
            return null;
        }
        private void PassDataToEngine(State state, DataMode mode)
        {
            switch (mode)
            {
                case DataMode.Add:
                    if (state != null)
                    {
                        engine.AddData(fixedUpdateEnabled ? state.OnFixedUpdate : null, 
                            updateEnabled ? state.OnUpdate : null);
                    }
                    break;
                case DataMode.Remove:
                    if(state != null) engine.RemoveData(state.OnFixedUpdate, state.OnUpdate);
                    break;
            }
        }
        public void Disable()
        {
            engine.enabled = false;
            Disabled = true;
        }
        
        public void Enable()
        {
            engine.enabled = true;
            Disabled = false;
        }
        
        private class StateMachineEngine : MonoBehaviour
        {
            private readonly List<Action> updateList = new List<Action>();
            private readonly List<Action> fixedUpdateList = new List<Action>();
            
            public void AddData(Action onFixedUpdate, Action onUpdate)
            {
                if (onFixedUpdate != null) fixedUpdateList.Add(onFixedUpdate);
                if (onUpdate != null) updateList.Add(onUpdate);
            }
            public void RemoveData(Action onFixedUpdate, Action onUpdate)
            {
                if (onFixedUpdate != null) fixedUpdateList.Remove(onFixedUpdate);
                if (onUpdate != null) updateList.Remove(onUpdate);
            }

            public void ClearData()
            {
                updateList.Clear();
                fixedUpdateList.Clear();
            }
            
            private void Update()
            {
                if (updateList.Count == 0) return;
                for (int i = updateList.Count - 1; i >= 0; i--)
                {
                    updateList[i]?.Invoke();
                }
            }
            private void FixedUpdate()
            {
                if(fixedUpdateList.Count == 0) return;
                for (int i = fixedUpdateList.Count - 1; i >= 0; i--)
                {
                    fixedUpdateList[i]?.Invoke();
                }
                
            }
        }

        
        public bool Disabled { get; private set; }
    }
}