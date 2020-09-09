using Characters;

namespace Interfaces
{
    public interface IEquipable : ISprite
    {
        CharacterBase Owner { get; }
    }
}