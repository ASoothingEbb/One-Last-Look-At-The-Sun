using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform;

    public float distance = 20;

    public void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, playerTransform.position.y - distance, 0);
    }
}
