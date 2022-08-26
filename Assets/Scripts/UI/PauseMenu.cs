using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    enum PauseState
    {
        Running,
        Paused,
    }
    PauseState state;
    public GameObject pauseUI;
    public GameObject settingsUI;

    void Start()
    {
        state = PauseState.Running;
        pauseUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == PauseState.Running)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        state=PauseState.Paused;
        pauseUI.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        state = PauseState.Running;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }
}
