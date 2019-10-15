using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudFade : MonoBehaviour
{
    public Image img;
    public float depth = 20;
    Transform player;
    bool doneIt = false;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
    }

    public void Update()
    {
        if (player.position.y < depth && !doneIt)
        {
            doneIt = true;
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        for (float i = 0; i <= 2; i += Time.deltaTime)
        {
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
           img.color = new Color(1, 1, 1, i);
           yield return null;
        }

        Destroy(gameObject);
    }
}
