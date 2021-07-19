using System;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Runtime.Other.Popups
{
    [Serializable]
    public class PopupEventData
    {
        public bool overrideTweeningSettings; //if
        public PopupTweenData tweenSettings; //show
        
        public bool useRandomText; //if
        [SerializeField] private string[] _randomText; //show
        public string text; //else show
        
        public bool usesWorldPos; //if
        public Vector3 worldPos; //show
        
        public Sprite sprite;
        public Color color = Color.white;
        
        [SerializeField, Tooltip("In viewport size"), Range(0, 1)] private float _width = 0.1f;

        
        
        public string randomText => _randomText.Length > 0 ? _randomText.GetRandom() : "NULL";
        public float width => Screen.width * _width;

#if UNITY_EDITOR
        public static string rand_text_prop_name => nameof(_randomText);
        public static string width_prop_name => nameof(_width);
#endif
    }
}