#pragma warning disable

using System;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    [Serializable]
    public abstract class Saveable<SaveType> : ISaveable
    {
        [Header("Saving")] 
        [SerializeField] private string m_saveId;

        public bool SuccessfulyLoaded { get; private set; }
        public string SaveID => m_saveId;
        
        public bool Register()
        {
            if (SaveSystem.AddSavedItem(m_saveId, this)) return true;
            return false;
        }
        
        public void Save() => SaveSystem.Save(Serialize(), m_saveId);

        public bool Load(bool loadDefault = false)
        {
            SuccessfulyLoaded = false;
            if (loadDefault) Deserialize(GetDefault());
            else if (SaveSystem.SaveExists(m_saveId))
            {
                Deserialize(SaveSystem.Load<SaveType>(m_saveId));
                SuccessfulyLoaded = true;
            }

            return SuccessfulyLoaded;
        }
        
        protected virtual SaveType GetDefault() => default;
        protected abstract SaveType Serialize();
        protected abstract void Deserialize(SaveType obj);
    }
}