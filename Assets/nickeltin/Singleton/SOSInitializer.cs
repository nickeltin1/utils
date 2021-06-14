using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.Singletons
{
    public class SOSInitializer : MonoSingleton<SOSInitializer>
    {
        [SerializeField] private SOSBase[] _awake;
        [SerializeField] private SOSBase[] _start;
        [SerializeField] private Object[] _editorReferences;

        protected override void Awake()
        {
            if (Awake_Internal())
            {
                _awake.ForEach(sos =>
                {
                    if (!sos.Initialize())
                    {
                        Debug.Log($"Scriptable Object Singleton of type {this.GetType().Name} is already initialized");
                    }
                });
            }
        }

        private void Start()
        {
            _start.ForEach(sos =>
            {
                if (!sos.Initialize())
                {
                    Debug.Log($"Scriptable Object Singleton of type {this.GetType().Name} is already initialized");
                }
            });
        }


        protected override void OnDestroy()
        {
            if (OnDestroy_Internal())
            {
                _awake.ForEach(sos => sos.Destruct());
                _start.ForEach(sos => sos.Destruct());
            }
        }

        [ContextMenu("Refresh List")]
        public int RefreshList()
        {
            _awake = Resources.FindObjectsOfTypeAll<SOSBase>();
            return _awake.Length;
        }
    }
}