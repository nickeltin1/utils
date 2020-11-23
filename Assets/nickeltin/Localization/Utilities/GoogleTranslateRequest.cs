using System;
using UnityEngine;

namespace nickeltin.Localization.Utilities
{
    [Serializable]
    public class GoogleTranslateRequest
    {
        public SystemLanguage Source;
        public SystemLanguage Target;
        public string Text;

        public GoogleTranslateRequest()
        {
        }

        public GoogleTranslateRequest(SystemLanguage source, SystemLanguage target, string text)
        {
            Source = source;
            Target = target;
            Text = text;
        }
    }
}
