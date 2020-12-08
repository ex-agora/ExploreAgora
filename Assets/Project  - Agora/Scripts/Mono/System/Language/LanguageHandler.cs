using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LanguageHandler
{
    #region Fields
    [SerializeField] Font languageFont;
    [SerializeField] [ReadOnly] string languageName;
    [SerializeField] string translatedText;
    [SerializeField] [ReadOnly] LanguageType type;
    #endregion Fields

    #region Properties
    public Font LanguageFont { get => languageFont; set => languageFont = value; }
    public string LanguageName { get => languageName; set => languageName = value; }
    public string TranslatedText { get => translatedText; set => translatedText = value; }
    public LanguageType Type { get => type; set => type = value; }
    #endregion Properties
}
