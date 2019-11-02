using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoveInCircle : MonoBehaviour
{
    public float circleSize = 10;
    public float speed = 5f;
    public float timeOffset = 0;
    float time;
    // Update is called once per frame
    void Update()
    {
        time = ((Time.time + timeOffset) * speed ) % (2 * Mathf.PI);
        transform.position  = new Vector3(circleSize * Mathf.Sin(time), transform.position.y, circleSize * Mathf.Cos(time));
        transform.rotation = Quaternion.Euler(0, -90 + time * (360/(Mathf.PI*2)), 0);
    }
}
