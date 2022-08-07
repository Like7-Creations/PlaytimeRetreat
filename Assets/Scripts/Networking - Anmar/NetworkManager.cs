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
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
   // [SerializeField] GameObject connectPanel;
    [SerializeField] Button CreateRoomButton;
    [SerializeField] Button JoinARoomButton;
    [SerializeField] Button sendButton;
    [SerializeField] Button ConnectButton;
    [SerializeField] Button KickButton;
    [SerializeField] Button LeaveButton;
    [SerializeField] Button StartButton;
    [SerializeField] public string ChosenLobbyName;

    [SerializeField] Canvas canvas;
    [SerializeField] TMP_InputField RoomName;
    [SerializeField] TMP_InputField ChatInput;
    [SerializeField] TMP_InputField RoomCodeInput;

    [SerializeField] TextMeshProUGUI KickMessage;
    [SerializeField] TextMeshProUGUI RoomCode;
    [SerializeField] TextMeshProUGUI PartnerUsername;
    
    
    [SerializeField] TextMeshProUGUI TextPrefabForChat;
    [SerializeField] GameObject ChatPanel;

    [SerializeField] List<Button> lobbyButtonNames;
    [SerializeField] GameObject ContentPanel;
    [SerializeField] Button buttonPrefabForLobbyNames;
    [SerializeField] List<int> GrabbedRoomCodes;

    [SerializeField] bool client;
    [SerializeField] bool host;
    int clienthost;

    int CharacterId;

    delegate void ConnectedEvent();
    ConnectedEvent connectEvent;

    //in case kick happens these need to be enabled and disabled
    public GameObject MainMenuPage;
    public GameObject LobbyPage;

    //__________


    Player player;
    Socket MainSocket;
    Socket lobbySocket;
    List<PlayerController> playerss = new List<PlayerController>();
    PlayerController[] playerController;

    string playerPrefabPath = "Prefabs/PlayerController";
    bool connected;
    void Start()
    { 
        MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        MainSocket.Blocking = false;
        player = new Player(Guid.NewGuid().ToString());

        //lobbySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        print("PlayerID is " + player.ID);
        MainSocket.Send(new CreatePlayerPacket(player.Name, player.ID, player).Serialize());

        CreateRoomButton.onClick.AddListener(() =>
        {
            try
            {
                host = true;
                client = false;
                //RoomCode.text = RoomName.text;

                print("connecting to created lobby server");

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
            MainSocket.Send(new DisplayLobbiesPacket("hello" , player).Serialize());
            print("Sending Request To Show Lobbies");
            client = true;
            host = false;
        });
        sendButton.onClick.AddListener(() =>
        {
            //chatlogs.text = (username.text + ": " + ChatInput.text);
            TextMeshProUGUI text = Instantiate(TextPrefabForChat, new Vector3(0,0,0),Quaternion.identity);
            text.text = ChatInput.text;
            text.transform.parent = canvas.transform;
            text.transform.parent = ContentPanel.transform;
            print("f");
            MainSocket.Send(new MessagePacket(ChatInput.text, player).Serialize());
        });
        ConnectButton.onClick.AddListener(() =>
        {
        //check if inputted roomcode is one of the grabbedroomcodes...
            int roomcode = int.Parse(RoomCodeInput.text);
            print(roomcode);
            MainSocket.Send(new JoinRequestPacket(ChosenLobbyName,roomcode,"null", player).Serialize());
        });
        StartButton.onClick.AddListener(() =>
        {
            if(host) 
                MainSocket.Send(new StartGamePacket("start", player).Serialize());
        });
        KickButton.onClick.AddListener(() =>
        {
            if(host) 
                MainSocket.Send(new KickRequestPacket("kickrequest", player).Serialize());
        });
        LeaveButton.onClick.AddListener(() =>
        {
            BackToMainServer();
        });
        /*for (int i = 0; i < lobbyButtonNames.Count; i++)
        {
            if (lobbyButtonNames[i] != null)
            {
                lobbyButtonNames[i].onClick.AddListener(() =>
                {
                    //ChosenLobbyName = lobbyButtonNames[i].transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text;
                    //print(ChosenLobbyName);
                    print("button list testing");
                });
            }
        }*/
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
                        print($" someone spit: {mp.Message}");
                        TextMeshProUGUI text = Instantiate(TextPrefabForChat);
                        text.text = mp.Message;
                        text.transform.parent = ContentPanel.transform;
                        //ClientTest.text = (mp.player.Name + ("is Saying ") + mp.Message);

                        break;

                       
                    //lobby information packet...and joining a lobby
                    case BasePacket.PacketType.Lobby:
                        LobbyInformationPacket lp = (LobbyInformationPacket)new LobbyInformationPacket().DeSerialize(recievedBuffer);
                        string name = lp.Name;
                        int roomcode = lp.RoomCode;
                        int portnumber = lp.LobbyPort;
                        print("Connecting to " + lp.Name + "with port " + portnumber);
                        MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), portnumber));
                        MainSocket.Blocking = false;
                        print("I have connected to " + name);
                        
                       // MainSocket.Send(new DisplayLobbiesPacket().Serialize());
                        break;

                        // join request packet
                    case BasePacket.PacketType.JoinRequest:
                        //this is in case the room code inputted was invalid
                        JoinRequestPacket jrp = (JoinRequestPacket)new JoinRequestPacket().DeSerialize(recievedBuffer);
                        print(jrp.RequestYN);
                        BackToMenuScreen();
                        break;


                    //Recieve Display Lobbies Packet
                    case BasePacket.PacketType.LobbyName:
                        LobbyNamesPacket lnp = (LobbyNamesPacket)new LobbyNamesPacket().DeSerialize(recievedBuffer);
                        //GrabbedroomNames = lnp.LobbyNames;
                        for (int i = 0; i < lnp.LobbyNames.Count; i++)
                        {
                            string.Join(", ", lnp.LobbyNames);
                            print(lnp.LobbyNames[i]);
                            print("Recieved lobby name: " + lnp.LobbyNames[i]);
                            Button instantiateButton = Instantiate(buttonPrefabForLobbyNames);
                            instantiateButton.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = lnp.LobbyNames[i];
                            lobbyButtonNames.Add(instantiateButton);
                            lobbyButtonNames[i].transform.parent = ContentPanel.transform;
                           /* GrabbedroomNames.Add(lnp.LobbyNames);
                            GrabbedLobbyCodes.Add(lnp.RoomCodes);*/
                        }
                        break;

                        //kick request recieve as a client
                    case BasePacket.PacketType.KickRequest:
                        KickRequestPacket krp = (KickRequestPacket)new KickRequestPacket().DeSerialize(recievedBuffer);
                        if (client)
                        {
                            KickMessage.gameObject.SetActive(true);
                            KickMessage.text = krp.Request;
                            print("I got kicked out! or lobby is full");
                            BackToMainServer();
                            //SceneManager.LoadScene("_MainMenu");
                            print("connected back to main server");
                        }
                        //for leaving...if client...relaunch main scene
                        //if host send packet to cancel the lobby and remove it off the list of lobbies in the main server... 
                        break;
                    case BasePacket.PacketType.StartGame:
                        SceneManager.LoadScene("Chris Scene");
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

    public void BackToMainServer()
    {
        MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3300));
        MainSocket.Blocking = false;
        BackToMenuScreen();
    }
    public void BackToMenuScreen()
    {
        LobbyPage.gameObject.SetActive(false);
        MainMenuPage.gameObject.SetActive(true);
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
    private void OnApplicationQuit()
    {
        MainSocket.Shutdown(SocketShutdown.Both);
        MainSocket.Disconnect(false);
        print("Disconnected from server");
    }
}
