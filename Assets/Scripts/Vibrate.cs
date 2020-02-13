using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : Offsetable
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
            offset = PitManager.rand(0, period);
        }

        time = offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(start, end, Mathf.Abs(period - (time % (2*period)))*2 );
        time += Time.deltaTime;
    }
}
