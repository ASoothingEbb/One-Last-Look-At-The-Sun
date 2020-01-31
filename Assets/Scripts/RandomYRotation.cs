using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomYRotation : MonoBehaviour
{
    void Start()
    {
            transform.rotation = Quaternion.Euler(0, PitManager.rand(0, 360), 0);
    }
}
