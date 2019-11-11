using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnables;
    public bool evenChance = false;
    void Start()
    {
        float chance = 1 / spawnables.Length;
        int i = 0;
        for (; i < 100; i += 1)
        {

            if (!evenChance)
            {
                chance = spawnables[i % spawnables.Length].chance;
            }

            if (PitManager.rand(0,1) < chance)
            {
                break;
            }

        }
        Instantiate(spawnables[i % spawnables.Length].prefab, gameObject.transform);
    }
}

[System.Serializable]
public struct SpawnData
{
    public GameObject prefab;
    public float chance;
}
