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
        if (Mathf.Pow(this.transform.position.x - player.transform.position.x, 2) + Mathf.Pow(this.transform.position.z - player.transform.position.z, 2) > 0.2f)
        {
            Vector3 target = player.transform.position;

            Quaternion targetRot = Quaternion.LookRotation(target - this.transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRot, speed * Time.deltaTime);
            if (player.transform.position.y - this.transform.position.y < noticeDist)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, target, Time.deltaTime * speed * 1 / (Mathf.Pow(Mathf.Abs(player.transform.position.y - this.transform.position.y), 0.7f)));
            }
        }
    }
}
