using System.Collections;
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
    float dir = 1;
    public bool randomDir = true;

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

        if (randomDir)
        {
            if(PitManager.rand(0,1) > 0.5f)
            {
                this.transform.localScale = Vector3.Scale(this.transform.localScale, (new Vector3(-1, 1, 1)));
                dir = -1;
            }
        }
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, dir * (offset + Time.time * speed), 0);
    }
}
