using System;
using nickeltin.Extensions.Attributes;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    [RequireComponent(typeof(Renderer))]
    public class PropertyBlockSetter : MonoBehaviour
    {
        [Serializable]
        private enum BaseProperties
        {
            Custom, _Color, _MainTex
        }

        [SerializeField] private BaseProperties _basePropertyType = BaseProperties._Color;
        [SerializeField, ShowIf("_usesCustomProp")] private string _propertyName = "_Color";
        [SerializeField] private int _materialId;
        private bool _usesCustomProp => _basePropertyType == BaseProperties.Custom;
        
        private Renderer _renderer;
        private int _property;
        private MaterialPropertyBlock _propertyBlock;
        private bool _initialized = false;

        public MaterialPropertyBlock PropertyBlock => _propertyBlock;
        
        public void Init()
        {
            _renderer = GetComponent<Renderer>();
            _property = Shader.PropertyToID(_usesCustomProp ? _propertyName : _basePropertyType.ToString());
            _propertyBlock = new MaterialPropertyBlock();
            _initialized = true;
        }
        
        private void Awake()
        {
            if(!_initialized) Init();
        }

        public void SetColor(Color color)
        {
            _propertyBlock.SetColor(_property, color);
            _renderer.SetPropertyBlock(_propertyBlock, _materialId);
        }

        public void SetFloat(float f)
        {
            _propertyBlock.SetFloat(_property, f);
            _renderer.SetPropertyBlock(_propertyBlock, _materialId);
        }

        public void SetTexture(Texture tex)
        {
            _propertyBlock.SetTexture(_property, tex);
            _renderer.SetPropertyBlock(_propertyBlock, _materialId);
        }
    }
}