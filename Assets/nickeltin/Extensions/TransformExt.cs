using UnityEngine;

namespace nickeltin.Extensions.TransformComponent
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
    }
}