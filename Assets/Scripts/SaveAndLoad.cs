using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveAndLoad
{
    static string filename = "mysave";
    public static void BeginSave(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + filename;
        FileStream filestream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(player);
        formatter.Serialize(filestream, data);
        filestream.Close();
    }

    public static PlayerData BeginLoad()
    {
        string path = Application.persistentDataPath + filename;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream filestream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(filestream) as PlayerData;
            filestream.Close();

            return data;
        }
        else
        {
            Debug.LogError("There Is No Save Data in " + path);
            return null;
        }
    }
}
