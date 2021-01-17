namespace nickeltin.GameData.Saving
{
    public interface IMonoSaveable
    {
        void Load(MonoSave from);

        void Save(MonoSave to);
    }
}