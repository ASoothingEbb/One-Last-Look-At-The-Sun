using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnCollideWithPlayer : MonoBehaviour
{
    //public particles

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player") || other.CompareTag("tapParry") || other.CompareTag("holdParry"))
        {
            //play particles here
            Destroy(gameObject);
        }
    }
}
