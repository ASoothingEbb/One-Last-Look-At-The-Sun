using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitManager : MonoBehaviour
{
    public GameObject[] pitSections;
    public GameObject pitBlank;
    public List<GameObject> currentSections;
    public float sectionLength = 249.99f;
    Transform playerPos;
    float lastPosSpawned = 0;
    public int startBuffer = 5;
    public int maxSections = 3;
    public static System.Random random;
    public float startSections;
    public Material tunnelMat;

    public TunnelState[] tunnelStates;
    TunnelState a;
    TunnelState b;
    int currentTunnelState = 0;

    void Start()
    {
        random = new System.Random();
        playerPos = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        lastPosSpawned += sectionLength/2;
        for (int i = 0; i < maxSections; i++) spawnNextSection();
        a = tunnelStates[currentTunnelState];
        b = tunnelStates[currentTunnelState + 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos.position.y < lastPosSpawned + (maxSections-1.5)*sectionLength)
        {
            spawnNextSection();
        }

        if (playerPos.position.y < b.depth)
        {
            currentTunnelState += 1;
            a = tunnelStates[currentTunnelState];
            b = tunnelStates[currentTunnelState + 1];
        }

        tunnelMat.Lerp(a.mat, b.mat, (playerPos.position.y - a.depth) / (b.depth - a.depth));
    }

    void spawnNextSection()
    {
        GameObject newSection;
        if (lastPosSpawned > -sectionLength * startBuffer)
        {
            newSection = Instantiate(pitBlank, this.transform);
        }
        else
        {
            newSection = Instantiate(pitSections[random.Next(0, pitSections.Length)], this.transform);
        }
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

[System.Serializable]
public struct TunnelState
{
    public Material mat;
    public float depth;
}