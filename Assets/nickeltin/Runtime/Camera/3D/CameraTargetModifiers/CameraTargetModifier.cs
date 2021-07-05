using UnityEngine;

namespace nickeltin.Runtime.Cameras.TriDimensional.CameraTargetModifiers
{
    [RequireComponent(typeof(CameraTarget))]
    public abstract class CameraTargetModifier : MonoBehaviour
    {
        public CameraTarget cameraTarget { get; protected set; }

        protected virtual void Awake() => cameraTarget = GetComponent<CameraTarget>();
    }
}