using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[CreateAssetMenu(fileName = "LanguageConfig", menuName = "SO/Language/LanguageConfig", order =0)]
public class LanguageConfig : ScriptableObject
{
    #region Fields
    static List<SystemLanguage> allLanguages = new List<SystemLanguage>();
    [SerializeField] SystemLanguage languageName;
    [SerializeField] LanguageType type;
    #endregion Fields

    #region Properties
    public SystemLanguage LanguageName { get => languageName; set => languageName = value; }
    public LanguageType Type { get => type; set => type = value; }
    #endregion Properties


}
