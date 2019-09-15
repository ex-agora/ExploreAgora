using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;
using ArabicSupport;
[ExecuteInEditMode]
public class LanguageTextSupport : MonoBehaviour
{
    Text text;
    [SerializeField] List<LanguageHandler> languages;
    void Start()
    {
        text = this.GetComponent<Text>();
        if (text == null)
            Destroy(this);
        if (LanguageManager.Instance == null)
        {
            GameObject manger = new GameObject();
            manger.name = "LanguageManager";
            manger.AddComponent<LanguageManager>();
            Instantiate(manger);
        }

        SetSubscraption();
        SetCurrentLanguageText();
    }
    private void OnEnable()
    {
        if (languages == null)
        {
            languages = new List<LanguageHandler>();
            CreateLanguageText();
        }
        

    }
    void SetCurrentLanguageText() {
        var s = languages[LanguageManager.Instance.SelectedLanguage].TranslatedText;
        if (languages[LanguageManager.Instance.SelectedLanguage].Type == LanguageType.RTL && (!string.IsNullOrEmpty(s)))
            s = ArabicFixer.Fix(s,false,false);
        text.text = s;
        text.font = languages[LanguageManager.Instance.SelectedLanguage].LanguageFont;
    }
    public void SetSubscraption() {
        LanguageManager.Instance.addLanguageEvent += AddLanguage;
        LanguageManager.Instance.removeLanguageEvent += RemoveLanguage;
        LanguageManager.Instance.languageChangedEvent += SetCurrentLanguageText;
    }
    private void OnDisable()
    {
        if (LanguageManager.Instance == null)
            return;
        LanguageManager.Instance.addLanguageEvent -= AddLanguage;
        LanguageManager.Instance.removeLanguageEvent -= RemoveLanguage;
        LanguageManager.Instance.languageChangedEvent -= SetCurrentLanguageText;
    }
    void AddLanguage(List<LanguageConfig> languagesCon) {
        foreach (var i in languagesCon)
        {
            CreateLanguageHandler(i);
        }
    }
    void RemoveLanguage(List<int> indeices) {
        foreach (var i in indeices)
        {
            languages.RemoveAt(i);
        }
    }
    void CreateLanguageHandler(LanguageConfig config) {
        var lang = new LanguageHandler();
        lang.LanguageName = config.LanguageName.ToString();
        lang.Type = config.Type;
        languages.Add(lang);
    }
    void CreateLanguageText() {
        foreach (var i in LanguageManager.Instance.Configs)
        {
            CreateLanguageHandler(i);
            
        }
    }
 
}
