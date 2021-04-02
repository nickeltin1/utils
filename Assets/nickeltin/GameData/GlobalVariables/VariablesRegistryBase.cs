using UnityEngine;

namespace nickeltin.Experimental.GlobalVariables
{
    public abstract class VariablesRegistryBase : ScriptableObject
    {
        public abstract string[] Keys { get; }
        
        public abstract  int Count { get; }
    }
}