using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicOnOff : MonoBehaviour
{
    public float onTime = 1f;
    public float offTime = 1f;

    GameObject child;

    private void Start()
    {
        child = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if(Time.time % (onTime + offTime) < onTime)
        {
            child.SetActive(true);
        }
        else
        {
            child.SetActive(false);
        }
    }
}
