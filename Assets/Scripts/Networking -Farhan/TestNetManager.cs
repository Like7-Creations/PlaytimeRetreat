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
    //public GameObject prefabController;

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
    public Guid PartnerGuidStore;


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

        //print(PlayerId);

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
    }

    void Update()
    {
        if (isConnected)
        {
            if (socket.Available > 0)
            {
                DeserializePackets();
            }

            if (playerDesignation != null)
            {
                if (localPlayer == null)
                {
                    //Instantiate(prefabController, p1Spawn.position, Quaternion.identity);
                    GameObject localController = InstantiateFromResources(localPrefabName, playerDesignation);
                    print($"{localController.name} has been instantiated for {playerDesignation}");

                    localPlayer = localController.GetComponent<PlayerNetComp>();
                    print($"{localController.name} has been set as the localPlayer for {playerDesignation}");

                    localPlayer.localID = PlayerId;
                    print($"{localPlayer.name} for {playerDesignation} has received the ID:{localPlayer.localID}");

                    SendInstantiationUpdate(localPrefabName, localPlayer.localID, playerDesignation);
                    print($"An Instantiation Packet for {playerDesignation} with ID:{localPlayer.localID} has been sent to the server");
                }
            }
        }
    }

    int test = 0;
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
            //------------------------------

            case GameBasePacket.PacketType.PlayerInfo:
                PlayerInfoPacket piPack = (PlayerInfoPacket)new PlayerInfoPacket().DeSerialize(receivedBuffer);
                if(playerDesignation == null)
                {
                    playerDesignation = piPack.playerName;
                    print($"Welcome {playerDesignation}");
                }

                break;

            case GameBasePacket.PacketType.Instantiate:
                InstantiateObjPacket ioPack = (InstantiateObjPacket)new InstantiateObjPacket().DeSerialize(receivedBuffer);
               // print("OVER HERE!@!@!" + ioPack.OwnerID.ToString());
                Guid tempID = (Guid.Parse(ioPack.OwnerID));
                print($"Received {ioPack.Type} with ID:{tempID}");
                if (tempID != PlayerId)
                {
                    //partnerPlayer = null;
                    if (partnerPlayer == null)
                    {
                        print($"ID:{tempID} is not equal to TestManager's ID:{PlayerId}");

                        GameObject partnerController = InstantiateFromResources(localPrefabName, ioPack.objID);
                        print($"{partnerController.name} has been instantiated for {playerDesignation}");

                        partnerPlayer = partnerController.GetComponent<PlayerNetComp>();
                        print($"{partnerController.name} has been set as the partnerPlayer for {playerDesignation}");

                        partnerPlayer.localID = (Guid.Parse(ioPack.OwnerID));
                        PartnerGuidStore = Guid.Parse(ioPack.OwnerID);
                        //partnerController.GetComponent<PlayerController>().pcNetComp.localID = Guid.Parse(ioPack.OwnerID);
                        print($"{partnerPlayer.name} for {playerDesignation} has received the ID:{partnerPlayer.localID}");
                    }
                    else if (test == 0)
                    {
                        GameObject partnerController = InstantiateFromResources(localPrefabName, ioPack.objID);
                        print($"{partnerController.name} has been instantiated for {playerDesignation}");

                        partnerPlayer = partnerController.GetComponent<PlayerNetComp>();
                        print($"{partnerController.name} has been set as the partnerPlayer for {playerDesignation}");

                        partnerPlayer.localID = (Guid.Parse(ioPack.OwnerID));
                        PartnerGuidStore = Guid.Parse(ioPack.OwnerID);
                        //partnerController.GetComponent<PlayerController>().pcNetComp.localID = Guid.Parse(ioPack.OwnerID);
                        print($"{partnerPlayer.name} for {playerDesignation} has received the ID:{partnerPlayer.localID}");
                        test++;
                    }
                }

                break;


            case GameBasePacket.PacketType.PlayerController:
                PlayerControllerPacket pcPack = (PlayerControllerPacket)new PlayerControllerPacket().DeSerialize(receivedBuffer);
                print($"Received position: {pcPack.position}, from {pcPack.objID}");

                //Console.WriteLine($"{pcPack} packet of {pcPack.objID} conaining {pcPack.position} has been received.");

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

    public GameObject InstantiateFromResources(string prefabName, string designation)
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");
        Vector3 spawn;
        if (designation == "Player1")
        {
            spawn = p1Spawn.position;
            Instantiate(prefab, spawn, Quaternion.identity);     //Replace rotation with new values later.
           // prefabName.GetComponent<Renderer>().material.color = Color.red;
            prefab.name = playerDesignation + " Controller";
            print($"{prefab.name} has been instantiated for {playerDesignation} with a color of Red at spawnPos {spawn}");
        }
        else if (designation == "Player2")
        {
            spawn = p2Spawn.position;
            Instantiate(prefab, spawn, Quaternion.identity);     //Replace spawnPos and rotation with new values later.
           // prefabName.GetComponent<Renderer>().material.color = Color.blue;
            prefab.name = playerDesignation + " Controller";

            partnerPlayer = prefab.GetComponent<PlayerNetComp>();
            
            print($"{prefab.name} has been instantiated for {playerDesignation} with a color of Blue at spawnPos {spawn}");
        }

        //prefab.AddComponent<PlayerNetComp>();       //Optional, use only if player doesnt have the netComp.

        return prefab;
    }

    public void SendInstantiationUpdate(string prefabName, Guid ownerID, string objId)
    {
        socket.Send(new InstantiateObjPacket(prefabName, ownerID.ToString(), objId).Serialize());

        //InstantiateFromResources(prefabName);
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
