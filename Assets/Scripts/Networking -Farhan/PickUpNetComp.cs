using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using GamePackets;

public class PickUpNetComp : NetworkComponent
{
    public bool Holding;
    public Vector3 Position;
    public Vector3 Rotation;
    
    Vector3 currentPos;
    Vector3 currentRot;
    bool currentBool;
    
    
    PickUpThrow pickupthrow;
    Rigidbody rb;
    Vector3 currentScale;
   
    
    BoxCollider bCollider;


    void Start()
    {
        pickupthrow = GetComponent<PickUpThrow>();
        rb = GetComponent<Rigidbody>();
        bCollider = GetComponent<BoxCollider>();
        currentPos = transform.position;
        currentRot = transform.eulerAngles;
        currentBool = pickupthrow.holding;
        currentScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
            SendUpdateRequest();
        /*if (transform.position != currentPos || pickupthrow != pickupthrow.holding || transform.localScale != currentScale)
        {
            currentPos = transform.position;
            currentBool = pickupthrow.holding;
            currentScale = transform.localScale;
        }*/
    }
    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.PickUp:
                PickUpPacket puPack = (PickUpPacket)new PickUpPacket().DeSerialize(receivedBuffer);
                //print($"Packet contains position:{pup.Position}");

                pickupthrow.holding = puPack.Holding;
                print(puPack.Holding);
                transform.position = puPack.Position;
                transform.rotation = Quaternion.Euler(puPack.Rotation);
                print("updating position and rotation");
                currentPos = transform.position;
                break;

            case GameBasePacket.PacketType.SizeMass:
                SizeMassPacket smPack = (SizeMassPacket)new SizeMassPacket().DeSerialize(receivedBuffer);
                print("updating object scale and mass");
                transform.localScale = smPack.Scale;
                rb.mass = smPack.Mass;
                currentScale = transform.localScale;
                break;

            default:
                break;
        }
    }
    public override void SendUpdateRequest()
    {
        byte[] buffer;
        if (transform.position != currentPos/* || pickupthrow != pickupthrow.holding*/)
        {
            GameBasePacket pickUpPacket = new PickUpPacket(pickupthrow.holding, transform.position, transform.eulerAngles, gameObjID);
            buffer = pickUpPacket.Serialize();
            testNetManager.SendPacket(buffer);
            currentPos = transform.position;
           // currentBool = pickupthrow.holding;
        }

        if (transform.localScale != currentScale)
        {
            GameBasePacket sizemass = new SizeMassPacket(transform.localScale, rb.mass, gameObjID);
            buffer = sizemass.Serialize();
            testNetManager.SendPacket(buffer);
            currentScale = transform.localScale;
        }

    }
}

