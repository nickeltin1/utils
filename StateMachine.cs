using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class StateMachine : MonoBehaviour
    {
        public enum StateType
        {
            Default
        }
        private State currentState;
        private State mainState;
        private Dictionary<Enum, State> states = new Dictionary<Enum, State>();

        public static StateMachine Initialize(Transform parent, State[] states = null)
        {
            var instance = parent.gameObject.AddComponent<StateMachine>();
            
            if (states != null)
            {
                instance.currentState = states[0];
                foreach (var state in states)
                {
                    instance.states.Add(state.Type, state);
                }
            }
            return instance;
        }

        public State SwichtState(Enum type)
        {
            if (states.TryGetValue(type, out var requestedState))
            {
                if (requestedState == currentState) return null;
                currentState.OnStateEnd?.Invoke();
                currentState = requestedState;
                currentState.OnStateStart?.Invoke();
                return currentState;
            }

            Debug.LogWarning($"Requested state " + type + " doesn't exist");
            return null;
        }
        public void AddState(State newState)
        {
            if (!states.ContainsKey(newState.Type))
            {
                states.Add(newState.Type, newState);
                if (currentState == null) currentState = newState;
            }
        }
        public void SetMainState(State defaultState)
        {
            this.mainState = defaultState;
        }


        private void Update()
        {
            mainState?.OnUpdate?.Invoke();
            currentState?.OnUpdate?.Invoke();
        }
        private void FixedUpdate()
        {
            mainState?.OnFixedUpdate?.Invoke();
            currentState?.OnFixedUpdate?.Invoke();
        }
    }
}