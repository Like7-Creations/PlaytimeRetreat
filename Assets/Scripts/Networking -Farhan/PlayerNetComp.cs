using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public class PlayerNetComp : NetworkComponent
{
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
        Host,
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

    public override void UpdateComponent(GameBasePacket packet)
    {
        byte[] receivedBuffer = new byte[1024];

        switch (packet.Type)
        {
            case GameBasePacket.PacketType.PlayerController:
                PlayerControllerPacket pcPack = (PlayerControllerPacket)new PlayerControllerPacket().DeSerialize(receivedBuffer);
                playerType = (PlayerType)pcPack.playerIntType;


                break;

            default:
                break;
        }

    }

    public override GameBasePacket SendUpdateRequest(GameBasePacket packet)
    {
        switch (packet.Type)
        {
            case GameBasePacket.PacketType.PlayerController:
                packet = new PlayerControllerPacket((int)playerType, gameObjID, pController.isGrounded, pController.velocity);
                break;

            default:
                break;
        }

        return packet;
    }
}
