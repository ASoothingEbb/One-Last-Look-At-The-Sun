using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DeathScreen : MonoBehaviour
{

    public Text text;
    public Image whiteScreen;
    public AudioSource deathKnell;
    bool waiting = true;
    bool notDone = true;
    void Start()
    {
        StartCoroutine(FadeInText());
        deathKnell.Play();
    }

    // Update is called once per frame
    void Update()
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
                StartCoroutine(PlayAgain());
            }
        }

    }

    public IEnumerator FadeInText()
    {
        for (float i = 0; i <= 0.5; i += Time.deltaTime)
        {
            text.color = new Color(0.3f, 0.3f, 0.3f, i/ 0.5f);
            yield return null;
        }
    }

    public IEnumerator PlayAgain()
    {
        for (float i = 0; i <= 0.5; i += Time.deltaTime) 
        {
            whiteScreen.color = new Color(1,1,1, i/0.5f);
            yield return null;
        }
        SceneManager.LoadScene("game");
    }
}
