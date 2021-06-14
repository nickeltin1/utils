using UnityEngine;
using UnityEngine.UI;

namespace nickeltin.Extensions
{
    public static class GraphicExt
    {
        private static Sprite m_emptyTexture;
        public static Sprite TransparentTexture
        {
            get
            {
                if(m_emptyTexture == null) m_emptyTexture = Sprite.Create(new Texture2D(100, 100),
                    new Rect(-100, -100, 100, 100), Vector2.one / 2);
                return m_emptyTexture;
            }
        }

        public static void SetEmptyTexture(this Image img) => img.sprite = TransparentTexture;
    }
}