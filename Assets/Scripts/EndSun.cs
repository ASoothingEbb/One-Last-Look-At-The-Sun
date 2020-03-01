using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSun : MonoBehaviour
{
    public float time = 5f;
    public float endSize = 2700f;
    public AudioSource fall;
    public AudioSource boom;

    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Restart());
        Destroy(other.gameObject);
   
    }

    IEnumerator Restart()
    {
        boom.Play();
        fall.Stop();
        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            this.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(endSize, endSize, endSize), i / time);
            yield return null;
        }

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            yield return null;
        }

        SceneManager.LoadScene("game");

    }
}
