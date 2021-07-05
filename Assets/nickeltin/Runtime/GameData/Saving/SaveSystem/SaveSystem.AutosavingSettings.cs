using System;
using nickeltin.Extensions.Types;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        [Serializable] 
        public class AutosavingSettings
        {
            // public const float MIN_AUTOSAVE_INTERVAL = 5; 
            // public const float MAX_AUTOSAVE_INTERVAL = 900;
            
            [Tooltip("In seconds")] public Optional<float> autosaveInterval = 120;
            public bool onApplicationQuit = true;
            public bool onApplicationLooseFocus = false;
            public bool onApplicationPause = false;
        }
    }
}