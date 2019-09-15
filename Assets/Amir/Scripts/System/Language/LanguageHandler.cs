using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LanguageHandler
{
    [SerializeField] [ReadOnly] string languageName;
    [SerializeField] string translatedText;
    [SerializeField] Font languageFont;
    [SerializeField] [ReadOnly] LanguageType type;

    public string LanguageName { get => languageName; set => languageName = value; }
    public string TranslatedText { get => translatedText; set => translatedText = value; }
    public LanguageType Type { get => type; set => type = value; }
    public Font LanguageFont { get => languageFont; set => languageFont = value; }
}
