using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnables;
    void Start()
    {
        int i = 0;
        for(;; i += 1)
        {
            i = i % spawnables.Length;

            if (PitManager.rand(0,1) < spawnables[i].chance)
            {
                break;
            }

        }
        Instantiate(spawnables[i].prefab, gameObject.transform);
    }
}

[System.Serializable]
public struct SpawnData
{
    public GameObject prefab;
    public float chance;
}
