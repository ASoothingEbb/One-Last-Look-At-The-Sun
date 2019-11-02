using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour
{
    public float activateDistance = 30f;
    public float speed = 2f;

    Transform player;
    bool activated = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated || (transform.position - player.position).sqrMagnitude < activateDistance * activateDistance)
        {
            activated = true;
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.LookAt(player);
        }
    }
}
