using UnityEngine;

namespace nickeltin.Singletons
{
    public abstract class SOSBase : ScriptableObject
    {
        public abstract bool Initialize();

        public abstract bool Destruct();
    }
}