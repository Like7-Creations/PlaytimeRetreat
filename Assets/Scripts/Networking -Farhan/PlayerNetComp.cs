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
    public MobileController mController;
    public PlayerCollision collision;

    public AbilityHolder ability;
    public AbilityTargeting targeting;

    public ObjFreezeAbility freezeAbility;
    public ObjMassAbility massAbility;
    public ObjScaleAbility scaleAbility;
    public ObjSurfaceAbility surfaceAbility;

    public enum PlayerType
    {
        Local,
        Partner
    }

    public PlayerType playerType;

    void Start()
    {
        currentPos = transform.position;
        currentRot = transform.rotation;

        playerTransform = GetComponent<Transform>();
        capCollider = GetComponent<CapsuleCollider>();

        pController = GetComponent<PlayerController>();
        mController = GetComponent<MobileController>();
        collision = GetComponent<PlayerCollision>();

        ability = GetComponent<AbilityHolder>();
        targeting = GetComponent<AbilityTargeting>();

        freezeAbility = GetComponent<ObjFreezeAbility>();
        massAbility = GetComponent<ObjMassAbility>();
        scaleAbility = GetComponent<ObjScaleAbility>();
        surfaceAbility = GetComponent<ObjSurfaceAbility>();
    }

    void FixedUpdate()
    {
        if (playerType == PlayerType.Local)
        {
            /*if (transform.position != currentPos && transform.rotation != currentRot)
            {*/
            SendUpdateRequest();
            currentPos = transform.position;
            currentRot = transform.rotation;
            //}
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
                if (playerType == PlayerType.Partner)
                {
                    transform.position = pcPack.position;
                    print($"{pcPack.position} from packet of type {pcPack.Type}, is being passed onto player of type {playerType}");
                    //pController.velocity = pcPack.velocity;

                    currentPos = transform.position;
                    currentRot = transform.rotation;
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

        GameBasePacket pcPacket = new PlayerControllerPacket((int)playerType, gameObjID, transform.position);
        print($"Sending {pcPacket.Type} containing {playerType}, {gameObjID}, {transform.position}");
        buffer = pcPacket.Serialize();
        testNetManager.SendPacket(buffer);
    }
}