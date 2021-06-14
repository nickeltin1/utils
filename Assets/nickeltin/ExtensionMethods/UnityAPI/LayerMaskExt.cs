using UnityEngine;

namespace nickeltin.Extensions
{
    public static class LayerMaskExt
    {
        public static bool ContainsLayer(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}