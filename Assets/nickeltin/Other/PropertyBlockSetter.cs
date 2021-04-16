using UnityEngine;

namespace nickeltin.Other
{
    [RequireComponent(typeof(Renderer))]
    public class PropertyBlockSetter : MonoBehaviour
    {
        [SerializeField] private string m_propertyName = "_Color";
        [SerializeField] private int m_materialId;
        
        private Renderer m_renderer;
        private int m_property;
        private MaterialPropertyBlock m_propertyBlock;

        public MaterialPropertyBlock PropertyBlock => m_propertyBlock;

        private void Awake()
        {
            m_renderer = GetComponent<Renderer>();
            m_property = Shader.PropertyToID(m_propertyName);
            m_propertyBlock = new MaterialPropertyBlock();
        }

        public void SetColor(Color color)
        {
            m_propertyBlock.SetColor(m_property, color);
            m_renderer.SetPropertyBlock(m_propertyBlock, m_materialId);
        }

        public void SetFloat(float f)
        {
            m_propertyBlock.SetFloat(m_property, f);
            m_renderer.SetPropertyBlock(m_propertyBlock, m_materialId);
        }
    }
}