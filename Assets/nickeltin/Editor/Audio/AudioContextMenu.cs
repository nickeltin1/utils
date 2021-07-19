using System;
using System.Collections.Generic;
using System.IO;
using nickeltin.Runtime.Audio;
using nickeltin.Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.Audio
{
    public static class AudioContextMenu
    {
        private const string baseMenuPath = "Assets/Create/" + MenuPathsUtility.audioMenu;
        private const string itemName = nameof(AudioEvent) + " from clip";
        private const string menuPath = baseMenuPath + itemName;

        [MenuItem(menuPath)]
        public static void CreateAudioEventFromClip_Context()
        {
            string path;
            
            if (Selection.activeObject is AudioClip firstClip)
            {
                path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(firstClip));
                
                path = EditorUtility.SaveFilePanelInProject("Create new " + nameof(AudioEvent),
                    Path.GetFileNameWithoutExtension(firstClip.name), "asset", "Select file destination", path);
                
                if(string.IsNullOrEmpty(path)) return;
            }
            else throw new Exception("Not an " + nameof(AudioClip));

            var audioEvent = ScriptableObject.CreateInstance<AudioEvent>();

            AssetDatabase.CreateAsset(audioEvent, AssetDatabase.GenerateUniqueAssetPath(path));
            
            IEnumerable<AudioClip> GetClipsFromSelection()
            {
                for (int i = 0; i < Selection.count; i++) if (Selection.objects[i] is AudioClip clip) yield return clip;
            }
            
            audioEvent.SetClips(GetClipsFromSelection());
            
            EditorUtility.SetDirty(audioEvent);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem(menuPath, true)]
        public static bool CreateAudioEventFromClip_Validator()
        {
            if (Selection.count == 0) return false;
            
            for (int i = 0; i < Selection.count; i++)
            {
                if (Selection.objects[i] is AudioClip) continue;
                return false;
            }
            
            return true;
        }
    }
}