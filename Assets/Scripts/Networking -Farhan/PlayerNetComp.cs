using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;
using System;

public class PlayerNetComp : NetworkComponent
{
    public Guid localID;

    Vector3 currentPos;
    Quaternion currentRot;

    public Transform playerTransform;
    public CapsuleCollider capCollider;

    public PlayerController pController;
    //public MobileController mController;
    //public CameraController cam;


    //public AbilityHolder ability;
    public AbilityTargeting targeting;

    /*public ObjFreezeAbility freezeAbility;
    public ObjMassAbility massAbility;
    public ObjScaleAbility scaleAbility;
    public ObjSurfaceAbility surfaceAbility;

    public enum PlayerType
    {
        Local,
        Partner
    }

    public PlayerType playerType;*/

    void Start()
    {
        currentPos = transform.position;
        currentRot = transform.rotation;

        //if (playerType == PlayerType.Local)
        //{
        //    cam = GetComponentInChildren<CameraController>();
        //    cam.gameObject.SetActive(true);
        //}

        playerTransform = GetComponent<Transform>();
        //capCollider = GetComponent<CapsuleCollider>();

        pController = GetComponent<PlayerController>();

        targeting = GetComponent<AbilityTargeting>();
    }

    void FixedUpdate()
    {
        if (localID == testNetManager.PlayerId)
        {
            SendUpdateRequest();
            currentPos = transform.position;      //This version works. The one with the if-statement doesnt.

            /*if (transform.position != currentPos)
            {
                SendUpdateRequest();
                currentPos = transform.position;
                //currentRot = transform.rotation;
            }*/
        }
        //if local
        //check for rotation and position change
        //if true, sendupdatepacket.
    }

    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.PlayerController:
                PlayerControllerPacket pcPack = (PlayerControllerPacket)new PlayerControllerPacket().DeSerialize(receivedBuffer);
                if (localID == Guid.Parse(pcPack.objID))
                {
                    transform.position = pcPack.position;
                    print($"{pcPack.position} from packet of type {pcPack.Type}, is being passed onto player of type {this.name}");

                    /*pController.movement = pcPack.movement;
                    pController.velocity = pcPack.velocity;*/

                    currentPos = transform.position;
                }

                break;

            default:
                break;
        }
    }

    public void SetSpawn(byte[] buffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(buffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.PlayerSpawn:
                SpawnPosPacket spawnPacket = (SpawnPosPacket)new SpawnPosPacket().DeSerialize(buffer);
                print($"Received packet from {spawnPacket.objID} containg {spawnPacket.spawnPos}");

                transform.position = spawnPacket.spawnPos;
                print($"Object Spawn at{transform.position} obtained from {spawnPacket.spawnPos}");

                //Deserialize SpawnPos Packet, and then update gameobj Transform.pos.
                break;

            default:
                break;
        }
    }

    public override void SendUpdateRequest()
    {
        byte[] buffer;

        GameBasePacket pcPacket = new PlayerControllerPacket(localID.ToString(), transform.position);
        print($"Sending {pcPacket.Type} containing {localID.ToString()}, {transform.position}");
        buffer = pcPacket.Serialize();
        testNetManager.SendPacket(buffer);
    }
}