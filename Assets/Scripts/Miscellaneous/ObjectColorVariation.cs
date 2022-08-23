using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColorVariation : MonoBehaviour
{
    public Color[] Variations;

    // Start is called before the first frame update
    void Awake()
    {
        Material mat = GetComponent<Renderer>().material;
        mat.color = Variations[Random.Range(0, Variations.Length)];
        GetComponent<Renderer>().material = mat;
    }
}
