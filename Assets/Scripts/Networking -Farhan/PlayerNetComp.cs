using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;
using System;

public class PlayerNetComp : NetworkComponent
{
    public Guid localID;

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
        Unkown = -1,
        Local,
        Partner
    }

    public PlayerType playerType;

    void Start()
    {


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

    void Update()
    {

    }

    public override void UpdateComponent(byte[] receivedBuffer)
    {
        /*byte[] receivedBuffer = new byte[1024];

        switch (packet.Type)
        {
            case GameBasePacket.PacketType.PlayerController:
                PlayerControllerPacket pcPack = (PlayerControllerPacket)new PlayerControllerPacket().DeSerialize(receivedBuffer);
                playerType = (PlayerType)pcPack.playerIntType;

                break;

            default:
                break;
        }*/

    }

    public override void SendUpdateRequest()
    {
        byte[] buffer;

        GameBasePacket pcPacket = new PlayerControllerPacket((int)playerType, gameObjID, pController.isGrounded, pController.velocity);
        buffer = pcPacket.Serialize();
        gnManager.SendPacket(buffer);


    }
}
