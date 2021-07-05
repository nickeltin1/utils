using System;

namespace nickeltin.Runtime.SceneManagment
{
    public partial class SceneManager
    {
        [Serializable]
        private class Settings
        {
            public bool logEventsToConsole = true;
            public bool loadUI = true;
        }
    }
}