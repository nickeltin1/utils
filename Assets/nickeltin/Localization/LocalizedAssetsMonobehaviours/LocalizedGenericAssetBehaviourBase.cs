using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace nickeltin.Localization
{
    public abstract class LocalizedGenericAssetBehaviourBase : LocalizedAssetBehaviour
    {
        [SerializeField]
        protected Component m_Component;

        [SerializeField]
        protected string m_Property = "";

        protected PropertyInfo m_PropertyInfo;

        protected virtual void Awake()
        {
            InitializePropertyIfNeeded();
        }

        protected virtual void InitializePropertyIfNeeded()
        {
            if (m_PropertyInfo == null && m_Component)
            {
                m_PropertyInfo = FindProperty(m_Component, m_Property);
            }
        }

        public abstract PropertyInfo FindProperty(Component component, string name);
        public abstract List<PropertyInfo> FindProperties(Component component);
    }
}
