using UnityEngine;

namespace nickeltin.Runtime.Other
{
    public sealed class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}