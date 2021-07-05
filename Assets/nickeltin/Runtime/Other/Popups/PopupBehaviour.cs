using nickeltin.Extensions;
using nickeltin.Runtime.ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nickeltin.Runtime.Other.Popups
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class PopupBehaviour : PoolItem<PopupBehaviour>
    {
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;
        public TMP_Text text;
        public Image image;

        private void OnValidate()
        {
            ComponentExt.Cache(ref rectTransform, gameObject);
            ComponentExt.Cache(ref canvasGroup, gameObject);
        }
    }
}