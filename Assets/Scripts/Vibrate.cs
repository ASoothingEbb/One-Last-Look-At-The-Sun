using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    public float period = 1;
    public float time = 0;
    public Vector3 start;
    public Vector3 end;
    public bool randomOffset;
    void Start()
    {
        if (randomOffset)
        {
            time = PitManager.rand(0, period);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(time >= period)
        {
            time = 0;
            Vector3 tmp = start;
            start = end;
            end = tmp;
        }
        transform.localPosition = Vector3.Lerp(start, end, time / period);
        time += Time.deltaTime;
    }
}
