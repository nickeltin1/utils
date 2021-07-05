using System;
using nickeltin.Runtime.GameData.VariablesRefrences;

namespace nickeltin.Runtime.SceneManagment
{
    public partial class SceneManager
    {
        [Serializable]
        private struct Events
        {
            public EventRef afterLevelLoad;
            public EventRef afterLevelCompleted;
            public EventRef beforeLevelReload;
        }
    }
}