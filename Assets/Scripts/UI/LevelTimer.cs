using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{

    float timeTaken;
    public bool TimerOn = true;

    public GameObject CompletedUI;
    public GameObject MenuUI;
    public TMP_Text timeText;


    // Start is called before the first frame update
    void Start()
    {
        timeTaken = 0;
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            timeTaken += Time.deltaTime;
        }
    }

    public void DisplayTime()
    {
        TimerOn = false;
        Time.timeScale = 0;
        MenuUI.SetActive(false);
        CompletedUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float min = Mathf.FloorToInt(timeTaken / 60);
        float sec = Mathf.FloorToInt(timeTaken % 60);

        timeText.text = string.Format("{0:00} : {1:00}", min, sec);
    }
}
