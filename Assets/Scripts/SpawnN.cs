using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnN : MonoBehaviour
{
    public GameObject prefab;
    public int n;
    void Start()
    {
        for (int i = 0; i < n; i++)
        {
            Instantiate(prefab, this.transform);
        }
    }
}
