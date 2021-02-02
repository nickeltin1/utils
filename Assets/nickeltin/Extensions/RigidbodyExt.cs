using System;
using System.Collections;
using UnityEngine;

namespace nickeltin.Extensions
{
    public static class RigidbodyExt
    {
        public static void MoveTween(this Rigidbody rigidbody, MonoBehaviour parent, Vector3 to, 
            float time, Action onComplete = null)
        {
            IEnumerator Move()
            {
                float fixedFramesCount = time / Time.fixedDeltaTime;
                Vector3 position = rigidbody.transform.position;
                Vector3 direction = to - position;
                Vector3 positionChange = direction / fixedFramesCount;

                for (int i = 1; i <= fixedFramesCount; i++)
                {
                    rigidbody.MovePosition(position + (positionChange * i));
                    yield return new WaitForFixedUpdate();
                }

                onComplete?.Invoke();
            }

            parent.StartCoroutine(Move());
        }
    }
}