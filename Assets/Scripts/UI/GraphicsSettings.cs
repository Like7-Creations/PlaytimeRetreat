using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    public string prefPrefix = "Graphics_";

    public int defaultQuality;
    public int quality;

    public bool fullscreen;

    Resolution[] resolutions;
    public TMP_Dropdown resUI;
    public TMP_Dropdown qualityUI;
    public TMP_Dropdown fpsUI;
    public Toggle fullscreenUI;

    private void Awake()
    {
        resolutions = Screen.resolutions;
        resUI.ClearOptions();
        List<string> res = new List<string>();
        int currentRes = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string r = resolutions[i].width + "x" + resolutions[i].height;
            res.Add(r);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentRes = i;
        }
        resUI.AddOptions(res);
        resUI.value = currentRes;
        resUI.RefreshShownValue();

        if (PlayerPrefs.HasKey(prefPrefix + "Fullsreen"))
        {
            int state = PlayerPrefs.GetInt(prefPrefix + "Fullsreen");
            if (state == 1)
            {
                Screen.fullScreen = true;
                fullscreenUI.isOn = true;
            }
            else
            {
                Screen.fullScreen = false;
                fullscreenUI.isOn = false;
            }
        }
        else
        {
            Screen.fullScreen = true;
            fullscreenUI.isOn = true;
        }

        if (PlayerPrefs.HasKey(prefPrefix + "FrameRate"))
        {
            int fps = PlayerPrefs.GetInt(prefPrefix + "FrameRate");
            fpsUI.value = fps;
            switch (fps)
            {
                case 0:
                    Application.targetFrameRate = 30;
                    break;
                case 1:
                    Application.targetFrameRate = 60;
                    break;
                case 2:
                    Application.targetFrameRate = 120;
                    break;
                case 3:
                    Application.targetFrameRate = -1;
                    break;
                default:
                    Application.targetFrameRate = -1;
                    break;
            }
        }

        qualityUI.value = GetQuality();
    }

    public int GetQuality()
    {
        if (PlayerPrefs.HasKey(prefPrefix + "Quality"))
        {
            quality = PlayerPrefs.GetInt(prefPrefix + "Quality");
            return quality;
        }
        else
        {
            return defaultQuality;
        }
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        quality = index;
        PlayerPrefs.SetInt(prefPrefix + "Quality", quality);
    }

    public void SetFullscreen(bool state)
    {
        Screen.fullScreen = state;

        if (state)
            PlayerPrefs.SetInt(prefPrefix + "Fullsreen", 1);
        else
            PlayerPrefs.SetInt(prefPrefix + "Fullsreen", 0);
    }

    public void SetFPS(int fps)
    {
        switch (fps)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = -1;
                break;
            default:
                Application.targetFrameRate = -1;
                break;
        }
        PlayerPrefs.SetInt(prefPrefix + "FrameRate", fps);
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt(prefPrefix + "Resolution", resIndex);
    }
}
