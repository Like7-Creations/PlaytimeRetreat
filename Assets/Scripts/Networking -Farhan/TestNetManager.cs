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
    public string localPrefabName;      //Until we have multiple characters, we will use the same prefab for both characters.

    public NetworkComponent[] netObjs;
    //public PlayerNetComp[] playerComps = new PlayerNetComp[2];
    public PlayerNetComp localPlayer;
    public PlayerNetComp partnerPlayer;

    public Transform p1Spawn;
    public Transform p2Spawn;

    public string playerDesignation;
    public Guid PlayerId;

    public Socket socket;
    public Player player;

    bool isConnected;


    /*HOW TO SOLVE OWNERSHIP:-
     * Basically, we need the NetManager to first generate an ID for the client it's linked to.
     * In this way, both clients will have a unique ID tied to them.
     * Following this, they instantiate a player character that will then be assigned to local player. 
     * The NetManager will also assign its id to localPlayer.localID, in order to mark it as local.
     * At the same time, they send an Instantiation packet to the other client, containing the prefabName and the localPlayer.localID/NetManager.OwnerID.
     * Upon deserializing that packet, the second client will Instantiate the received prefab to partnerPlayer, and assign the received ID to partnerPlayer.ID.
     *
     * Anytime ownership comes into play, like for example, when we need to decide which client should send the packet and which one should receive it,
     * we just have the owned object check the localPlayer.localID against the ownerID stored in the object, and if it's match, it sends a packet.
     * If it isn't a match, it has to receive a packet.
     */

    void Start()
    {
        //player = new Player(playerName, Guid.NewGuid());

        netObjs = FindObjectsOfType<NetworkComponent>();
        PlayerId = Guid.NewGuid();
        playerDesignation = null;

        /* Generate a client/owner/player ID.
         * Instantiate the local player prefab here
         * Upon instantiation, assign the newly generated ID to the playerPrefab's netComp
         * Then, assign the player to localPlayer and send an InstantiationPacket over the network.
         */

        //------------------------------
        /*for (int i = 0; i < netObjs.Length; i++)
        {
            if (netObjs[i].GetComponent<PlayerNetComp>())
            {
                PlayerNetComp pcComp = (PlayerNetComp)netObjs[i];
                if (pcComp.playerType == PlayerNetComp.PlayerType.Local)
                {
                    localPlayer = pcComp;
                    localPlayer.localID = player.ID;
                }
                else if (pcComp.playerType == PlayerNetComp.PlayerType.Partner)
                {
                    partnerPlayer = pcComp;
                }
            }
        }*/
        //------------------------------

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        socket.Blocking = false;

        isConnected = true;

        /*localPlayer = InstantiateFromResources(localPrefabName).GetComponent<PlayerNetComp>();
        localPlayer.gameObject.GetComponent<Renderer>().material.color = Color.red;
        localPlayer.localID = PlayerId;
        SendInstantiationUpdate(localPrefabName, PlayerId);*/
    }

    void Update()
    {
        if (isConnected)
        {
            if (socket.Available > 0)
            {
                DeserializePackets();
            }

            if(localPlayer == null)
            {
                localPlayer = InstantiateFromResources(localPrefabName).GetComponent<PlayerNetComp>();
                localPlayer.gameObject.GetComponent<Renderer>().material.color = Color.red;
                localPlayer.localID = PlayerId;
                SendInstantiationUpdate(localPrefabName, PlayerId);
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
            //------------------------------
            /* Create a case that checks if the NetManager received an Instantiation packet.
             * Create an object of type InstantiationPacket and deserialize the received buffer into it.
             * Check if the ownerID received by the NetManager is the same as the one it currently has.
             * If false, call the instantiate function and pass in the prefabName received from the packet
             * Then assign the instantiated Player to the partnerPlayer, and set its localID to the one received from the packet.
             */
            //------------------------------Done

            case GameBasePacket.PacketType.Instantiate:
                InstantiateObjPacket ioPack = (InstantiateObjPacket)new InstantiateObjPacket().DeSerialize(receivedBuffer);
                Guid tempID = Guid.Parse(ioPack.OwnerID);
                if (tempID != PlayerId)
                {
                    partnerPlayer = InstantiateFromResources(ioPack.prefabName).GetComponent<PlayerNetComp>();
                    partnerPlayer.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                    partnerPlayer.localID = PlayerId;
                }

                break;

            case GameBasePacket.PacketType.PlayerInfo:
                PlayerInfoPacket piPack = (PlayerInfoPacket)new PlayerInfoPacket().DeSerialize(receivedBuffer);
                if (playerDesignation == null)
                {
                    playerDesignation = piPack.playerName;
                }
                break;

            case GameBasePacket.PacketType.PlayerController:
                PlayerControllerPacket pcPack = (PlayerControllerPacket)new PlayerControllerPacket().DeSerialize(receivedBuffer);
                print($"Received position: {pcPack.position}, from {pcPack.objID}");

                Console.WriteLine($"{pcPack} packet of {pcPack.objID} conaining {pcPack.position} has been received.");

                if (partnerPlayer.localID == Guid.Parse(pcPack.objID))
                {
                    partnerPlayer.UpdateComponent(receivedBuffer);
                }

                break;

            /*Check for Spawnpoint Packet
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

                break;*/

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

    public GameObject InstantiateFromResources(string prefabName)
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");
        Vector3 spawn;
        if (playerDesignation == "Player1")
        {
            spawn = p1Spawn.position;
            Instantiate(prefab, spawn, Quaternion.identity);     //Replace spawnPos and rotation with new values later.
        }
        else if (playerDesignation == "Player2")
        {
            spawn = p2Spawn.position;
            Instantiate(prefab, spawn, Quaternion.identity);     //Replace spawnPos and rotation with new values later.
        }

        //prefab.AddComponent<PlayerNetComp>();       //Optional, use only if player doesnt have the netComp.

        return prefab;
    }

    public void SendInstantiationUpdate(string prefabName, Guid ownerID)
    {
        socket.Send(new InstantiateObjPacket(prefabName, ownerID.ToString()).Serialize());
    }

    /* Create 2 Instantiation functions:
     * 
     * public GameObject InstantiatePlayer ->
     * Create an empty GameObject called tempObj and call "Resources.Load<GameObject>($"Prefabs/{*Enter PrefabName Here*}")"
     * Call the Instantiate Obj function and pass in the tempObj, 
     * along with a spawnPosition that is stored in the NetManager
     * and any other necessary information.
     * Finally, this function will return tempObj, allowing us to assign it to localPlayer.
     * 
     * SendInstantiatePacket ->
     * Using Socket.Send, it would pass in the prefabName, it's SpawnPosition, and localID.
     */
}
