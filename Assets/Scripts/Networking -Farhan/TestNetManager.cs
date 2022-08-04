using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using TMPro;
using GamePackets;
using PlayTimePackets;

public class TestNetManager : MonoBehaviour
{
    public NetworkComponent[] netObjs;
    //public PlayerNetComp[] playerComps = new PlayerNetComp[2];
    public PlayerNetComp localPlayer;
    public PlayerNetComp partnerPlayer;

    public Socket socket;
    Player player;

    bool isConnected;


    // Start is called before the first frame update
    void Start()
    {
        player = new Player("Bot");

        netObjs = FindObjectsOfType<NetworkComponent>();

        for (int i = 0; i < netObjs.Length; i++)
        {
            if (netObjs[i].GetComponent<PlayerNetComp>())
            {
                
                
                /*if(playerComps.Length == 0)
                {
                    playerComps[0] = (PlayerNetComp) netObjs[i];
                }
                else
                {
                    playerComps[playerComps.Length - 1] = (PlayerNetComp) netObjs[i];
                }*/
            }
        }


        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        socket.Blocking = false;

        isConnected = true;
    }

    void Update()
    {
        if (isConnected)
        {
            if (socket.Available > 0)
            {
                DeserializePackets();

                //for (int i = 0; i < netObjs.Length; i++)
                //{

                //}
            }
        }
    }

    public void SendPacket(byte[] buffer)
    {
        socket.Send(buffer);
    }

    public void DeserializePackets()
    {
        byte[] receivedBuffer = new byte[socket.Available];

        socket.Receive(receivedBuffer);
        GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);
        print($"Packet received {pb.objID}");

        switch (pb.Type)
        {
            //case 
        }

        for (int i = 0; i < netObjs.Length; i++)
        {
            if (netObjs[i].gameObjID.Equals(pb.objID))
            {
                print($"{netObjs[i].name} Object found. Providing Packet {pb.objID}");
                netObjs[i].UpdateComponent(receivedBuffer);
            }
        }
    }
}
