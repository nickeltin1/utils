using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.Singletons
{
    public class SOSInitializer : MonoSingleton<SOSInitializer>
    {
        [SerializeField] private List<SOSBase> toInitialize;
        [SerializeField] private List<Object> editorReferences;

        public void AddItems(SOSBase[] targets)
        {
            if (toInitialize == null) toInitialize = new List<SOSBase>();
            toInitialize.AddRange(targets);
        }

        protected override void Awake()
        {
            base.Awake();
            
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