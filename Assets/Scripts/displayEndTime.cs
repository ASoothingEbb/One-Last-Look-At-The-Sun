using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayEndTime : MonoBehaviour
{
    public Text text;
    void Start()
    {
        text.text = "Congratulations!\n" + Mathf.Round ((int) PitManager.timeSinceStart/60) + ":" + ((int) PitManager.timeSinceStart) % 60;
    }
}
