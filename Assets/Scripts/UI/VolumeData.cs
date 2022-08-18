using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class VolumeHolder
{
    public string name;
    [Range(0.001f, 1)]
    public float defaultVolume = .5f;
    public float volume;
    public float tempVol;
}

[CreateAssetMenu(menuName = "Volume Data")]
public class VolumeData : ScriptableObject
{
    public string prefPrefix = "Volume_";
    public AudioMixer mixer;
    public VolumeHolder[] volumes;

    public float GetVolumeLevels(string name)
    {
        float volume = .5f;

        if (mixer == null)
        {
            return volume;
        }

        foreach (VolumeHolder vol in volumes)
        {
            if (vol.name.Equals(name))
            {
                if (PlayerPrefs.HasKey(prefPrefix + vol.name))
                {
                    vol.volume = PlayerPrefs.GetFloat(prefPrefix + vol.name, vol.tempVol);
                    return vol.volume;
                }
                else
                {
                    vol.volume = vol.defaultVolume;
                    return vol.volume;
                }
            }
            vol.tempVol = vol.volume;
            mixer.SetFloat(vol.name, Mathf.Log(volume) * 20f);
        }
        return volume;
    }

    public void GetVolumeLevels()
    {
        if (mixer == null)
        {
            return;
        }

        foreach (VolumeHolder vol in volumes)
        {
            if (PlayerPrefs.HasKey(prefPrefix + vol.name))
            {
                vol.volume = PlayerPrefs.GetFloat(prefPrefix + vol.name, vol.tempVol);
            }
            else
            {
                vol.volume = vol.defaultVolume;
            }
            //Debug.Log("Receiving Values - " + prefPrefix + vol.name + ": " + vol.volume);
            vol.tempVol = vol.volume;
            mixer.SetFloat(vol.name, Mathf.Log(vol.volume) * 20f);
        }
    }

    public void SetVolumeLevels(string name, float volume)
    {
        if (mixer == null)
        {
            return;
        }

        foreach (VolumeHolder vol in volumes)
        {
            if (vol.name.Equals(name))
            {
                mixer.SetFloat(vol.name, Mathf.Log(vol.volume) * 20f);
                vol.tempVol = volume;
                break;
            }
        }
    }

    public void SaveVolumeLevels()
    {
        if (mixer == null)
        {
            return;
        }

        float volume = 0;
        foreach (VolumeHolder vol in volumes)
        {
            volume = vol.tempVol;
            Debug.Log("Saving Values - " + prefPrefix + vol.name + ": " + volume);
            PlayerPrefs.SetFloat(prefPrefix + vol.name, volume);
            mixer.SetFloat(vol.name, Mathf.Log(vol.volume) * 20f);
            vol.volume = volume;
        }
    }
}