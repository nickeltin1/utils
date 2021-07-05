using System;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        [Serializable] 
        public class AutosavingSettings
        {
            public const float MIN_AUTOSAVE_INTERVAL = 5; 
            public const float MAX_AUTOSAVE_INTERVAL = 900;
            
            [Range(15, 900)] public float autosaveInterval = 120;
            public bool onApplicationQuit = true;
            public bool onApplicationLooseFocus = false;
            public bool onApplicationPause = false;
            
            [HideInInspector] public bool autosaveEnabled = false;
        }
    }
}