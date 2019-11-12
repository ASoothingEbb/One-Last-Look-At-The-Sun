using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateContinuously : MonoBehaviour
{
    public float speed = 10;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, Time.time * speed, 0);
    }
}
