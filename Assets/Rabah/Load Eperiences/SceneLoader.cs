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

    [SerializeField] Animator ScreenLodAnim;
    string _scene;//Scene Name
    int _index;//Scene Index
    // Start is called before the first frame update
    void Start()
    {
     //Fade image at the satrt of scene
        ScreenLodAnim.SetBool("StartFade", true);
    }
    //Set Experience name and reset index
    public void LoadExperience(string SceneName)
    {
        _scene = SceneName;
        _index = -1;
        ScreenLodAnim.SetBool("Loading", true);

    }
    //Set Experience index and reset name
    public void LoadExperience(int Index)
    {
        _index = Index;
        _scene = string.Empty;
        ScreenLodAnim.SetBool("Loading", true);

    }
    //Load Experience by index or name
    public void LoadExperienceScene()
    {
        if (_scene != string.Empty)
            SceneManager.LoadSceneAsync(_scene);
        else if (_index != -1)
            SceneManager.LoadSceneAsync(_index);

    }
}
