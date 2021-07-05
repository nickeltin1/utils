using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    public static class Input
    {
        private class Engine : MonoBehaviour
        {
            private void Update()
            {
                pointerPosition = ScreenPointToViewport(UnityEngine.Input.mousePosition);
                pointerDelta = pointerPosition - lastPointerPosition;

                if (PointerPressed()) lastPointerSwipedPosition = pointerPosition;
                else if (PointerPressing())
                {
                    pointerSwipeDelta = pointerPosition - lastPointerSwipedPosition;
                    lastPointerSwipedPosition = pointerPosition;
                }
                else if (PointerUp()) pointerSwipeDelta = Vector2.zero;


                lastPointerPosition = pointerPosition;
                pointerOffsetFromCenter = (lastPointerPosition * 2).SubNoRef(1, 1);
                pointerPressedLastFrame = PointerPressing();
            }
        }

        static Input()
        {
            var engine = new GameObject("input").AddComponent<Engine>();
            engine.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        private static Vector2 lastPointerSwipedPosition;

        public const float SWIPE_THRESHOLD = 0.01f;

        public static float swipeThreshold = SWIPE_THRESHOLD;
        
        /// <summary>Viewport rect [0 - 1]</summary>
        public static Vector2 lastPointerPosition { get; private set; }
        
        /// <summary>Viewport rect [-1 - 1] because its direction</summary>
        public static Vector2 pointerDelta { get; private set; }
        
        /// <summary>Viewport rect [-1 - 1] because its direction</summary>
        public static Vector2 pointerSwipeDelta { get; private set; }
        
        public static bool pointerPressedLastFrame { get; private set; }
        
        /// <summary>[-1 - 1] From screen center </summary>
        public static Vector2 pointerOffsetFromCenter { get; private set; }

        /// <summary>Screen pos in pixels </summary>
        public static Vector2 pointerScreenPosition => UnityEngine.Input.mousePosition;

        /// <summary>Viewport rect [0 - 1]</summary>
        public static Vector2 pointerPosition { get; private set; }
        
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

        public static bool GetKeyDown(KeyCode key) => UnityEngine.Input.GetKeyDown(key);
        public static bool GetKey(KeyCode key) => UnityEngine.Input.GetKey(key);
        public static bool GetKeyUp(KeyCode key) => UnityEngine.Input.GetKeyUp(key);
    }
}