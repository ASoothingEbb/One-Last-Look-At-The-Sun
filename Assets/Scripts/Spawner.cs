using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnables;
    bool notSelected = true;
    void Start()
    {
        int i = 0;
        for(; notSelected && i < spawnables.Length; i += 1)
        {
            if(Random.Range(0,1) < spawnables[i].chance)
            {
                notSelected = false;
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
