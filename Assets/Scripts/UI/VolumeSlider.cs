using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{

    public VolumeData data;
    private Slider slider;
    public string volumeName;
    public TMP_Text ui;

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
        ResetSliderValue();
    }

    public void ValueChange(float volume)
    {
        data.SetVolumeLevels(volumeName, volume);
        ui.text = (volume * 100).ToString("F0");
        data.SaveVolumeLevels();
    }

    public void ResetSliderValue()
    {
        if (data != null)
        {
            float volume = data.GetVolumeLevels(volumeName);
            ValueChange(volume);
            slider.value = volume;
            ui.text = (volume * 100).ToString("F0");
        }
    }
}