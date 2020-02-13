using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStaggeredOffsets : MonoBehaviour
{
    public Offsetable[] items;
    public float minOffset = 0;
    public float maxOffset = 1;
    public float spacing = 0.25f;
    void Start()
    {
        float start = PitManager.rand(minOffset, maxOffset);
        for(int i = 0; i < items.Length; i++)
        {
            items[i].offset = start + spacing * i;
        }
    }
}
