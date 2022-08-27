using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgression : MonoBehaviour
{

    public string nextLevel;
    public LevelTimer timer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            timer.DisplayTime();
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
