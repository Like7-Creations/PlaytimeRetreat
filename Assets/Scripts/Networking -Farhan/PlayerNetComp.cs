using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;
using System;

public class PlayerNetComp : NetworkComponent
{
    public Guid localID;

    public PlayerController pController;

    public Transform playerTransform;
    public CapsuleCollider capCollider;

    public string playerIdDisplay;
    public string playerDesignation;

    public bool playerDataSet = false;

    //public AbilityHolder ability;
    public AbilityTargeting targeting;

    void Start()
    {
        testNetManager = FindObjectOfType<TestNetManager>();

        pController = GetComponent<PlayerController>();
        playerTransform = GetComponent<Transform>();
        targeting = GetComponent<AbilityTargeting>();

        gameObjID = gameObject.name;
    }

    void FixedUpdate()
    {
        if (localID == testNetManager.clientID)
        {
            SendUpdateRequest();
            //currentPos = transform.position;      //This version works. The one with the if-statement doesnt.

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
                if (gameObjID == pcPack.objID)
                {
                    print($"{gameObject} has received {pcPack.Type} packet from the server.");

                    Guid tempID = Guid.Parse(pcPack.ownershipID);

                    if (localID != testNetManager.clientID)
                    {
                        if (localID == tempID)
                        {
                            //transform.position = pcPack.position;
                            print("Player Movement: " + pController.movement);
                            pController.movement = pcPack.movement;

                            print("Player Velocity: " + pController.velocity);
                            pController.velocity = pcPack.velocity;

                            print($"{pcPack.movement} and {pcPack.velocity} from packet of type {pcPack.Type}, are being passed onto player of type {gameObject}");
                        }
                    }
                }

                break;

            default:
                break;
        }
    }

    /*public void SetSpawn(byte[] buffer)
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
    }*/

    public override void SendUpdateRequest()
    {
        byte[] buffer;

        GameBasePacket pcPack = new PlayerControllerPacket(pController.movement, pController.velocity, localID.ToString(), gameObjID);
        print($"Sending {pcPack.Type} containing {localID.ToString()}");
        buffer = pcPack.Serialize();
        testNetManager.SendPacket(buffer);
    }
}