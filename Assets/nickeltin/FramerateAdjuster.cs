using UnityEngine;

namespace nickeltin.Other
{
    public class FramerateAdjuster : MonoBehaviour
    {
        [SerializeField, Range(12, 144)] private int targetFramerate = 60;
        private void Awake()
        {
            Application.targetFrameRate = targetFramerate;
        }
    }
}
