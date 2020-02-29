using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour
{
    public float speed = 2f;

    Transform player;
    public bool randomOffset = true;
    public Vector3 offset;
    public float randomOffsetMaxDist = 0.25f;
    public float noticeDist = 15f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;

        if (randomOffset)
        {
            offset = new Vector3(PitManager.rand(-randomOffsetMaxDist, randomOffsetMaxDist), 0, PitManager.rand(-randomOffsetMaxDist, randomOffsetMaxDist));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player.position.y - this.transform.position.y < noticeDist)
        {
            Vector3 target = player.position + offset;

            Quaternion targetRot = Quaternion.LookRotation(target);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRot, speed * Time.deltaTime);
            this.transform.position += (target - this.transform.position).normalized * Time.deltaTime * speed;
            
        }
        
    }
}
