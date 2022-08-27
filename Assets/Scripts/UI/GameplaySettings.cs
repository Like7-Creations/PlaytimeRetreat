using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplaySettings : MonoBehaviour
{
    public string prefPrefix = "Gameplay_";
    
    public Slider sensitivityUI;
    public Slider fovUI;
    public TMP_Text fovCount;

    public CameraController controller;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey(prefPrefix + "Sensitivity"))
        {
            float sensitivity = PlayerPrefs.GetFloat(prefPrefix + "Sensitivity");
            sensitivityUI.value = sensitivity;
            SetSensitivity(sensitivity);
        }
        else
        {
            sensitivityUI.value = 300;
            SetSensitivity(300);
        }

        if (PlayerPrefs.HasKey(prefPrefix + "FOV"))
        {
            int fov = PlayerPrefs.GetInt(prefPrefix + "FOV");
            fovUI.value = fov;
            fovCount.text = "" + fov;
            SetFOV(fov);
        }
        else
        {
            fovUI.value = 60;
            SetFOV(60);
        }
    }

    public void SetSensitivity(float sen)
    {
        if (controller != null)
            controller.SetSensitivity(sen);
        PlayerPrefs.SetFloat(prefPrefix + "Sensitivity", sen);
    }

    public void SetFOV(float fov)
    {
        if (controller != null)
            controller.SetFOV((int)fov);
        fovCount.text = "" + fov;
        PlayerPrefs.SetInt(prefPrefix + "FOV", (int)fov);
    }
}
