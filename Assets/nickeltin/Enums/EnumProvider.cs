using System;
using UnityEngine;

namespace nickeltin.Enums
{
    public abstract class EnumProvider<T> : MonoBehaviour where T : Enum
    {
        public T Enum;
    }
}