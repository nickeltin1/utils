using nickeltin.Runtime.Utility;
using nickeltin.Runtime.GameData.Events;
using UnityEngine;

namespace nickeltin.Runtime.Other.Popups
{
    [CreateAssetMenu(menuName = MenuPathsUtility.otherMenu + nameof(PopupEvent))]
    public class PopupEvent : EventObject<PopupEventData>
    {
        public PopupEventData data
        {
            get => _event.invokeData;
            set => _event.invokeData = value;
        }

        public void Invoke(Vector3 worldPosition) => Invoke(worldPosition, data.text);

        public void Invoke(Vector3 worldPosition, string text)
        {
            Invoke(true, worldPosition, text, false, data.color);
        }
        
        public void Invoke(bool usesWorldPos, Vector3 position, string text, bool usesRandomText, Color color)
        {
            data.usesWorldPos = usesWorldPos;
            data.worldPos = position;
            data.text = text;
            data.useRandomText = usesRandomText;
            data.color = color;
            Invoke();
        }
    }
}