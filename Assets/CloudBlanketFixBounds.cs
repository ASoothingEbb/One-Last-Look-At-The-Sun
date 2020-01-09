using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBlanketFixBounds : MonoBehaviour
{
    public float x = 200, y = 200, z = 200;
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(x, y, z));
    }
}
