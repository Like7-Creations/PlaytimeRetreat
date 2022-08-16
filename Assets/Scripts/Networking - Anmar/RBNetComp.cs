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

    bool recieving;

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
        timer += Time.deltaTime;
        if(timer > 10)
        {
            SendUpdateRequest();
            timer = 0;
        }/*
        /*if (Velocity != rb.velocity && !recieving)
        {
            SendUpdateRequest();
            Velocity = rb.velocity;
        }
        else if (mass != rb.mass && !recieving)
        {
            SendUpdateRequest();
            mass = rb.mass;
        }
        else if(gravityActive != rb.useGravity && !recieving)
        {
            SendUpdateRequest();
            gravityActive = rb.useGravity;
        }*/
        if (kinematic != rb.isKinematic && !recieving)
        {
            SendUpdateRequest();
            kinematic = rb.isKinematic;
        }
        Debug.Log("The Kinematic Bool is " + rb.isKinematic);
    }
    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.Rigidbody:
                RigidbodyPacket rbp = (RigidbodyPacket)new RigidbodyPacket().DeSerialize(receivedBuffer);
                //print($"Packet contains position:{pup.Position}");
                recieving = true;
                rb.isKinematic = rbp.isKinematic;
                rb.velocity = rbp.velocity;
                rb.mass = rbp.mass;
                rb.useGravity = rbp.gravityActive;

                Velocity = rbp.velocity;
                mass = rbp.mass;
                gravityActive = rbp.gravityActive;
                kinematic = rbp.isKinematic;
                recieving = false;
                print(rbp.isKinematic);
                
                break;

            default:
                break;
        }
    }

    public override void SendUpdateRequest()
    {
        byte[] buffer;

        GameBasePacket pickUpPacket = new RigidbodyPacket(rb, gameObjID);
        buffer = pickUpPacket.Serialize();
        testNetManager.SendPacket(buffer);
        // currentBool = pickupthrow.holding;
        

    }
}
