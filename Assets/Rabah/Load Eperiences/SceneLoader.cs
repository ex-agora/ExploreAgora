using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator ScreenLodAnim;
    string _scene;
    int _index;
    // Start is called before the first frame update
    void Start()
    {
        ScreenLodAnim.SetBool("StartFade", true);
    }
    public void LoadExperience(string SceneName)
    {
        _scene = SceneName;
        _index = -1;
        ScreenLodAnim.SetBool("Loading", true);

    }
    public void LoadExperience(int Index)
    {
        _index = Index;
        _scene = string.Empty;
        ScreenLodAnim.SetBool("Loading", true);

    }
    public void LoadExperienceScene()
    {
        if (_scene != string.Empty)
            SceneManager.LoadSceneAsync(_scene);
        else if (_index != -1)
            SceneManager.LoadSceneAsync(_index);

    }
}
