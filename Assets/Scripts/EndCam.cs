using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCam : MonoBehaviour
{
    public float fallSpeed = 100f;

    public float cutTime = 4f;

    float time = 0f;

    public Vector3 pos;
    public Vector3 rot;

    void Update()
    {
        time += Time.deltaTime;
        if(time > cutTime)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, pos, Mathf.Clamp((time - cutTime) / 128, 0, 1));
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(rot), Mathf.Clamp((time - cutTime) / 128, 0, 1));
        }
        else
        {
            this.transform.localPosition += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }
}
