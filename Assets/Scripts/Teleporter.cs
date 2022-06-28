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
            playerController.transform.position = TargetPos.transform.position;
        }
    }
}
