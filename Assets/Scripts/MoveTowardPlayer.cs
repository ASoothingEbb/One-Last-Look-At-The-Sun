using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour
{
    public float activateDistance = 30f;
    public float speed = 2f;
    public float horiSpeedMultiplier = 5f;

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
            //transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            Vector3 dist = player.position - transform.position;
            transform.position = new Vector3(transform.position.x + dist.normalized.x * horiSpeedMultiplier * speed * Time.deltaTime, transform.position.y + dist.normalized.y * speed * Time.deltaTime, transform.position.z + dist.normalized.z * horiSpeedMultiplier * speed * Time.deltaTime);
            transform.LookAt(player);
        }
    }
}
