using UnityEngine;

namespace nickeltin.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        public MonoBehaviour owner;
        
        public bool TryGetOwner<T>(out T owner) where T : MonoBehaviour
        {
            if (this.owner is T requestedObject)
            {
                owner = requestedObject;
                return true;
            }

            owner = null;
            return false;
        }

        public static bool TryToGetOwner<T>(GameObject other, out T owner) where T : MonoBehaviour
        {
            if (other.TryGetComponent(out Hitbox hitbox) && hitbox.TryGetOwner(out owner))
            {
                return true;
            }
            
            owner = null;
            return false;
        }
    }
}