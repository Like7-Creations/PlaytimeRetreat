using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public class RBNetComp : NetworkComponent
{
    public Vector3 Velocity;

    public float mass;

    public bool gravityActive;
    public bool kinematic;

    bool receiving;

    Rigidbody rb;

    float timer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        testNetManager = FindObjectOfType<TestNetManager>();
        gameObjID = gameObject.name;
        Velocity = rb.velocity;
        mass = rb.mass;
        gravityActive = true;
        rb.useGravity = gravityActive;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*timer += Time.deltaTime;
        if(timer > 0.05f)
        {
            SendUpdateRequest();
            //print(rb.isKinematic);
        }
        if (Velocity != rb.velocity && !receiving)
        {
            SendUpdateRequest();
            Velocity = rb.velocity;
        }*/
        if (testNetManager.localPlayer != null && testNetManager.partnerPlayer != null)
        {
            if (mass != rb.mass && !receiving)
            {
                //SendUpdateRequest();
                mass = rb.mass;
            }
            else if (gravityActive != rb.useGravity && !receiving)
            {
                // SendUpdateRequest();
                gravityActive = rb.useGravity;
            }
            if (kinematic != rb.isKinematic && !receiving)
            {
                //SendUpdateRequest();
                kinematic = rb.isKinematic;
            }
        }
        //Debug.Log("The Kinematic Bool is " + rb.isKinematic);
    }
    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.Rigidbody:
                RigidbodyPacket rbp = (RigidbodyPacket)new RigidbodyPacket().DeSerialize(receivedBuffer);
                //print($"Packet contains position:{pup.Position}");
                receiving = true;
                rb.isKinematic = rbp.isKinematic;
                print("received kinematic = " + rbp.isKinematic);
                rb.mass = rbp.Mass;
                rb.useGravity = rbp.GravityActive;

               // Velocity = rbp.velocity;
                mass = rb.mass;
                gravityActive = rb.useGravity;
                kinematic = rb.isKinematic;
                //print(rbp.isKinematic);
                receiving = false;
                
                break;

            default:
                break;
        }
    }

    public override void SendUpdateRequest()
    {
        byte[] buffer;

        GameBasePacket pickUpPacket = new RigidbodyPacket(rb.mass, rb.useGravity, rb.isKinematic, gameObjID);
        buffer = pickUpPacket.Serialize();
        testNetManager.SendPacket(buffer);
        print("Sending Rigidbody Packet");
        // currentBool = pickupthrow.holding;
    }
}
