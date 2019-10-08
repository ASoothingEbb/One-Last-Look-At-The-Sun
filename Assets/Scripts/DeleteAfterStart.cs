using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterStart : MonoBehaviour
{
    Transform player;
    public float depth = -10;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.y < depth)
        {
            Destroy(gameObject);
        }
    }
}
