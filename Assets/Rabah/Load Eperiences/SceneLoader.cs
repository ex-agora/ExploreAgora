using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
/// Fade black image when open the scene and when go to other scene start loading animation
/// Add this script in canvas and animator.
/// the sort order of canvas must be the heighest in the scene. 
/// </summary>
public class SceneLoader : MonoBehaviour
{

    #region Fields
    int _index;
    string _scene;
    [SerializeField] Animator ScreenLodAnim;
    [SerializeField] TextMeshProUGUI loadTxt;
    static SceneLoader instance;

    public static SceneLoader Instance { get => instance; set => instance = value; }
    #endregion Fields

    #region Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void ChangeInstance() {
        instance = this;
        ScreenLodAnim.SetBool("StartFade", true);
    }
    //Set Experience name and reset index
    public void LoadExperience(string SceneName)
    {
        loadTxt.text = $"Loading...";
        _scene = SceneName;
        _index = -1;
        ScreenLodAnim.SetBool("Loading", true);

    }
    public void Loading(bool isFade = false)
    {
        loadTxt.text = $"Loading...";
        ScreenLodAnim.SetBool("Loading", true);
        if(isFade)
            ScreenLodAnim.SetBool("StartFade", true);
    }
    //Set Experience index and reset name
    public void LoadExperience(int Index)
    {
       
        _index = Index;
        _scene = string.Empty;
        ScreenLodAnim.SetBool("Loading", true);
    }
    public void UpdatePrecent(float p) { 
        ScreenLodAnim.SetBool("Loading", true);
        loadTxt.text = $"Loading:\n{p.ToString("P")}";
    }
    public void HideLoading() {
        ScreenLodAnim.SetBool("StartFade", true);
        ScreenLodAnim.SetTrigger("IsHide");
        ScreenLodAnim.SetBool("Loading", false);
    }
    //Load Experience by index or name
    public void LoadExperienceTrigger()
    {
        if (_scene != string.Empty)
        {
            //SceneManager.LoadSceneAsync(_scene);
            AddressableScenesManager.Instance.LoadScene(_scene);
        }
        else if (_index != -1)
            SceneManager.LoadSceneAsync(_index);

    }
    //private void Update()
    //{
    //    Debug.LogWarning(ScreenLodAnim.GetCurrentAnimatorClipInfo(0).Length>0? ScreenLodAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name:ScreenLodAnim.GetAnimatorTransitionInfo(0).ToString());
    //    Debug.LogWarning("Loading:::"+ScreenLodAnim.GetBool("Loading"));
    //    Debug.LogWarning("fade:::"+ScreenLodAnim.GetBool("StartFade"));
    //    //Debug.Log("hide:::"+ScreenLodAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToString());
    //}

    //Scene Name
    //Scene Index
    // Start is called before the first frame update
    void Start()
    {
     //Fade image at the satrt of scene
        //ScreenLodAnim.SetBool("StartFade", true);
    }
    #endregion Methods
}
