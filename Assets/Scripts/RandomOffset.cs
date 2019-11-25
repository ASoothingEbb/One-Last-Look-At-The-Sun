using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOffset : MonoBehaviour
{
    public float maxX = 3f;
    public float minX = 0f;
    public float maxZ = 3f;
    public float minZ = 0f;
    void Start()
    {
        transform.localPosition = transform.localPosition + new Vector3(PitManager.rand(minX, maxX), 0, PitManager.rand(minZ, maxZ));
    }
}
