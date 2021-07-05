using UnityEngine;

namespace nickeltin.Runtime.Singletons
{
    public abstract class SOSBase : ScriptableObject
    {
        public abstract bool Initialize();

        public abstract bool Destruct();
    }
}