using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnables;
    public bool evenChance = false;
    void Start()
    {
        float chance = 1f / spawnables.Length;
        int i = PitManager.random.Next(spawnables.Length);
        for (; i < 1000; i += 1)
        {
            SpawnData a = spawnables[i % spawnables.Length];
            if (!evenChance)
            {

                chance = a.chance;
            }

            if (PitManager.rand(0, 1) < chance * a.depthChance.Evaluate(transform.position.y/PitManager.max_depth))
            {
                break;
            }

        }
        GameObject toSpawn = spawnables[i % spawnables.Length].prefab;
        Instantiate(toSpawn, gameObject.transform).name = toSpawn.name;
    }
}

[System.Serializable]
public struct SpawnData
{
    public GameObject prefab;
    public float chance;
    public AnimationCurve depthChance;
}
