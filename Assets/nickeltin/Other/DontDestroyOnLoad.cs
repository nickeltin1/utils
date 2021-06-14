using UnityEngine;

namespace nickeltin.Other
{
    public sealed class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}