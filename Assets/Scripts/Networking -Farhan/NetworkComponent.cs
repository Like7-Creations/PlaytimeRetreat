using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public abstract class NetworkComponent : MonoBehaviour
{
    public GameNetworkManager gnManager;
    public TestNetManager testNetManager;

    public Guid ownerID;

    public string gameObjID;

    //public enum Ownership
    //{
    //    Host,
    //    Partner,
    //    Unowned
    //}

    //public Ownership ownStatus;

    public void Awake()
    {
        gnManager = FindObjectOfType <GameNetworkManager>();
        testNetManager = FindObjectOfType<TestNetManager>();
        gameObjID = gameObject.name;
    }

    public abstract void UpdateComponent(byte[] buffer);

    public virtual void SendUpdateRequest()
    {
        byte[] buffer = new byte[1024];

        gnManager.SendPacket(buffer);
    }
}
