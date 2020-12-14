using UnityEngine;
using UnityEngine.EventSystems;

namespace nickeltin.Extensions
{
    public static class CameraExt
    {
        /// <returns>Returns true if hitted something</returns>
        public static bool RaycastFromScreenpoint(this Camera cam, Vector2 screenPoint, out RaycastHit hit, 
            float range = 1000f, LayerMask? mask = null, bool preventUiIntersecting = false)
        {
            Ray ray = cam.ScreenPointToRay(screenPoint);

            bool hasHit;
            
            if (mask.HasValue) hasHit = Physics.Raycast(ray, out hit, range, (LayerMask) mask);
            else hasHit = Physics.Raycast(ray, out hit, range);

            if (!preventUiIntersecting) return hasHit;
            return hasHit && !EventSystem.current.IsPointerOverGameObject();
        }
        
        /// <returns>Returns number of hits</returns>
        public static int RaycastFromScreenpointAll(this Camera cam, Vector2 screenPoint, RaycastHit[] hits, 
            float range = 1000f, LayerMask? mask = null)
        {
            Ray ray = cam.ScreenPointToRay(screenPoint);

            int hitCount;
            
            if (mask.HasValue) hitCount = Physics.RaycastNonAlloc(ray, hits, range, (LayerMask) mask);
            else hitCount = Physics.RaycastNonAlloc(ray, hits, range);
            return hitCount;
        }
    }
}