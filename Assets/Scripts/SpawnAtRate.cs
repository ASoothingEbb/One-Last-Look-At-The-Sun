using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtRate : MonoBehaviour
{
    public GameObject spawnable;
    public float rate = 5;
    float timeSinceLastSpawn = 0;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > 1 / rate)
        {
            Instantiate(spawnable, transform);
            timeSinceLastSpawn = 0;
        }
    }
}
