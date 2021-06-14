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
        
        public override void Save(SaveSystem saveSystem) => saveSystem.Save(Serialize(), SaveID);

        public override bool Load(SaveSystem saveSystem)
        {
            if (!saveSystem.SaveExists(SaveID))
            {
                LoadDefault();
                SetLoadState(false);
            }
            else if(saveSystem.SaveExists(SaveID))
            {
                Deserialize(saveSystem, saveSystem.Load<SaveType>(SaveID));
                SetLoadState(true);
            }

            return SuccessfulyLoaded;
        }
        
        public override void SetFileWithoutType(SaveSystem saveSystem, object file) => 
            Deserialize(saveSystem, (SaveType) file);

        public override object GetFileWithoutType() => Serialize();
        
        protected virtual SaveType Serialize() => File;
        protected virtual void Deserialize(SaveSystem saveSystem, SaveType obj) => File = obj;
    }
}