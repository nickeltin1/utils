using UnityEngine;
using SimpleMan.Extensions.UnityEditor;
 
 
namespace SimpleMan.EventSystem
{ 
     [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Event /Empty")]
     public class GameEvent : GameEventBase
     {
         [Button]
         private void MakeInvoke()
         { 
             Invoke();
         } 
     }
}
