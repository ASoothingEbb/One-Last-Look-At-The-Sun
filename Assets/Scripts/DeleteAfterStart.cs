using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterStart : MonoBehaviour
{
    Transform player;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.y < 0)
        {
            Destroy(gameObject);
        }
    }
}
