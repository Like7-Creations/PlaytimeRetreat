using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePackets;

public class ObjNetComp : NetworkComponent
{
    public Collider objCol;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void UpdateComponent(GameBasePacket packet)
    {

    }

    public override GameBasePacket SendUpdateRequest(GameBasePacket packet)
    {
        throw new System.NotImplementedException();
    }
}
