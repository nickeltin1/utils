namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// Saveable item without SAVE/LOAD functionality, but it can be added as a part of custom <see cref="SaveRegistry{T}"/>
    /// and as entry inside <see cref="SaveSystem"/> database.
    /// </summary>
    public abstract class RegistryItem : SaveableBase
    {
        public override bool Load(SaveSystem saveSystem) => false;

        public override void Save(SaveSystem saveSystem) { }

        public override void LoadDefault() { }
        
        public override void SetFileWithoutType(SaveSystem saveSystem, object file) { }

        public override object GetFileWithoutType() => new object();
    }
}