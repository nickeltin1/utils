using UnityEditor;
using UnityEngine;

namespace nickeltin.Cameras.TriDimensional
{
    public abstract class CameraSystemObjectBase : MonoBehaviour
    {
#if UNITY_EDITOR
        [ContextMenu("Copy settings")]
        private void CopySettings_Context() => CopySettings();

        [ContextMenu("Paste settings")]
        private void PasteSettings_Context()
        {
            Undo.RecordObject(gameObject, "Pasting camera settings");
            PasteSettings();
        }

        protected abstract void CopySettings();
        protected abstract void PasteSettings();
#endif
    }
}