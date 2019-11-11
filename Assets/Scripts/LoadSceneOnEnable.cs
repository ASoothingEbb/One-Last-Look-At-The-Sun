using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnable : MonoBehaviour
{
    public int sceneNumber;
    private void OnEnable()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
