using System;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.GameData.VariablesRefrences;

namespace nickeltin.Runtime.SceneManagment
{
    public partial class SceneManager
    {
        [Serializable]
        private struct Events
        {
            public EventObject afterLevelLoad;
            public EventObject beforeLevelCompleted;
            public EventObject afterLevelCompleted;
            public EventObject beforeLevelReload;
            
        }
    }
}