using UnityEngine;

namespace nickeltin.Extensions
{
    public static class TransformExt
    {
        public static void AllChildrensSetActive(this Transform transform, bool isActive)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
               transform.GetChild(i).gameObject.SetActive(isActive); 
            }
        }

        public static Vector2 GetViewportPosition(this Transform transform, Camera cam)
        {
            return transform.position.ToViewportPosition(cam);
        }

        public static Vector2 GetLocalPositionInRect(this Transform transform, Camera cam, RectTransform rect)
        {
            return transform.position.ToLocalPositionInRect(cam, rect);
        }
    }
}