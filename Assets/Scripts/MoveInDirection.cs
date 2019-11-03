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
            speed = Random.Range(0, 1);
            direction = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction.normalized * Time.deltaTime * speed;
    }
}
