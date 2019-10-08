using System.Collections;
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
    System.Random random;
    public Material mat;
    float time = 0;

    public GameObject rock;

    void Start()
    {
        playerPos = GetComponent<Transform>();
        random = new System.Random();
        lastPosSpawned += sectionLength/2;
        for (int i = 0; i < maxSections; i++) spawnNextSection();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        mat.SetFloat("Vector1_523F8AAB", time);
        if (playerPos.position.y < lastPosSpawned + (maxSections-1.5)*sectionLength)
        {
            spawnNextSection();
        }
    }

    void spawnNextSection()
    {
        GameObject newSection = Instantiate(pitSections[random.Next(0, pitSections.Length)]);
        lastPosSpawned -= sectionLength;
        newSection.transform.position = Vector3.up * lastPosSpawned;
        currentSections.Add(newSection);
        if (currentSections.Count > maxSections)
        {
            Destroy(currentSections[0]);
            currentSections.RemoveAt(0);
        }

        spawnObstacles();
    }

    void spawnObstacles()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject r = Instantiate(rock);
            r.transform.position = new Vector3(Random.Range(-4, 4), lastPosSpawned - 5 - 4 * i, Random.Range(-4, 4));
            r.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            r.transform.localScale = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
        }
    }
}
