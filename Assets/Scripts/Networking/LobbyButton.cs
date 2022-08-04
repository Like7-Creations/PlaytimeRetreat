using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyButton : MonoBehaviour
{
    string ButtonName;
    public NetworkManager networkmanager;
    void Start()
    {
        
    }

    void Update()
    {
        networkmanager = FindObjectOfType<NetworkManager>();
        ButtonName = transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text;
        Debug.Log(ButtonName);
    }
    public void ChangeNMstring()
    {
        networkmanager.ChosenLobbyName = ButtonName;
    }
}
