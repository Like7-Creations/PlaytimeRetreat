using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{

    public VolumeData data;
    public List<VolumeSlider> sliders = new List<VolumeSlider>();

    private void Awake()
    {
        data.GetVolumeLevels();
    }

    private void Start()
    {
        data.GetVolumeLevels();
    }

    public void ApplyChanges()
    {
        data.SaveVolumeLevels();
        PlayerPrefs.Save();
    }
}
