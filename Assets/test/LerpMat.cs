using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LerpMat : MonoBehaviour
{
    public Material a;
    public Material b;
    public Material mat;
    public float lerp;
    // Update is called once per frame
    void Update()
    {
        mat.Lerp(a, b, lerp);
    }
}
