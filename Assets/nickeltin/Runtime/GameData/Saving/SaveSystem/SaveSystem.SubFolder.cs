using System;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        [Serializable]
        public class SubFolder
        {
            [SerializeField, HideInInspector, Delayed] private string _name;
            [SerializeField, HideInInspector] private string _oldName;

            public SubFolder(string name)
            {
                _name = name;
                _oldName = name;
            }

            public static implicit operator string(SubFolder source) => source._name;

#if UNITY_EDITOR
            public static string name_prop_name => nameof(_name);
            public static string old_name_prop_name => nameof(_oldName);
#endif
        }
    }
}