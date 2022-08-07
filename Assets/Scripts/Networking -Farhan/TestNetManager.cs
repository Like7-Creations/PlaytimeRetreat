using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using TMPro;
using GamePackets;
using PlayTimePackets;
using System;

public class TestNetManager : MonoBehaviour
{
    public string playerName;

    public NetworkComponent[] netObjs;
    //public PlayerNetComp[] playerComps = new PlayerNetComp[2];
    public PlayerNetComp localPlayer;
    public PlayerNetComp partnerPlayer;

    public Socket socket;
    public Player player;

    bool isConnected;


    // Start is called before the first frame update
    void Start()
    {
        player = new Player(playerName);

        netObjs = FindObjectsOfType<NetworkComponent>();

        for (int i = 0; i < netObjs.Length; i++)
        {
            if (netObjs[i].GetComponent<PlayerNetComp>())
            {
                PlayerNetComp pcComp = (PlayerNetComp)netObjs[i];
                if (pcComp.playerType == PlayerNetComp.PlayerType.Local)
                {
                    localPlayer = pcComp;

                }
                else if (pcComp.playerType == PlayerNetComp.PlayerType.Partner)
                {
                    partnerPlayer = pcComp;
                }

                /*
                 * 
                 */
            }
        }

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        socket.Blocking = false;

        isConnected = true;

        localPlayer.localID = player.ID;
        socket.Send(new PlayerInfoPacket(player.ID, player.Name).Serialize());
    }

    void Update()
    {
        if (isConnected)
        {
            if (socket.Available > 0)
            {
                DeserializePackets();
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
            /*case GameBasePacket.PacketType.PlayerInfo:
                PlayerInfoPacket piPack = (PlayerInfoPacket)new PlayerInfoPacket().DeSerialize(receivedBuffer);

                break;*/

            case GameBasePacket.PacketType.PlayerController:
                PlayerControllerPacket pcPack = (PlayerControllerPacket)new PlayerControllerPacket().DeSerialize(receivedBuffer);
                print($"Received position: {pcPack.position}, from {pcPack.objID}");

                Console.WriteLine($"{pcPack} packet of {pcPack.objID} conaining {pcPack.position} has been received.");

                if (partnerPlayer.gameObjID.Equals(pcPack.objID))
                {
                    partnerPlayer.UpdateComponent(receivedBuffer);
                }

                break;

            //Check for Spawnpoint Packet
            case GameBasePacket.PacketType.PlayerSpawn:
                Guid temp = new Guid(pb.objID);
                //If it's id is equal to playyer.id
                if (temp == player.ID)
                {
                    //Send packet to local controller
                    localPlayer.SetSpawn(receivedBuffer);
                }
                else
                {
                    //Else, give it to partner.
                    partnerPlayer.SetSpawn(receivedBuffer);
                }

                break;

            default:
                break;
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
