using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.UI
{
    [RequireComponent(typeof(Menu))]
    public class Menu : MonoBehaviour
    {
        [Serializable]
        public enum MenuType
        {
            Overlay, Main
        }
        
        [SerializeField, Tooltip("Overlay - Do not disables other menus on open. Main - Disables other menus on open")] 
        private MenuType m_type;

        [SerializeField] private UnityEvent m_onOpen;
        [SerializeField] private UnityEvent m_onClose;

        private bool m_opened => gameObject.activeInHierarchy;
        
        protected virtual void Open()
        {
            if(m_opened) return;
            
            gameObject.SetActive(true);
            m_onOpen.Invoke();
        }

        protected virtual void Close()
        {
            if(!m_opened) return;
            
            m_onClose.Invoke();
            gameObject.SetActive(false);
            
        }

        public static void OpenMenu(Menu target, IEnumerable<Menu> others)
        {
            if (target.m_type == MenuType.Main)
            {
                foreach (var menu in others)
                {
                    if(!menu.Equals(target)) menu.Close();
                }
            }
            
            target.Open();
            target.transform.SetSiblingIndex(target.transform.parent.childCount-1);
        }

        public static void CloseMenu(Menu target)
        {
            target.Close();
        }
    }
}