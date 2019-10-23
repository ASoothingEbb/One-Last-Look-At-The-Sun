using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEmitWithVelocity : MonoBehaviour
{
    Rigidbody body;
    ParticleSystem.EmissionModule emitter;

    public float rateScale = 10;

    public void Start()
    {
        body = GameObject.FindGameObjectWithTag("player").GetComponent<Rigidbody>();
        emitter = GetComponent<ParticleSystem>().emission;
    }
    // Update is called once per frame
    void Update()
    {
        emitter.rateOverTime = rateScale * (-body.velocity.y);
    }
}
