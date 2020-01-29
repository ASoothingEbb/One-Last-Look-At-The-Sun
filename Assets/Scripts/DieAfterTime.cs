using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    //like tears in rain.
    public float timeToDie = 1;
    float timeAlive = 0;
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > timeToDie)
        {
            Destroy(gameObject);
        }
    }
}
