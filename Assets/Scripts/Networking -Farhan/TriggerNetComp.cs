using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public class TriggerNetComp : NetworkComponent
{
    public TriggerSystem trigger;
    bool currentActive;
    float timer;

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
        timer += Time.deltaTime;
        if(timer >= 0.05)
        {
            SendUpdateRequest();
            timer = 0;
        }
       // Debug.Log(trigger.triggerActive);
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
                print(trigger.triggerActive);

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