using UnityEngine;

namespace nickeltin.GameData.Saving
{
    public abstract class MonoSaveable : MonoBehaviour
    {
        /// <summary>
        /// Called before save
        /// </summary>
        /// <returns>Object with all save data</returns>
        public abstract object Save();
        
        /// <summary>
        /// Called after load
        /// </summary>
        /// <param name="obj">Object with all save data</param>
        public abstract void Load(object obj);
    }
}