using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    PlayerController playerController;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            StartCoroutine(Loading());
        }
    }
    IEnumerator Loading()
    {
        SaveAndLoad.BeginLoad();
        yield return new WaitForSeconds(2);
        Load();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerController.lastCheckpoint = transform.position;
            Save();
            Debug.Log("Saving Game");
        }
    }

    public void Save()
    {
        SaveAndLoad.BeginSave(playerController);
    }

    public void Load()
    {
        PlayerData data = SaveAndLoad.BeginLoad();
        playerController.level = data.level;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        playerController.transform.position = position;
    }
}
