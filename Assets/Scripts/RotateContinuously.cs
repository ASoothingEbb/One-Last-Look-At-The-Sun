﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateContinuously : MonoBehaviour
{
    public float speed = 10;
    public bool random = false;
    public float minRandom = 0f;
    public float maxRandom = 20f;
    public bool randomOffset = false;
    public float offset = 0f;

    private void Start()
    {
        if (random)
        {
            speed = PitManager.rand(minRandom, maxRandom);
        }

        if (randomOffset)
        {
            offset = PitManager.rand(0, 360);
        }
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, offset + Time.time * speed, 0);
    }
}
