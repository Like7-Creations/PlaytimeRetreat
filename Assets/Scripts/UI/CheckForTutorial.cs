using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForTutorial : MonoBehaviour
{
    public string prefPrefix = "Tut_";
    bool firstTimeTut;

    public GameObject prompt;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(prefPrefix + "FirstTime"))
        {
            int first = PlayerPrefs.GetInt(prefPrefix + "FirstTime");
            if (first == 1)
                firstTimeTut = true;
            else
                firstTimeTut = false;
        }
        else
            firstTimeTut = false;
    }

    public void DisplayTut()
    {
        if (!firstTimeTut)
        {
            prompt.SetActive(true);
            firstTimeTut = true;
            PlayerPrefs.SetInt(prefPrefix + "FirstTime", 1);
        }
    }
}
