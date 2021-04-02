using UnityEngine;

namespace nickeltin.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        public MonoBehaviour owner;
        
        public void SetOwner<T>(T owner) where T : MonoBehaviour
        {
            this.owner = owner;
        }
        
        public bool TryGetOwner<T>(out T owner)
        {
            if (this.owner is T requestedObject)
            {
                owner = requestedObject;
                return true;
            }

            owner = default;
            return false;
        }

        public static bool TryGetOwner<T>(GameObject other, out T owner)
        {
            if (other.TryGetComponent(out Hitbox hitbox) && hitbox.TryGetOwner(out owner))
            {
                return true;
            }
            
            owner = default;
            return false;
        }
    }
}