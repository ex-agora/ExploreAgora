using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[ExecuteInEditMode]
public class LanguageManager : MonoBehaviour
{
    #region Fields
    public System.Action<List<LanguageConfig>> addLanguageEvent;
    public System.Action languageChangedEvent;
    public System.Action<List<int>> removeLanguageEvent;
    static LanguageManager instance;
    [SerializeField] List<LanguageConfig> configs;
    List<int> removedIndeices;
    int removedIndex;
    int selectedLanguage;
    #endregion Fields

    #region Properties
    public static LanguageManager Instance { get => instance; set => instance = value; }
    public List<LanguageConfig> Configs { get => configs; set => configs = value; }
    public int SelectedLanguage { get => selectedLanguage; set { selectedLanguage = value; UpdateLanguage(); } }
    #endregion Properties

    #region Methods
    public void LanguagesUpdate()
    {
        var temp = Resources.LoadAll<LanguageConfig>("SO/Language/");
        if (temp.Length > Configs.Count)
        {
            var r = temp.Select(i => i).Where(i => !configs.Contains(i)).ToList();
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

    bool CheckMissingReferences(LanguageConfig temp)
    {
        bool up = true;
        try
        {
            if (temp == null)
                up = false;
            temp?.GetInstanceID();
        }
        catch
        {
            up = false;
        }
        return up;

    }

    bool RemoveMissingReferences(LanguageConfig config)
    {
        bool isMaissing = !CheckMissingReferences(config);
        removedIndex++;
        if (isMaissing)
            removedIndeices.Add(removedIndex);
        return isMaissing;
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
    void UpdateLanguage() {
        languageChangedEvent?.Invoke();
    }
    #endregion Methods

}
