using System;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        [Serializable]
        public class SubFolder
        {
            [SerializeField, HideInInspector] private string _name;

            public SubFolder(string name) => _name = name;

            public static implicit operator string(SubFolder source) => source._name;

#if UNITY_EDITOR
            public static string name_prop_name => nameof(_name);
#endif
        }
    }
}