using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicOnOff : MonoBehaviour
{
    public float onTime = 1f;
    public float offTime = 1f;
    public float offsetTime = 0f;
    public bool randomOffset = false;

    GameObject child;

    private void Start()
    {
        if (randomOffset)
        {
            offsetTime = Random.Range(0, onTime + offsetTime);
        }
        child = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if(Time.time + offsetTime % (onTime + offTime) < onTime)
        {
            child.SetActive(true);
        }
        else
        {
            child.SetActive(false);
        }
    }
}
