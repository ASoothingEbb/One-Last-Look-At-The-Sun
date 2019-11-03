﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitManager : MonoBehaviour
{
    public GameObject[] pitSections;
    public List<GameObject> currentSections;
    public float sectionLength = 249.99f;
    Transform playerPos;
    float lastPosSpawned = 0;
    public int maxSections = 3;
    public static System.Random random;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        random = new System.Random();
        lastPosSpawned += sectionLength/2;
        for (int i = 0; i < maxSections; i++) spawnNextSection();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos.position.y < lastPosSpawned + (maxSections-1.5)*sectionLength)
        {
            spawnNextSection();
        }
    }

    void spawnNextSection()
    {
        GameObject newSection = Instantiate(pitSections[random.Next(0, pitSections.Length)], this.transform);
        lastPosSpawned -= sectionLength;
        newSection.transform.position = Vector3.up * lastPosSpawned;
        currentSections.Add(newSection);
        if (currentSections.Count > maxSections)
        {
            Destroy(currentSections[0]);
            currentSections.RemoveAt(0);
        }
    }

    public static float rand(float start, float stop)
    {
        return (float)(random.NextDouble() * (stop - start) - start);
    }
}
