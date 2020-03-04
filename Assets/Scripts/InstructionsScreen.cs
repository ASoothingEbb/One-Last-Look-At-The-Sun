using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InstructionsScreen : MonoBehaviour
{

    public Image whiteScreen;
    bool waiting = true;
    bool notDone = true;
    float time=0;
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2)
        {
            if (notDone)
            {
                if (waiting)
                {
                    if (Input.anyKey)
                    {
                        waiting = false;
                    }
                }
                else
                {
                    notDone = false;
                    StartCoroutine(Play());
                }
            }
        }
    }

    public IEnumerator FadeIn()
    {
        for (float i = 0; i <= 0.5; i += Time.deltaTime)
        {
            whiteScreen.color = new Color(1f, 1f, 1f, 1-(i/ 0.5f));
            yield return null;
        }

        whiteScreen.color = new Color(1f, 1f, 1f, 0);
    }

    public IEnumerator Play()
    {
        for (float i = 0; i <= 0.5; i += Time.deltaTime) 
        {
            whiteScreen.color = new Color(1,1,1, i/0.5f);
            yield return null;
        }
        SceneManager.LoadScene("game");
    }
}
