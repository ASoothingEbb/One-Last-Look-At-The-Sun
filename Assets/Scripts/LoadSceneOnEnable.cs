using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnable : MonoBehaviour
{
    public string scene;
    private void OnEnable()
    {
        SceneManager.LoadScene(scene);
    }
}
