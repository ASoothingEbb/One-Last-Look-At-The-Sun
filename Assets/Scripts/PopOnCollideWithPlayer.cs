using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnCollideWithPlayer : MonoBehaviour
{
    //public particles

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            //play particles here
            Destroy(gameObject);
        }
    }
}
