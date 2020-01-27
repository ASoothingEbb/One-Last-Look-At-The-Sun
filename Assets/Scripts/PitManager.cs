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
    public float lastPosSpawned = 0;
    public int startBuffer = 5;
    public int maxSections = 3;
    public static System.Random random;
    public const int max_depth = 15000;
    public float startSections;
    public Material tunnelMat;
    public static float timeSinceStart = 0;

    public TunnelState[] tunnelStates;
    TunnelState a;
    TunnelState b;
    int currentTunnelState = 0;
    int flip = 1;

    void Start()
    {
        random = new System.Random();
        playerPos = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        lastPosSpawned += sectionLength/2;
        for (int i = 0; i < maxSections; i++) spawnNextSection();
        a = tunnelStates[currentTunnelState];
        b = tunnelStates[currentTunnelState + 1];
        timeSinceStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos.position.y + sectionLength < lastPosSpawned + (maxSections-1.5)*sectionLength)
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
        timeSinceStart += Time.deltaTime;
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
        newSection.transform.GetChild(0).transform.localScale = new Vector3(1, flip, 1);
        currentSections.Add(newSection);
        if (currentSections.Count > maxSections)
        {
            Destroy(currentSections[0]);
            currentSections.RemoveAt(0);
        }
        flip *= -1;
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