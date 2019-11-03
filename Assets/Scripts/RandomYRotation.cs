using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomYRotation : MonoBehaviour
{

    public bool enabled = false;
    void Start()
    {
        if (enabled)
        {
            transform.rotation = Quaternion.Euler(0, PitManager.rand(0, 360), 0);
        }
    }
}
