using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public abstract class NetworkComponent : MonoBehaviour
{
    public Guid ownerID;

    public int gameObjID;

    public enum Ownership
    {
        Host,
        Partner,
        Unowned
    }

    public Ownership ownStatus;

   
    public abstract void UpdateComponent(GameBasePacket packet);

    public abstract GameBasePacket SendUpdateRequest(GameBasePacket packet);
}
