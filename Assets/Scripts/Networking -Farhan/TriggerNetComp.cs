using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public class TriggerNetComp : NetworkComponent
{
    public TriggerSystem trigger;
    bool currentActive;
    bool currentActive2;
    float timer;
    bool receiving = false;

    void Start()
    {
        trigger = GetComponent<TriggerSystem>();
        testNetManager = FindObjectOfType<TestNetManager>();
        gameObjID = this.gameObject.name;
        currentActive = trigger.triggerActive;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (testNetManager.localPlayer != null && testNetManager.partnerPlayer != null) 
        { 
            if(currentActive != trigger.triggerActive & !receiving)
            {
                SendUpdateRequest();
                currentActive = trigger.triggerActive;
            }
            if (currentActive2 != trigger.buttonPressed)
            {
                SendUpdateRequest();
                currentActive2 = trigger.buttonPressed;
            }
        }

    }

    public override void UpdateComponent(byte[] receivedBuffer)
    {
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.Trigger:
                TriggerPacket tp = (TriggerPacket)new TriggerPacket().DeSerialize(receivedBuffer);
                Debug.Log("Received Trigger Request");
                receiving = true;
                trigger.triggerActive = tp.triggerActive;
                trigger.buttonPressed = tp.PressedActive;
                currentActive = trigger.triggerActive;
                currentActive2 = trigger.buttonPressed;
                print(trigger.triggerActive);
                receiving = false;

                break;

            default:
                break;
        }
    }

    public override void SendUpdateRequest()
    {
        byte[] buffer;
        Debug.Log("Sending Trigger Packet");
        GameBasePacket pcPacket = new TriggerPacket(gameObjID, trigger.buttonPressed,trigger.triggerActive);
        buffer = pcPacket.Serialize();
        testNetManager.SendPacket(buffer);
    }
}