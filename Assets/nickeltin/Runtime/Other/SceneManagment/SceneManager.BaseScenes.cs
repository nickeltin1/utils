using System;
using nickeltin.Extensions.Attributes;
using UnityEditor;

namespace nickeltin.Runtime.SceneManagment
{
    public partial class SceneManager
    {
        [Serializable]
        private struct BaseScenes
        {
            [Scene] public string initializeScene;
            [Scene] public string uiScene;
        }
    }
}