using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// Packs all your saves into one file, all of them is added to saves database, as well as Package itself.
    /// </summary>
    [CreateAssetMenu(menuName = "GameData/Saving/SavePackage")]
    public sealed class SavePackage : Saveable<object[]>
    {
        [SerializeField] private List<SaveableBase> m_saves;
        public IReadOnlyList<SaveableBase> Saves => m_saves;
        
        protected override void Deserialize(object[] obj)
        {
            for (int i = 0; i < m_saves.Count; i++)
            {
                m_saves[i].SetFileWithoutType(obj[i]);
            }
        }

        protected override object[] Serialize()
        {
            object[] obj = new object[m_saves.Count];

            for (int i = 0; i < m_saves.Count; i++)
            {
                obj[i] = m_saves[i].GetFileWithoutType();
            }

            return obj;
        }

        public override bool Register()
        {
            bool value = base.Register();
            
            foreach (var save in m_saves)
            {
                if(!save.Register()) value = false;
            }

            return value;
        }

        public override void SetLoadState(bool state)
        {
            base.SetLoadState(state);
            foreach (var save in m_saves)
            {
                save.SetLoadState(state);
            }
        }
        
        protected override void LoadDefault()
        {
            foreach (var save in m_saves) save.Load(true);
        }
    }
}