using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jelly : MonoBehaviour
{
    public float speed = 0.2f;
    public Vector3 direction = new Vector3(0, 0, -1);

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction.normalized * Time.deltaTime * speed;
    }
}
