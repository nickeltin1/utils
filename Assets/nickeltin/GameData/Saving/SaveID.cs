using System;
using System.Text;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    [Serializable]
    public class SaveID
    {
        [SerializeField, HideInInspector, Tooltip("Use automatically generated ID")] public bool useGUID = true;
        [SerializeField, HideInInspector, Tooltip("ID should be unique")] private string _saveID;
        [SerializeField, HideInInspector, Tooltip("Globally Unique Identifier")] private string _GUID;

        public SaveID() => GenerateGUID();
        
        public bool GUIDGenerated => !_GUID.IsNullOrEmpty();
        public bool SaveIDGenerated => !_saveID.IsNullOrEmpty();
        public void GenerateGUID() => _GUID = Guid.NewGuid().ToString();

        public void GenerateSaveID(string objectName)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < objectName.Length; i++)
            {
                if (char.IsWhiteSpace(objectName[i])) sb.Append("_");
                else if (char.IsUpper(objectName[i]))
                {
                    if (i == 0 || char.IsWhiteSpace(objectName[(i - 1).Clamp0NoRef()])) sb.Append(char.ToLower(objectName[i]));
                    else sb.Append("_" + char.ToLower(objectName[i]));
                }
                else sb.Append(objectName[i]);
            }

            _saveID = sb.ToString().Trim();
        }
        public override string ToString() => useGUID ? _GUID : _saveID;
        
        public static implicit operator string(SaveID source) => source.ToString();
    }
}