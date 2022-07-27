using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using PlayTimePackets;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class NetworkManager : MonoBehaviour
{
   // [SerializeField] GameObject connectPanel;
    [SerializeField] Button CreateRoomButton;
    [SerializeField] Button JoinARoomButton;
    [SerializeField] TMP_InputField RoomName;
    [SerializeField] TextMeshProUGUI RoomCode;
    [SerializeField] TextMeshProUGUI PartnerUsername;

    int CharacterId;

    delegate void ConnectedEvent();
    ConnectedEvent connectEvent;


    Player player;
    Socket MainSocket;
    Socket lobbySocket;

    string playerPrefabPath = "Prefabs/PlayerController";
    bool connected;
    void Start()
    {
        MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        MainSocket.Blocking = false;
        player = new Player(Guid.NewGuid().ToString());

        lobbySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        print("PlayerID is " + player.ID);
        MainSocket.Send(new CreatePlayerPacket(player.Name, player.ID, player).Serialize());

        CreateRoomButton.onClick.AddListener(() =>
        {
            try
            {
                //RoomCode.text = RoomName.text;

                print("connecting to server");

                /*socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
                socket.Blocking = false;*/ //commented this cuz we are connecting right from start
                //connected = true;

                //socket.Send(new LobbyPacket(MyUsername.text, player).Serialize());
                MainSocket.Send(new CreateLobbyPacket(RoomName.text, player).Serialize());
                //socket.Disconnect(true);

                print("connecting successful & created lobby with name: " + RoomName.text);

                // this is just a quick testing keybpard sound on oibs lol
                
                if (connectEvent != null)
                    connectEvent();
            }
            catch (SocketException ex)
            {
                print(ex);
            }
        });
        JoinARoomButton.onClick.AddListener(() =>
        {
            MainSocket.Send(new DisplayLobbiesPacket().Serialize());
            if (connectEvent != null)
                connectEvent();
        });
       /* sendButton.onClick.AddListener(() =>
        {
            ///socket.Send(new MessagePacket(ChatInput.text, player).Serialize());
            chatlogs.text = (username.text + ": " + ChatInput.text);
        });*/
    }
    void Update()
    {
        try
        {
            if (MainSocket.Available > 0)
            {
                byte[] recievedBuffer = new byte[MainSocket.Available];
                MainSocket.Receive(recievedBuffer);
                BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                print("instantiating player for other players");
                switch (pb.Type)
                {
                    case BasePacket.PacketType.Message:
                        MessagePacket mp = (MessagePacket)new MessagePacket().DeSerialize(recievedBuffer);
                        print($"{mp.player.Name} spit: {mp.Message}");
                        //ClientTest.text = (mp.player.Name + ("is Saying ") + mp.Message);

                        break;

                    /*case BasePacket.PacketType.Instantiate:
                        InstantiatePacket instaPacket = (InstantiatePacket)new InstantiatePacket(playerPrefabPath, player).DeSerialize(recievedBuffer);*/

                        //GameObject playerSpawn = Resources.Load<GameObject>(playerPrefabPath);
                        //Instantiate(playerSpawn, Vector3.zero, Quaternion.identity);

                        //break;
                    
                    case BasePacket.PacketType.Lobby:
                        LobbyInformationPacket lp = (LobbyInformationPacket)new LobbyInformationPacket().DeSerialize(recievedBuffer);
                        string name = lp.Name;
                        int roomcode = lp.RoomCode;
                        int portnumber = lp.LobbyPort;
                        print("Connecting to " + lp.Name + "with port " + portnumber);
                        //MainSocket.Shutdown(SocketShutdown.Both);
                        //MainSocket.Disconnect(true);
                        MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), portnumber));
                        MainSocket.Blocking = false;
                        print("lobbySocket has connected to " + name);
                        MainSocket.Send(new DisplayLobbiesPacket().Serialize());
                        break;



                    default:
                        break;
                }
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine(ex);
        }
           
    }

    public void CreateLobby()
    {

    }

    public void DisplayLobbies()
    {

    }

    public void JoinLobby()
    {
        //request for the port
    }

    public void LeaveLobby()
    {

    }
}
