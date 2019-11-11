using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScriptedStart : MonoBehaviour
{
    bool waiting = true;
    bool notDone = true;
    PlayableDirector p;
    void Start()
    {
        p = GetComponent<PlayableDirector>();
        p.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (notDone)
        {
            if (waiting)
            {
                if (Input.anyKey)
                {
                    waiting = false;
                }
            }
            else
            {
                notDone = false;
                p.Play();
            }
        }
        
    }
}
