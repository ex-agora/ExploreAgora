using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    static SceneLoader instance;

    public static SceneLoader Instance { get => instance; set => instance = value; }
    #endregion Fields

    #region Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    //Set Experience name and reset index
    public void LoadExperience(string SceneName)
    {
        _scene = SceneName;
        _index = -1;
        ScreenLodAnim.SetBool("Loading", true);

    }
    public void Loading ()
    {
        ScreenLodAnim.SetBool ("Loading" , true);

    }
    //Set Experience index and reset name
    public void LoadExperience(int Index)
    {
        _index = Index;
        _scene = string.Empty;
        ScreenLodAnim.SetBool("Loading", true);

    }

    //Load Experience by index or name
    public void LoadExperienceTrigger()
    {
        if (_scene != string.Empty)
            SceneManager.LoadSceneAsync(_scene);
        else if (_index != -1)
            SceneManager.LoadSceneAsync(_index);

    }

    //Scene Name
    //Scene Index
    // Start is called before the first frame update
    void Start()
    {
     //Fade image at the satrt of scene
        ScreenLodAnim.SetBool("StartFade", true);
    }
    #endregion Methods
}
