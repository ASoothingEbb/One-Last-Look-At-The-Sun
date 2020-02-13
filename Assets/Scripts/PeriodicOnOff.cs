using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicOnOff : Offsetable
{
    public float onTime = 1f;
    public float offTime = 1f;
    public bool randomOffset = false;

    GameObject child;

    private void Start()
    {
        if (randomOffset)
        {
            offset = PitManager.rand(0, onTime + offset);
        }
        child = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if((Time.time + offset) % (onTime + offTime) < onTime)
        {
            child.SetActive(true);
        }
        else
        {
            child.SetActive(false);
        }
    }
}
