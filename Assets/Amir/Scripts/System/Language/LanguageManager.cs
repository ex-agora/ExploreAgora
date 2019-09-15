using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[ExecuteInEditMode]
public class LanguageManager : MonoBehaviour
{
    [SerializeField] List<LanguageConfig> configs;
    static LanguageManager instance;
    int selectedLanguage;
    public static LanguageManager Instance { get => instance; set => instance = value; }
    public List<LanguageConfig> Configs { get => configs; set => configs = value; }
    public int SelectedLanguage { get => selectedLanguage; set { selectedLanguage = value; UpdateLanguage(); } }
    

    List<int> removedIndeices;
    int removedIndex;
    public System.Action<List<LanguageConfig>> addLanguageEvent;
    public System.Action<List<int>> removeLanguageEvent;
    public System.Action languageChangedEvent;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (this.gameObject != null)
        {
            if (Application.isPlaying)
                Destroy(this.gameObject);
            else
                DestroyImmediate(this.gameObject);
        }
    }

    void Start()
    {
        Configs = Configs ?? new List<LanguageConfig>();
        removedIndeices = new List<int>();
        LanguagesUpdate();
        var texts = Resources.FindObjectsOfTypeAll<LanguageTextSupport>();
        foreach (var i in texts)
        {
            i.SetSubscraption();   
        }
    }
    public void LanguagesUpdate() {
        var temp  = Resources.LoadAll<LanguageConfig>("SO/Language/");
        if (temp.Length > Configs.Count) {
            var r = temp.Select(i=>i).Where(i => !configs.Contains(i)).ToList();
            Configs.AddRange(r);
            addLanguageEvent?.Invoke(r);
        }
        else if (temp.Length < Configs.Count)
        {
            var tempR = temp.ToList();
            removedIndeices.Clear();
            removedIndex = -1;
            Configs.RemoveAll(i => RemoveMissingReferences(i));
            removeLanguageEvent?.Invoke(removedIndeices);
        }
    }
    bool RemoveMissingReferences(LanguageConfig config) {
        bool isMaissing = !CheckMissingReferences(config);
        removedIndex++;
        if (isMaissing)
            removedIndeices.Add(removedIndex);
        return isMaissing;
    }
    bool CheckMissingReferences(LanguageConfig temp) {
        bool up = true;
        try {
            if (temp == null)
                up = false;
            temp?.GetInstanceID();
        } catch {
            up = false;
        }
        return up;

    }
    void UpdateLanguage() {
        languageChangedEvent?.Invoke();
    }

}
