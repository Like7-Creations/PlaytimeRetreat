using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField]float timer;
    public GameObject InstantiatorPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 7)
        {
            Instantiate(InstantiatorPrefab, transform.position, Quaternion.identity);
            timer = 0;
        } 
    }
}
