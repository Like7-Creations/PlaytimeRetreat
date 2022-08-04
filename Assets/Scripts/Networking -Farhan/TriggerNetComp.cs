using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public class TriggerNetComp : NetworkComponent
{
    public TriggerSystem trigger;

    int objID;

    void Start()
    {
        trigger = GetComponent<TriggerSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //gameid == this.name
    }

    public override void UpdateComponent(GameBasePacket tPacket)
    {
        byte[] receivedBuffer = new byte[1024];

        switch (tPacket.Type)
        {
            case GameBasePacket.PacketType.Trigger:
                TriggerPacket tPack = (TriggerPacket)new TriggerPacket().DeSerialize(receivedBuffer);
                trigger.triggerActive = tPack.triggerActive;

                    break;

            default:
                break;
        }
    }

    public override GameBasePacket SendUpdateRequest(GameBasePacket packet)
    {
        //new TriggerPacket(objID.ToString(), trigger.IsTriggered());

        return packet;
    }
}