using UnityEngine;

namespace nickeltin.Other
{
    public static class Utils
    {
        public static Vector2 ScreenResolution
        {
            get => new Vector2(Screen.width, Screen.height);
        }
    }
}