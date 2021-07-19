using nickeltin.Extensions;
using nickeltin.Runtime.GameData.VariablesRefrences;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    [RequireComponent(typeof(Collider))]
    public class ColliderFiller : MonoBehaviour
    {
        public enum DrawType { DontDraw, Always, OnlyWhenSelected }
        
        public Collider collider;
        public VariableRef<Color> color = Color.green;
        public DrawType drawType = DrawType.OnlyWhenSelected;

#if UNITY_EDITOR
        private void OnValidate() => ComponentExt.Cache(ref collider, gameObject);
# endif
    }
}