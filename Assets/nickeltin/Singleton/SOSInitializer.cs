using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.Singletons
{
    public class SOSInitializer : MonoBehaviour
    {
        [SerializeField, ReorderableList("SOSingleton")] private List<SOSBase> toInitialize;

        public void AddItems(SOSBase[] targets)
        {
            if (toInitialize == null) toInitialize = new List<SOSBase>();
            toInitialize.AddRange(targets);
        }

        private void Awake()
        {
            foreach (var sos in toInitialize)
            {
                if (!sos.Initialize())
                {
                    Debug.Log($"Scriptable Object Singleton of type {this.GetType().Name} is already initialized");
                }
            }
        }
    }
}