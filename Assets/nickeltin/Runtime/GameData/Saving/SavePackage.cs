using System.Collections.Generic;
using nickeltin.Extensions.Attributes;
using nickeltin.Runtime.Utility;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    /// <summary>
    /// Packs all your saves into one file, all of them is added to saves database, as well as Package itself.
    /// </summary>
    [CreateAssetMenu(menuName = MenuPathsUtility.savingMenu + nameof(SavePackage))]
    public sealed class SavePackage : Saveable<object[]>
    {
        [SerializeField, HideInInspector] private SaveableBase[] _saves;
        public IReadOnlyList<SaveableBase> Saves => _saves;
        
        protected override void Deserialize(SaveSystem saveSystem, object[] obj)
        {
            for (int i = 0; i < _saves.Length; i++)
            {
                _saves[i].SetFileWithoutType(saveSystem, obj[i]);
            }
        }

        protected override object[] Serialize()
        {
            object[] obj = new object[_saves.Length];

            for (int i = 0; i < _saves.Length; i++) obj[i] = _saves[i].GetFileWithoutType();

            return obj;
        }

        public override bool Register(SaveSystem saveSystem)
        {
            bool value = base.Register(saveSystem);
            
            foreach (var save in _saves)
            {
                if(!save.Register(saveSystem)) value = false;
            }
            return value;
        }

        public override void SetLoadState(bool state)
        {
            base.SetLoadState(state);
            foreach (var save in _saves)
            {
                save.SetLoadState(state);
            }
        }
        
        public override void LoadDefault()
        {
            _saves.ForEach(save => save.LoadDefault());
        }

#if UNITY_EDITOR
        public static string saves_prop_name => nameof(_saves);
#endif
    }
}