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
    float currentMass;
    bool receiving;
    float currentBounciness;

    float timer;

    PickUpThrow pickupthrow;
    Rigidbody rb;
    Vector3 currentScale;
    Collider collider;


    void Awake()
    {
        pickupthrow = GetComponent<PickUpThrow>();
        rb = GetComponent<Rigidbody>();
        testNetManager = FindObjectOfType<TestNetManager>();
        collider = GetComponent<Collider>();
        gameObjID = gameObject.name;
        currentPos = transform.position;
        currentRot = transform.eulerAngles;
        currentBool = pickupthrow.holding;
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
            if (transform.position != currentPos && !receiving)
            {
                SendUpdateRequest();
                currentPos = transform.position;
            }
            else if (currentBool != pickupthrow.holding && !receiving)
            {
                SendUpdateRequest();
                currentBool = pickupthrow.holding;
                print("bool has changed sending request");
            }
            else if (currentRot != transform.rotation.eulerAngles && !receiving)
            {
                SendUpdateRequest();
                currentRot = transform.rotation.eulerAngles;
            }
            else if (transform.localScale != currentScale && !receiving)
            {
                SendUpdateRequest();
                currentScale = transform.localScale;
            }
            else if (currentMass != rb.mass && !receiving)
            {
                SendUpdateRequest();
                currentMass = rb.mass;
            }
            else if (currentBounciness != collider.material.bounciness && !receiving)
            {
                SendUpdateRequest();
                currentBounciness = collider.material.bounciness;
            }
        }
    }
    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.PickUp:
                PickUpPacket puPack = (PickUpPacket)new PickUpPacket().DeSerialize(receivedBuffer);
                receiving = true;
                pickupthrow.holding = puPack.Holding;
               // print(puPack.Holding);
                //print("Receiving pickuppacket");
                transform.position = puPack.Position;
                transform.rotation = Quaternion.Euler(puPack.Rotation);
                currentPos = transform.position;
                currentBool = pickupthrow.holding;
                currentRot = transform.rotation.eulerAngles;
                receiving = false;
                break;

            case GameBasePacket.PacketType.SizeMass:
                SizeMassPacket smPack = (SizeMassPacket)new SizeMassPacket().DeSerialize(receivedBuffer);
                //print("receiving scale and mass packet");
                receiving = true;
                GetComponent<Transform>().localScale = smPack.Scale;
                rb.mass = smPack.Mass;
                currentScale = smPack.Scale;
                currentMass = rb.mass;
               // print(rb.mass);
                receiving = false;
                break;
            
            case GameBasePacket.PacketType.Bounciness:
                BouncinessPacket bp = (BouncinessPacket)new BouncinessPacket().DeSerialize(receivedBuffer);
                receiving = true;
                collider.material.bounciness = bp.Bounciness;
                currentBounciness = collider.material.bounciness;
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
        GameBasePacket pickUpPacket = new PickUpPacket(pickupthrow.holding, transform.position, transform.eulerAngles, gameObjID);
        buffer = pickUpPacket.Serialize();
        testNetManager.SendPacket(buffer);
        print("Sending PickUP");

        StartCoroutine(WaitFor(0.05f));

        GameBasePacket sizemass = new SizeMassPacket(transform.localScale, rb.mass, gameObjID);
        buffer = sizemass.Serialize();
        testNetManager.SendPacket(buffer);
        print("Sending Size&MAss");

        StartCoroutine(WaitFor(0.05f));

        GameBasePacket bouncinessP = new BouncinessPacket(collider.material.bounciness, gameObjID);
        buffer = bouncinessP.Serialize();
        testNetManager.SendPacket(buffer);
        print("sending bounciness");

    }

    IEnumerator WaitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}

