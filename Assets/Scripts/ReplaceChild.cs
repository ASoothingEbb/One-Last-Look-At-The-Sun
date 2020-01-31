using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceChild : MonoBehaviour
{
    public GameObject replaceWith;
    void Start()
    {
        Transform beingReplaced = transform.GetChild(PitManager.random.Next(0, transform.childCount));
        GameObject newObj = Instantiate(replaceWith, gameObject.transform);
        newObj.transform.position = beingReplaced.transform.position;
        newObj.transform.rotation = beingReplaced.transform.rotation;
        newObj.transform.localScale = beingReplaced.transform.localScale;
        Destroy(beingReplaced.gameObject);
    }
}
