using System;
using UnityEngine;

namespace nickeltin.Runtime.Other.Popups
{
    [Serializable]
    public class PopupTweenData
    {
        public float duration = 1;
        public Vector2 offset = new Vector2(0, 400);
        public AnimationCurve positionCurve =  AnimationCurve.Linear(0,0, 1, 1);
        public AnimationCurve alphaCurve = AnimationCurve.Linear(0,0, 1, 1);
    }
}