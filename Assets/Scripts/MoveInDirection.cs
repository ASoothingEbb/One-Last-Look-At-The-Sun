using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    public float speed = 0.2f;
    public Vector3 direction = new Vector3(0, 0, -1);
    public bool randomize = false;
    public float minX = 0;
    public float maxX = 1;
    public float minY = 0;
    public float maxY = 1;
    public float minZ = 0;
    public float maxZ = 1;
    public float minSpeed = 0;
    public float maxSpeed = 1;

    public void Start()
    {
        if (randomize)
        {
            speed = PitManager.rand(minSpeed, maxSpeed);
            direction = new Vector3(PitManager.rand(minX, maxX), PitManager.rand(minY, maxY), PitManager.rand(minZ, maxZ));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction.normalized * Time.deltaTime * speed;
    }
}
