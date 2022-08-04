using GamePackets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNetComp : NetworkComponent
{
    Vector3 currentObjPos;

    void Start()
    {
        currentObjPos = transform.position;
    }

    void FixedUpdate()
    {
        if (transform.position != currentObjPos)
        {
            SendUpdateRequest();
            currentObjPos = transform.position;
        }
    }

    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.None:
                TestPacket pcPack = (TestPacket)new TestPacket().DeSerialize(receivedBuffer);
                print($"Packet contains position:{pcPack.objPos}");

                transform.position = pcPack.objPos;
                currentObjPos = transform.position;

                break;

            default:
                break;
        }
    }

    public override void SendUpdateRequest()
    {
        byte[] buffer;

        GameBasePacket testPacket = new TestPacket(transform.position, gameObjID);
        buffer = testPacket.Serialize();
        testNetManager.SendPacket(buffer);
    }
}