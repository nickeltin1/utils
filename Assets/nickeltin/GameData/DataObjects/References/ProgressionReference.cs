using System;
using nickeltin.Other;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public class ProgressionReference : DataObjectReference<ProgressionObject, AnimationCurve>
    {
        public float this [float progress] => m_useConstant ? m_constantValue.Evaluate(progress) : m_dataObject[progress];
    }
}