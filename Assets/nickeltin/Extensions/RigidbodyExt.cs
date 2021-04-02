using System;
using System.Collections;
using UnityEngine;

namespace nickeltin.Extensions
{
    public static class RigidbodyExt
    {
        public static void MoveToPoint(this Rigidbody rigidbody, MonoBehaviour parent, Vector3 to, 
            float time, Action onComplete = null)
        {
           MoveToPoint_Internal(rigidbody, parent, to, time, onComplete, rigidbody.MovePosition);
        }
        
        public static void MoveToPointKinematic(this Rigidbody rigidbody, MonoBehaviour parent, Vector3 to, 
            float time, Action onComplete = null)
        {
            MoveToPoint_Internal(rigidbody, parent, to, time, onComplete, v => rigidbody.transform.position = v);
        }

        private static void MoveToPoint_Internal(Rigidbody rigidbody, MonoBehaviour parent, Vector3 to, 
            float time, Action onComplete = null, Action<Vector3> onUpdate = null)
        {
            IEnumerator Move()
            {
                float fixedFramesCount = time / Time.fixedDeltaTime;
                Vector3 position = rigidbody.transform.position;
                Vector3 direction = to - position;
                Vector3 positionChange = direction / fixedFramesCount;

                for (int i = 1; i <= fixedFramesCount; i++)
                {
                    onUpdate?.Invoke(position + (positionChange * i));
                    yield return new WaitForFixedUpdate();
                }
                onComplete?.Invoke();
            }
            parent.StartCoroutine(Move());
        }
    }
}