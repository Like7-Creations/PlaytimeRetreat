using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;
using System;

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
            if (currentActive != trigger.triggerActive & !receiving)
            {
                SendUpdateRequest();
                currentActive = trigger.triggerActive;
            }
            /*if (currentActive2 != trigger.buttonPressed)
            {
                SendUpdateRequest();
                currentActive2 = trigger.buttonPressed;
            }*/
        }

    }

    public override void UpdateComponent(byte[] receivedBuffer)
    {
        /*if (testNetManager.socket.Available > 0)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }*/

        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

        switch (pb.Type)
        {
            case GameBasePacket.PacketType.Trigger:
                TriggerPacket tp = (TriggerPacket)new TriggerPacket().DeSerialize(receivedBuffer);
                Debug.Log("Received Trigger Request");
                receiving = true;
                trigger.triggerActive = tp.triggerActive;
                currentActive = trigger.triggerActive;

                if (trigger.triggerType == TriggerSystem.TriggerType.Button)
                {
                    trigger.buttonPressed = tp.PressedActive;
                    currentActive2 = trigger.buttonPressed;
                }

                else if (trigger.triggerType == TriggerSystem.TriggerType.TimedButton)
                {
                    trigger.timerButtonPressed = tp.PressedActive;
                    currentActive2 = trigger.timerButtonPressed;
                }

                else if (trigger.triggerType == TriggerSystem.TriggerType.Lever)
                {
                    trigger.leverPulled = tp.PressedActive;
                    currentActive2 = trigger.leverPulled;
                }

                else if (trigger.triggerType == TriggerSystem.TriggerType.PressurePlate)
                {
                    trigger.pressureActive = tp.PressedActive;
                    currentActive2 = trigger.pressureActive;
                }

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

        bool setTrigger;
        GameBasePacket pcPacket;

        switch (trigger.triggerType)
        {
            case TriggerSystem.TriggerType.Button:

                setTrigger = trigger.buttonPressed;
                pcPacket = new TriggerPacket(gameObjID, setTrigger, trigger.triggerActive);
                buffer = pcPacket.Serialize();
                testNetManager.SendPacket(buffer);

                break;
            case TriggerSystem.TriggerType.TimedButton:

                setTrigger = trigger.timerButtonPressed;
                pcPacket = new TriggerPacket(gameObjID, setTrigger, trigger.triggerActive);
                buffer = pcPacket.Serialize();
                testNetManager.SendPacket(buffer);

                break;
            case TriggerSystem.TriggerType.Lever:

                setTrigger = trigger.leverPulled;
                pcPacket = new TriggerPacket(gameObjID, setTrigger, trigger.triggerActive);
                buffer = pcPacket.Serialize();
                testNetManager.SendPacket(buffer);

                break;
            case TriggerSystem.TriggerType.PressurePlate:

                setTrigger = trigger.pressureActive;
                pcPacket = new TriggerPacket(gameObjID, setTrigger, trigger.triggerActive);
                buffer = pcPacket.Serialize();
                testNetManager.SendPacket(buffer);

                break;

            default:
                break;
        }
    }
}