using System;

namespace nickeltin.Localization.Utilities
{
    [Serializable]
    public class GoogleTranslateResponse
    {
        public string TranslatedText;
        public string DetectedSourceLanguage;

        public GoogleTranslateResponse()
        {
        }

        public GoogleTranslateResponse(string translatedText)
        {
            TranslatedText = translatedText;
        }
    }
}
