using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public class TriggerNetComp : NetworkComponent
{
    public TriggerSystem trigger;
    bool currentActive;
    bool updating;

    void Start()
    {
        trigger = GetComponent<TriggerSystem>();
        gameObjID = this.gameObject.name;
        currentActive = trigger.triggerActive;
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger.triggerActive != currentActive)
        {
            SendUpdateRequest();
            currentActive = trigger.triggerActive;
        }
        Debug.Log(trigger.triggerActive);
    }

    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.Trigger:
                TriggerPacket tp = (TriggerPacket)new TriggerPacket().DeSerialize(receivedBuffer);
                Debug.Log("Received Trigger Request");
                trigger.triggerActive = tp.triggerActive;
                currentActive = tp.triggerActive;

                break;

            default:
                break;
        }
    }

    public override void SendUpdateRequest()
    {
        byte[] buffer;
        Debug.Log("Sending Trigger Request");
        GameBasePacket pcPacket = new TriggerPacket(gameObjID, trigger.triggerActive);
        buffer = pcPacket.Serialize();
        testNetManager.SendPacket(buffer);
    }
}