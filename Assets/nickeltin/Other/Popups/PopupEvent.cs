using System;
using nickeltin.Editor.Attributes;
using nickeltin.Editor.Utility;
using nickeltin.Extensions;
using nickeltin.GameData.Events;
using UnityEngine;

namespace nickeltin.Other.Popups
{
    [CreateAssetMenu(menuName = MenuPathsUtility.otherMenu + nameof(PopupEvent))]
    public class PopupEvent : EventObject<PopupEvent.Data>
    {
        [Serializable]
        public class Data
        {
            public bool overrideTweeningSettings;
            public PopupTweenData tweenSettings;
            
            [SerializeField] private string[] _text;
            public Color color;
            public bool usesWorldPos = false;
            public Vector3 worldPos;

            [SerializeField, Tooltip("In viewport size"), Range(0, 1)] private float _width = 0.1f;

            public string text => _text.Length > 0 ? _text.GetRandom() : "NULL";
            public float width => Screen.width * _width;
        }

        public Data data => m_event.invokeData;

        public void Invoke(Vector3 worldPos)
        {
            data.worldPos = worldPos;
            data.usesWorldPos = false;
            Invoke();
        }

        public void Invoke(Transform positionSource) => Invoke(positionSource.position);
    }
}