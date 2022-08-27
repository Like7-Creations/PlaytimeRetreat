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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController.characterController.enabled = false;
            playerController.transform.position = TargetPos.transform.position;
            playerController.characterController.enabled = true;
        }
        else if (collision.gameObject.tag == "EffectableObject")
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            collision.transform.position = TargetPos.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
