using System;
using System.Collections;
using UnityEngine;

namespace nickeltin.Extensions
{
    public static class ActionExt
    {
        public static void DelayedInvoke<T>(this Action<T> action, T arg, float t, MonoBehaviour owner)
        {
            IEnumerator Call()
            {
                yield return new WaitForSeconds(t);
                action?.Invoke(arg);
            }

            owner.StartCoroutine(Call());
        }
        
        public static void DelayedInvoke(this Action action, float t, MonoBehaviour owner)
        {
            IEnumerator Call()
            {
                yield return new WaitForSeconds(t);
                action?.Invoke();
            }

            owner.StartCoroutine(Call());
        }
    }
}