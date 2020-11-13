namespace nickeltin.Interfaces
{
    public interface IInitializable
    {
        IInitializable Initialize(IInitializer parent);
    }
}