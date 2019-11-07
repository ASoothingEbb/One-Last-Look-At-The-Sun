using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public Image img;
    public float seconds = 3;
    Transform player;
    public float startAlpha;
    public float stopAlpha;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
        StartCoroutine(Fade(seconds, startAlpha, stopAlpha));
    }

    IEnumerator Fade(float seconds, float startAlpha, float stopAlpha)
    {
        for (float i = 0; i <= seconds; i += Time.deltaTime)
        {
            img.color = new Color(1, 1, 1, Mathf.Lerp(startAlpha, stopAlpha, i/seconds));
            yield return null;
        }

        Destroy(gameObject);
    }
}
