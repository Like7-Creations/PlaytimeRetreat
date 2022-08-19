using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GamePackets;
using PlayTimePackets;

public class GameNetworkManager : MonoBehaviour
{
    public delegate void ConnectedToServer();
    public ConnectedToServer ServerConnectedEvent;

    public NetworkComponent[] netObjs;
    public List<GameObject> players = new List<GameObject>();

    [Header("Connect Panel")]
    [SerializeField] GameObject connectPanel;
    [SerializeField] TMP_InputField playerName;
    [SerializeField] Button connectButton;
    bool isConnected;

    [Header("Instate Prefabs")]
    [SerializeField] Button spawnButton;
    [SerializeField] GameObject objPrefab;

    public Socket socket;
    Player player;

    string objId;
    int ownerID;
    int gamePort;
    GameServerPort gameServerPort;

    void Start()
    {
        //Call instantiation function here
        netObjs = FindObjectsOfType<NetworkComponent>();

        //objId = 1;

        connectButton.onClick.AddListener(() =>
        {
            try
            {
                //player = new Player(playerName.text, Guid.NewGuid());
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
                socket.Blocking = false;

                isConnected = true;
                connectPanel.SetActive(false);
                //chatPanel.SetActive(true);


                if (ServerConnectedEvent != null)
                    ServerConnectedEvent();


                //InstantiateFromResources(objPrefab.name);
            }
            catch (SocketException ex)
            {
                print(ex);
            }
        });
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

        for (int i = 0; i < netObjs.Length; i++)
        {
            if (netObjs[i].gameObjID == pb.objID)
                netObjs[i].UpdateComponent(receivedBuffer);
        }

        /*switch (pb.Type)
        {
            case BasePacket.PacketType.Instantiate:
                InstantiateObjPacket pi = (InstantiateObjPacket)new InstantiateObjPacket().DeSerialize(receivedBuffer);
                InstantiateFromResources(pi.prefabName, pi.GameObjID);

                break;

            case BasePacket.PacketType.Rigidbody:
                RigidbodyPacket rPack = (RigidbodyPacket)new RigidbodyPacket().DeSerialize(receivedBuffer);
                for (int i = 0; i < netObjs.Count; i++)
                {
                    if (netObjs[i].gameObjID == rPack.GameObjID)
                    {
                        netObjs[i].UpdateRigidbody(rPack);
                    }
                }

                break;

            case BasePacket.PacketType.PlayerController:
                PlayerControllerPacket pcPack = (PlayerControllerPacket)new PlayerControllerPacket().DeSerialize(receivedBuffer);
                for (int i = 0; i < netObjs.Count; i++)
                {
                    if (netObjs[i].gameObject.GetComponent<PlayerController>())
                    {

                    }
                }

                break;

            case BasePacket.PacketType.Trigger:
                TriggerPacket tPack = (TriggerPacket)new TriggerPacket().DeSerialize(receivedBuffer);
                for (int i = 0; i < netObjs.Length; i++)
                {
                    if (netObjs[i].gameObjID == tPack.GameObjID)
                        netObjs[i].UpdateComponent(tPack);
                }


                break;


            default:
                break;
        }*/
    }

    /*public void InstantiateRequest(GameObject prefab)
    {
        print($"{player.Name} has requested the Instantiation of Object: {prefab.name}");
        socket.Send(new InstantiateObjPacket(prefab.name, player, objId).Serialize());

        //InstantiateFromResources(prefab.name);
    }*/

    public void RequestInstantiation(string prefabName, int objectID)
    {
        print($"{player.Name} has requested the Instantiation of Object: {prefabName}");
        //socket.Send(new InstantiateObjPacket(prefabName, /*player,*/ objectID).Serialize());

        //We would instantiate a player character and send a packet over the server.

        InstantiateFromResources(prefabName, objectID);

    }

    void InstantiateFromResources(string prefabName, int objectID)
    {
        GameObject prefabObj = Resources.Load<GameObject>($"Prefabs/{prefabName}");
        Instantiate(prefabObj, Vector3.zero, Quaternion.identity);

        prefabObj.AddComponent<NetworkComponent>();     //Ideally, this should already have been added to the prefab.

        //It should be added to the list of players.
        
        //prefabObj.GetComponent<NetworkComponent>().gameObjID = objectID;
        //objId++;
    }
}
