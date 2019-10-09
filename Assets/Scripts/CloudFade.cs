using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudFade : MonoBehaviour
{
    public Image img;
    Transform player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
        if(player.position.y < 3)
        {
            StartCoroutine(Fade());
        }
        if(player.position.y < -10)
        {
         //   Destroy(gameObject);
        }
    }

    IEnumerator Fade()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
           img.color = new Color(0, 0, 0, i);
           yield return null;
        }

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
           img.color = new Color(0, 0, 0, i);
           yield return null;
        }
    }
}
