using UnityEngine;
using UnityEngine.SceneManagement;

namespace nickeltin.Runtime.Other
{
    public static class Utils
    {
        public static Vector2 ScreenResolution
        {
            get => new Vector2(Screen.width, Screen.height);
        }

        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}