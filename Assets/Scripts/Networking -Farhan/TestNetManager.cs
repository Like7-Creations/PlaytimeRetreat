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
    [Header("Instate Prefabs")]
    [SerializeField] string prefabName;

    [Header("Net-Synced Objects")]
    List<NetworkComponent> playerComps = new List<NetworkComponent>();
    public NetworkComponent[] netObjs;
    public bool playersAdded = false;

    [Header("Player Spawns")]
    public Transform p1Spawn;
    public Transform p2Spawn;

    [Header("Local Client Details")]
    public Guid clientID;
    public string clientIdDisplay;
    public string clientDesignation;

    [Header("Local Player Details")]
    public PlayerNetComp localPlayer;
    public string localName;
    public string localIdDisplay;
    public string localDesignation;

    [Header("Partner Player Details")]
    public PlayerNetComp partnerPlayer;
    public string partnerName;
    public string partnerIdDisplay;
    public string partnerDesignation;


    public Socket socket;
    public Player player;

    bool isConnected;
    bool clientsLinked;
    
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
        clientID = Guid.NewGuid();
        clientIdDisplay = clientID.ToString();
        clientDesignation = null;

        //print(PlayerId);

        /* Generate a client/owner/player ID.
         * Instantiate the local player prefab here
         * Upon instantiation, assign the newly generated ID to the playerPrefab's netComp
         * Then, assign the player to localPlayer and send an InstantiationPacket over the network.
         */

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        socket.Blocking = false;

        isConnected = true;

        socket.Send(new ClientReadyPacket(true, gameObject.name).Serialize());      //This should send a player connected packet.
       // print("Sending client connection status to server");
    }

    void Update()
    {
        if (isConnected)
        {
            if (socket.Available > 0)
            {
                DeserializePackets();
                //print($"{gameObject.name} has received a designation of {clientDesignation} from the server.");

                if (clientsLinked)
                {
                    if (clientDesignation != null)
                    {
                        if (localPlayer == null)
                        {
                            SpawnController(prefabName, clientDesignation, clientID.ToString());
                            //print($"{localName} has been instantiated for {clientDesignation}");
                        }

                    }

                    if (playerComps.Count >= 2)
                    {
                        if (!playersAdded)
                        {
                            netObjs = playerComps.ToArray();
                            playersAdded = true;
                        }
                    }
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
        //print($"Packet received {pb.objID}");

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
                if (!clientsLinked)
                {
                    if (clientDesignation == null)
                    {
                        clientDesignation = piPack.clientDesignation;
                       // print($"Welcome {clientDesignation}");
                        clientsLinked = true;
                    }
                }

                break;

            case GameBasePacket.PacketType.Instantiate:
                InstantiateObjPacket iPack = (InstantiateObjPacket)new InstantiateObjPacket().DeSerialize(receivedBuffer);
                if (clientDesignation != iPack.objID)
                {
                    Guid tempID = Guid.Parse(iPack.ownershipID);

                    if (clientID != tempID)
                    {
                        if (partnerPlayer == null)
                        {
                            SpawnController(iPack.prefabName, iPack.objID, iPack.ownershipID.ToString());
                           // print($"{partnerName} has been instantiated for {partnerDesignation}");
                        }
                    }
                }

                break;

            default:
                break;
        }

        for (int i = 0; i < netObjs.Length; i++)
        {
            if (netObjs[i].gameObjID == pb.objID)
            {
                //print($"{netObjs[i].name} Object found. Providing Packet {pb.Type}");
                netObjs[i].UpdateComponent(receivedBuffer);

                //if (pb.Type == GameBasePacket.PacketType.PlayerController)
                   // print($"{pb.Type} packet has been received. Sending to {partnerName}");
            }
        }
    }

    void SpawnController(string prefabName, string designation, string ownerID)
    {
        GameObject prefabController = InstantiateController(prefabName, designation);

        if (designation == clientDesignation)
        {
            if (localPlayer == null)
            {
                localPlayer = prefabController.GetComponent<PlayerNetComp>();
                playerComps.Add(localPlayer);

                localPlayer.localID = Guid.Parse(ownerID);

                localPlayer.gameObjID = prefabController.name;
                localName = localPlayer.gameObjID;

                localPlayer.playerIdDisplay = localPlayer.localID.ToString();
                localIdDisplay = localPlayer.playerIdDisplay;

                localDesignation = localPlayer.playerDesignation;
            }

            SendInstantiationRequest(prefabName, localPlayer.localID.ToString(), designation);
        }
        else if (designation != clientDesignation)
        {
            if (partnerPlayer == null)
            {
                partnerPlayer = prefabController.GetComponent<PlayerNetComp>();
                playerComps.Add(partnerPlayer);

                partnerPlayer.localID = Guid.Parse(ownerID);

                partnerPlayer.gameObjID = prefabController.name;
                partnerName = partnerPlayer.gameObjID;

                partnerPlayer.playerIdDisplay = partnerPlayer.localID.ToString();
                partnerIdDisplay = partnerPlayer.playerIdDisplay;

                partnerDesignation = partnerPlayer.playerDesignation;
            }
        }
    }

    public GameObject InstantiateController(string prefabName, string designation)
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");
        Vector3 spawnPos;

        if (designation != null)
        {
            if (designation.Equals("Player1"))
            {
                spawnPos = p1Spawn.position;

                prefab = Instantiate(prefab, spawnPos, Quaternion.identity);
                prefab.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (designation.Equals("Player2"))
            {
                spawnPos = p2Spawn.position;
                prefab = Instantiate(prefab, spawnPos, Quaternion.identity);
                prefab.GetComponent<Renderer>().material.color = Color.blue;
            }

            prefab.name = /*playerName.text*/ designation + " Controller"/* + $" ({})"*/;
            prefab.GetComponent<PlayerNetComp>().playerDesignation = designation;
        }

        return prefab;
    }

    public void SendInstantiationRequest(string prefabName, string ownerID, string designation)
    {
        if (designation != null)
        {
            socket.Send(new InstantiateObjPacket(prefabName, ownerID, designation).Serialize());
        }
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
