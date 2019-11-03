using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    public float speed = 0.2f;
    public Vector3 direction = new Vector3(0, 0, -1);
    public bool randomize = false;

    public void Start()
    {
        if (randomize)
        {
            speed = PitManager.rand(0, 1);
            direction = new Vector3(PitManager.rand(-1, 1), PitManager.rand(-1, 1), PitManager.rand(-1, 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction.normalized * Time.deltaTime * speed;
    }
}
