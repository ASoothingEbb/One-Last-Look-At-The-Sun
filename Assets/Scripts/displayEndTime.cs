using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayEndTime : MonoBehaviour
{
    public Text text;
    void Start()
    {
        text.text = "One Last Look At The Sun\nCode, Art & Sound: Jak Kostrzanowski\nSupervisor: Ronald Grau\n\nTime: " + Mathf.Round ((int) PitManager.timeSinceStart/60) + ":" + ((int) PitManager.timeSinceStart) % 60;
    }
}
