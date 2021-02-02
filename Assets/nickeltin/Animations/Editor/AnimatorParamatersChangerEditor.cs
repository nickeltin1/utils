using UnityEditor;
using UnityEngine;

namespace nickeltin.Events.Editor
{
    [CustomEditor(typeof(AnimatorParamatersChanger))]
    public class AnimatorParamatersChangerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
                $"Create animator controller. Add int \"AnimIndex\" and assign indexes to correct states transition. " +
                $"Add trigger \"Next\" and assign to each transitions", 
                MessageType.Info);
            
            base.OnInspectorGUI();
        }
    }
}