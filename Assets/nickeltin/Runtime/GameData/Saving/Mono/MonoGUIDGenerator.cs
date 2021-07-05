using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    public sealed class MonoGUIDGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        public void GenerateGUIDs()
        {
            List<MonoSave> saves = new List<MonoSave>();
            GetComponentsInChildren(true, saves);
            if (TryGetComponent<MonoSave>(out var itself)) saves.Add(itself);
            Undo.RecordObjects(saves.ToArray(), "Generating GUID's");
            for (int i = saves.Count - 1; i >= 0; i--) saves[i].SaveID.GenerateGUID();
            Debug.Log($"{saves.Count} GUID's generated");
        }
#endif
    }
}