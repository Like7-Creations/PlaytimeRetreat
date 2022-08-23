using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeWeightIndicators : MonoBehaviour
{
    Text[] indicators;

    public Rigidbody cube;

    private void Start()
    {
        indicators = GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < indicators.Length; i++)
            indicators[i].text = "" + cube.mass;
    }
}
