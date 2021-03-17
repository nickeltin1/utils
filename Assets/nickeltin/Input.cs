using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Other
{
    public static class Input
    {
        private class Engine : MonoBehaviour
        {
            private void Update()
            {
                pointerDelta = ScreenPointToViewport(UnityEngine.Input.mousePosition) - lastPointerPos;
                lastPointerPos = ScreenPointToViewport(UnityEngine.Input.mousePosition);
                pointerPressedLastFrame = PointerPressing();
            }
        }

        static Input()
        {
            m_engine = new GameObject("input").AddComponent<Engine>();
            m_engine.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
        
        private static Engine m_engine;

        public const float SWIPE_THRESHOLD = 0.01f;

        public static float swipeThreshold = SWIPE_THRESHOLD;
        
        /// <summary>Viewport rect [0 - 1]</summary>
        public static Vector2 lastPointerPos { get; private set; }
        
        /// <summary>Viewport rect [-1 - 1] because its direction</summary>
        public static Vector2 pointerDelta { get; private set; }

        public static bool pointerPressedLastFrame { get; private set; }

        public static Vector2 pointerPos => UnityEngine.Input.mousePosition;
        
        /// <param name="screenPoint">In pixels</param>
        public static Vector2 ScreenPointToViewport(Vector2 screenPoint)
        {
            return new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
        }
        
        /// <param name="screenPoint">In pixels</param>
        public static Vector2 ViewportPointToScreen(Vector2 viewportPoint)
        {
            return new Vector2(viewportPoint.x * Screen.width, viewportPoint.y * Screen.height);
        }

        public static bool PointerPressed() => UnityEngine.Input.GetMouseButtonDown(0);
        public static bool PointerPressing() => UnityEngine.Input.GetMouseButton(0);
        public static bool PointerUp() => UnityEngine.Input.GetMouseButtonUp(0);
        
        /// <param name="threshold">Viewport coordinates [0 - 1]</param>
        /// <param name="axis">One vector</param>
        public static bool Swipe(Vector3 axis, float threshold = SWIPE_THRESHOLD)
        {
            threshold.Clamp01();
            return pointerPressedLastFrame && PointerPressing() && Vector3.Scale(pointerDelta, axis).magnitude > threshold;
        }
        
        /// <param name="threshold">Viewport coordinates [0 - 1]</param>
        public static bool HorizontalSwipe(float threshold) => Swipe(Vector3.right, threshold);

        /// <param name="threshold">Viewport coordinates [0 - 1]</param>
        public static bool VerticalSwipe(float threshold) => Swipe(Vector3.up, threshold);

        /// <param name="threshold">Viewport coordinates [0 - 1]</param>
        public static bool UpSwipe(float threshold)
        {
            return VerticalSwipe(threshold) && pointerDelta.y > threshold;
        }

        /// <param name="threshold">Viewport coordinates [0 - 1]</param>
        public static bool BottomSwipe(float threshold)
        {
            return VerticalSwipe(threshold) && pointerDelta.y < threshold;
        }

        /// <param name="threshold">Viewport coordinates [0 - 1]</param>
        public static bool LeftSwipe(float threshold)
        {
            return HorizontalSwipe(threshold) && pointerDelta.x < threshold;
        }

        /// <param name="threshold">Viewport coordinates [0 - 1]</param>
        public static bool RightSwipe(float threshold)
        {
            return HorizontalSwipe(threshold) && pointerDelta.x > threshold;
        }
        
        
        public static bool HorizontalSwipe() => HorizontalSwipe(swipeThreshold);
        
        public static bool VerticalSwipe() => VerticalSwipe(swipeThreshold);

        public static bool UpSwipe() => UpSwipe(swipeThreshold);

        public static bool BottomSwipe() => BottomSwipe(swipeThreshold);

        public static bool LeftSwipe() => LeftSwipe(swipeThreshold);

        public static bool RightSwipe() => RightSwipe(swipeThreshold);
    }
}