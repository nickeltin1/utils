namespace nickeltin.GameData.Saving
{
    public interface ISaveable
    {
        string SaveID { get; }
        bool Load(bool loadDefault = false);
        void Save();
        bool Register();
    }
}