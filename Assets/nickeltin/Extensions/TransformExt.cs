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

        /// <summary>
        /// Transforms rotation form local space to world space
        /// </summary>
        public static Quaternion TransformQuaternion(this Transform transform, Quaternion localRotation)
        {
            return transform.rotation * localRotation;
        }

        /// <summary>
        /// Transforms rotation form world space to local space
        /// </summary>
        public static Quaternion InverseTransformQuaternion(this Transform transform, Quaternion worldRotation)
        {
            return Quaternion.Inverse(transform.rotation) * worldRotation;
        }

        public static Vector2 GetViewportPosition(this Transform transform, Camera cam)
        {
            return transform.position.ToViewportPosition(cam);
        }

        public static Vector2 GetLocalPositionInRect(this Transform transform, Camera cam, RectTransform rect)
        {
            return transform.position.ToLocalPositionInRect(cam, rect);
        }

        public static Transform[] GetAllChilds(this Transform transform)
        {
            Transform[] childs = new Transform[transform.childCount];
            for (int i = transform.childCount - 1; i >= 0; i--) childs[i] = transform.GetChild(i);
            return childs;
        }
    }
}