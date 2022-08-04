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

    public override void UpdateComponent(byte[] receivedBuffer)
    {

    }

    public override void SendUpdateRequest()
    {

    }
}
