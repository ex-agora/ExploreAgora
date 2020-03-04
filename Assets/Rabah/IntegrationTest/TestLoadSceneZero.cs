using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoadSceneZero : MonoBehaviour
{
    public void back ()
    {
        
        SceneManager.LoadSceneAsync ("KareemFirstScene");
    }
}
