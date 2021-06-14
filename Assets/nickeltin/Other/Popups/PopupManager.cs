using nickeltin.Editor.Attributes;
using nickeltin.Editor.Utility;
using nickeltin.Extensions;
using nickeltin.ObjectPooling;
using nickeltin.Singletons;
using nickeltin.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace nickeltin.Other.Popups
{
    [CreateAssetMenu(menuName = MenuPathsUtility.otherMenu + nameof(PopupManager))]
    public class PopupManager : SOSingleton<PopupManager>
    {
        [SerializeField] private PopupBehaviour _popupPrefab;
        [SerializeField] private PopupTweenData _defaultTweening;
        [SerializeField] private PopupEvent[] _events;

        private Canvas _popupsCanvas;
        private Pool<PopupBehaviour> _popupPool;


        public override bool Initialize()
        {
            if (base.Initialize())
            {
                _popupsCanvas = new GameObject("popups").AddComponent<Canvas>();
                _popupsCanvas.gameObject.AddComponent<CanvasScaler>();
                _popupsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                DontDestroyOnLoad(_popupsCanvas.gameObject);
                _popupPool = new Pool<PopupBehaviour>(_popupPrefab, _popupsCanvas.transform);
                
                SubscribeForEvents();
                return true;
            }

            return false;
        }

        public override bool Destruct()
        {
            if (base.Destruct())
            {
                UnsubscribeFromEvents();
                return true;
            }

            return false;
        }

        public void DoPopup(PopupEvent.Data data)
        {
            DoPopup(data, data.overrideTweeningSettings ? data.tweenSettings : _defaultTweening);
        }

        public void DoPopup(PopupEvent.Data data, PopupTweenData tweenData)
        {
            var popup = _popupPool.Get();
            var rect = popup.rectTransform;
            float dur = tweenData.duration;
            
            popup.text.color = data.color;
            popup.text.text = data.text;
            popup.canvasGroup.alpha = 0;
            rect.sizeDelta = new Vector2(data.width, rect.sizeDelta.y);

            //TODO: replace with some kind of scr.obj. refrence for camera
            
            rect.anchoredPosition = data.usesWorldPos
                ? data.worldPos.FromWorldToCanvasPosition(Camera.main, _popupsCanvas)
                : Vector2.zero;

            LeanTween.move(rect, rect.anchoredPosition + tweenData.offset, dur).setEase(tweenData.positionCurve)
                .setOnComplete(popup.ReturnToPool);
            
            LeanTween.alphaCanvas(popup.canvasGroup, 1, dur).setEase(tweenData.alphaCurve);
        }

        
        private void SubscribeForEvents() => _events.ForEach(e => e.BindEvent(DoPopup));

        private void UnsubscribeFromEvents() => _events.ForEach(e => e.UnbindEvent(DoPopup));

#if UNITY_EDITOR
        [ContextMenu("Refresh AudioEvents"), Button("Refresh Events", EButtonEnableMode.Editor)]
        private void RefreshEventsList() => _events = Resources.FindObjectsOfTypeAll<PopupEvent>();

        [ContextMenu("Refresh AudioEvents", true)]
        private bool RefreshEvents_Validator() => !Application.isPlaying;
#endif
    }
}