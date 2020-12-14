using UnityEngine;

#pragma warning disable

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// Saveable with specified type, override its <see cref="Serialize"/>, <see cref="Deserialize"/>, <see cref="LoadDefault"/>
    /// methods to define how data inside is saved.
    /// Inherit from it to create your own save files.
    /// </summary>
    /// <typeparam name="SaveType">Any <see cref="System.Serializable"/> data type</typeparam>
    public abstract class Saveable<SaveType> : SaveableBase
    {
        [SerializeField] private SaveType m_file;
        public SaveType File
        {
            get => m_file;
            protected set => m_file = value;
        }
        
        public override void Save() => SaveSystem.Save(Serialize(), m_saveId);

        public override bool Load(bool loadDefault = false)
        {
            if (loadDefault || !SaveSystem.SaveExists(m_saveId))
            {
                LoadDefault();
                SetLoadState(false);
            }
            else if(SaveSystem.SaveExists(m_saveId))
            {
                Deserialize(SaveSystem.Load<SaveType>(m_saveId));
                SetLoadState(true);
            }

            return SuccessfulyLoaded;
        }
        
        public override void SetFileWithoutType(object file) => Deserialize((SaveType) file);

        public override object GetFileWithoutType() => Serialize();
        
        protected virtual SaveType Serialize() => File;
        protected virtual void Deserialize(SaveType obj) => File = obj;
    }
}