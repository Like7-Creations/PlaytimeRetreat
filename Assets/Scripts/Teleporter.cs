using UnityEngine;

public class Teleporter : MonoBehaviour
{
    PlayerController playerController;

    public GameObject TargetPos;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerController.characterController.enabled = false;
            playerController.transform.position = TargetPos.transform.position;
            playerController.characterController.enabled = true;
            //TargetPos.transform.position = playerController.transform.position;
            Debug.Log("hellotp?");
        }
    }
}
