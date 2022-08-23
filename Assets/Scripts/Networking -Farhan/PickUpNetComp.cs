using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using GamePackets;
using System;

public class PickUpNetComp : NetworkComponent
{
    public bool Holding;
    public Vector3 Position;
    public Vector3 Rotation;

    Vector3 currentPos;
    Vector3 currentRot;
    bool currentBool;
    /*private bool currentBool
    {
        get
        {
            return pickupthrow.holding;
        }

        set
        {
            pickupthrow.holding = value;
            SendUpdateRequest();
        }
    }*/
    float currentMass;
    bool receiving;
    float currentBounciness;

    float timer;

    PickUpThrow pickupthrow;
    Rigidbody rb;
    Vector3 currentScale;
    Collider col;

    bool HoldingCheck;
    bool PosCheck;
    bool SizeCheck;
    bool BounceCheck;

    void Start()
    {
        testNetManager = FindObjectOfType<TestNetManager>();

        if (GetComponent<PickUpThrow>())
            pickupthrow = GetComponent<PickUpThrow>();

        rb = GetComponent<Rigidbody>();
        
        col = GetComponent<Collider>();

        gameObjID = gameObject.name;

        currentPos = transform.position;
        currentRot = transform.eulerAngles;
        currentScale = transform.localScale;

        currentMass = rb.mass;
    }

    void FixedUpdate()
    {
        /* timer += Time.deltaTime;
         if(timer > 0.05f)
         {
             SendUpdateRequest();
             timer = 0;
         }*/
        if (testNetManager.localPlayer != null && testNetManager.partnerPlayer != null)
        {
            //if (pickupthrow.hasplayer && ownerID == testNetManager.clientID)
            //{

                if (transform.position != currentPos && !receiving)
                {
                    PosCheck = true;
                    SendUpdateRequest();
                    currentPos = transform.position;
                }
                else if (currentBool != pickupthrow.holding && !receiving)
                {
                    HoldingCheck = true;
                    SendUpdateRequest();
                    currentBool = pickupthrow.holding;
                    print("bool has changed sending request");
                }
                /* else if (currentRot != transform.rotation.eulerAngles && !receiving)
                 {
                     HoldingCheck = true;
                     SendUpdateRequest();
                     currentRot = transform.rotation.eulerAngles;
                 }*/
                else if (transform.localScale != currentScale && !receiving)
                {
                    SizeCheck = true;
                    SendUpdateRequest();
                    currentScale = transform.localScale;
                }
                else if (currentBounciness != col.material.bounciness && !receiving)
                {
                    BounceCheck = true;
                    SendUpdateRequest();
                    currentBounciness = col.material.bounciness;
                }
            //}
        }
    }
    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {

            case GameBasePacket.PacketType.PositionRotation:
                PositionRotation prp = (PositionRotation)new PositionRotation().DeSerialize(receivedBuffer);
                receiving = true;

                if (gameObjID == prp.objID)
                {
                    transform.position = prp.Position;
                    //transform.rotation = Quaternion.Euler(prp.Rotation.x, prp.Rotation.y, prp.Rotation.z);

                    currentPos = transform.position;
                    //currentRot = transform.rotation.eulerAngles;
                }
                receiving = false;
                break;

            case GameBasePacket.PacketType.PickUp:
                PickUpPacket puPack = (PickUpPacket)new PickUpPacket().DeSerialize(receivedBuffer);
                receiving = true;

                if (gameObjID == puPack.objID)
                {
                    if (ownerID != testNetManager.clientID)
                    {
                        Guid testID = Guid.Parse(puPack.objectOwnerID);
                        if (ownerID == testID)
                        {
                            pickupthrow.holding = puPack.Holding;
                        }
                    }
                }

                // print(puPack.Holding);
                //print("Receiving pickuppacket");
                //currentPos = transform.position;
                //currentBool = pickupthrow.holding;
                receiving = false;
                break;

            case GameBasePacket.PacketType.SizeMass:
                SizeMassPacket smPack = (SizeMassPacket)new SizeMassPacket().DeSerialize(receivedBuffer);
                //print("receiving scale and mass packet");
                receiving = true;

                if (gameObjID == smPack.objID)
                {
                    GetComponent<Transform>().localScale = smPack.Scale;
                    rb.mass = smPack.Mass;
                    currentScale = smPack.Scale;
                    currentMass = rb.mass;
                    // print(rb.mass);
                }
                receiving = false;
                break;

            case GameBasePacket.PacketType.Bounciness:
                BouncinessPacket bp = (BouncinessPacket)new BouncinessPacket().DeSerialize(receivedBuffer);
                receiving = true;

                if (gameObjID == bp.objID)
                {
                    col.material.bounciness = bp.Bounciness;
                    currentBounciness = col.material.bounciness;
                }
                // print("bounciness is now: " + collider.material.bounciness);
                receiving = false;
                break;

            default:
                break;
        }
    }
    public override void SendUpdateRequest()
    {
        byte[] buffer;
        if (PosCheck & !HoldingCheck)
        {
            GameBasePacket PosRot = new PositionRotation(transform.position, transform.localScale, gameObjID);
            buffer = PosRot.Serialize();
            testNetManager.SendPacket(buffer);
            print("Sending PosRot"); PosCheck = false;
        }
        StartCoroutine(WaitFor(0.05f));
        if (HoldingCheck)
        {
            GameBasePacket pickUpPacket = new PickUpPacket(pickupthrow.holding, ownerID.ToString(), gameObjID);
            buffer = pickUpPacket.Serialize();
            testNetManager.SendPacket(buffer);
            print("Sending PickUP"); HoldingCheck = false;
        }

        StartCoroutine(WaitFor(0.05f));

        if (SizeCheck)
        {
            GameBasePacket sizemass = new SizeMassPacket(transform.localScale, rb.mass, gameObjID);
            buffer = sizemass.Serialize();
            testNetManager.SendPacket(buffer);
            print("Sending Size&MAss"); SizeCheck = false;
        }

        StartCoroutine(WaitFor(0.05f));

        if (BounceCheck)
        {
            GameBasePacket bouncinessP = new BouncinessPacket(col.material.bounciness, gameObjID);
            buffer = bouncinessP.Serialize();
            testNetManager.SendPacket(buffer);
            print("sending bounciness"); BounceCheck = false;
        }
    }

    IEnumerator WaitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}

