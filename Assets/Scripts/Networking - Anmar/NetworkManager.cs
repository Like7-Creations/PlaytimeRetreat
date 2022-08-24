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
    [SerializeField] TMP_InputField MyUserName;
    [SerializeField] TMP_InputField RoomName;
    [SerializeField] TMP_InputField ChatInput;
    [SerializeField] TMP_InputField RoomCodeInput;

    [SerializeField] TextMeshProUGUI KickMessage;
    [SerializeField] TextMeshProUGUI RoomCode;
    [SerializeField] TextMeshProUGUI PartnerUsername;
    [SerializeField] TextMeshProUGUI RoomCodeText;
    [SerializeField] TextMeshProUGUI UsernameWarn;
    [SerializeField] TextMeshProUGUI Hostname;
    [SerializeField] TextMeshProUGUI PartnerName;
    
    
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
    public GameObject UsernamePage;
    public GameObject MenuPanel;

    //__________


    Player player;
    Socket MainSocket;
    List<Socket> Lobbysockets = new List<Socket>();
    Socket LobbySocket;
    List<PlayerController> playerss = new List<PlayerController>();
    PlayerController[] playerController;

    public GameServerPort gameServerPort;

    string playerPrefabPath = "Prefabs/PlayerController";
    bool connected;
    void Start()
    { 
        MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        MainSocket.Blocking = false;
        player = new Player(Guid.NewGuid().ToString());

        LobbySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);



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

                RoomName.text = RoomName.text.Replace(" ", "-");
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
            LobbySocket.Send(new MessagePacket(ChatInput.text, player).Serialize());
        });
        ConnectButton.onClick.AddListener(() =>
        {
            int roomcode = 0;
            if (RoomCodeInput.text != "")
            {
                roomcode = int.Parse(RoomCodeInput.text);
                print(roomcode);
            }
            if (ChosenLobbyName != "")
            {
                MainSocket.Send(new JoinRequestPacket(ChosenLobbyName, roomcode, "null", player).Serialize());
            }
            else { BackToMenuScreen(); KickMessage.gameObject.SetActive(true); KickMessage.text = "Please Pick A Lobby First"; }
        });
        StartButton.onClick.AddListener(() =>
        {
            if(host)
                LobbySocket.Send(new StartGamePacket(0, player).Serialize());
        });
        KickButton.onClick.AddListener(() =>
        {
            if(host) 
                LobbySocket.Send(new KickRequestPacket("kickrequest", player).Serialize());
        });
        LeaveButton.onClick.AddListener(() =>
        {
            if(host) LobbySocket.Send(new LeaveRequestPacket(RoomName.text, host, player).Serialize());
            if(client) LobbySocket.Send(new LeaveRequestPacket(ChosenLobbyName, false, player).Serialize());
            LobbySocket.Shutdown(SocketShutdown.Both);
            LobbySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        });
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
                switch (pb.Type)
                {                       
                    //lobby information packet...and joining a lobby
                    case BasePacket.PacketType.Lobby:
                        LobbyInformationPacket lp = (LobbyInformationPacket)new LobbyInformationPacket().DeSerialize(recievedBuffer);
                        string name = lp.Name;
                        int roomcode = lp.RoomCode;
                        int portnumber = lp.LobbyPort;
                        RoomCodeText.text = "Room Code: " + lp.RoomCode.ToString();
                        print("Connecting to " + lp.Name + "with port " + portnumber);
                        LobbySocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), portnumber));
                        LobbySocket.Blocking = false;
                        if(host)LobbySocket.Send(new UsernamePacket(MyUserName.text, "Partner", player).Serialize());
                        if(client)LobbySocket.Send(new UsernamePacket("Host", MyUserName.text, player).Serialize());
                        print("I have connected to " + name);

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
                        if(ContentPanel.transform.childCount > 0 && lobbyButtonNames != null)
                        {
                            lobbyButtonNames.Clear();
                            for (int j = 0; j < ContentPanel.transform.childCount; j++)
                            {
                                Destroy(ContentPanel.transform.GetChild(j).gameObject);
                            }
                        }
                        for (int i = 0; i < lnp.LobbyNames.Count; i++)
                        {
                            string.Join(", ", lnp.LobbyNames);
                            print("Recieved lobby name: " + lnp.LobbyNames[i]);
                            Button instantiateButton = Instantiate(buttonPrefabForLobbyNames);
                            instantiateButton.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = lnp.LobbyNames[i];
                            lobbyButtonNames.Add(instantiateButton);
                            lobbyButtonNames[i].transform.parent = ContentPanel.transform;
                        } 
                        break;

                    default:
                        break;
                }
            }
            if(LobbySocket.Available > 0)
            {
                byte[] recievedBuffer = new byte[LobbySocket.Available];
                LobbySocket.Receive(recievedBuffer);
                BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
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

                    //kick request recieve as a client
                    case BasePacket.PacketType.KickRequest:
                        KickRequestPacket krp = (KickRequestPacket)new KickRequestPacket().DeSerialize(recievedBuffer);                     
                        KickMessage.gameObject.SetActive(true);
                        KickMessage.text = krp.Request;
                        print("I got kicked out! or lobby is full");
                        BackToMenuScreen();
                        LobbySocket.Shutdown(SocketShutdown.Both);
                        LobbySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        break;
                    case BasePacket.PacketType.StartGame:
                        StartGamePacket sgp = (StartGamePacket)new StartGamePacket().DeSerialize(recievedBuffer);
                        gameServerPort.GamePort = sgp.Gameport;
                        SceneManager.LoadScene("_Level 1");
                        break;

                    case BasePacket.PacketType.Usernames:
                        UsernamePacket unp = (UsernamePacket)new UsernamePacket().DeSerialize(recievedBuffer);
                        Hostname.text = unp.HostName;
                        PartnerName.text = unp.PartnerName;
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

    void OnApplicationQuit()
    {
        MainSocket.Send(new PlayerShutDownPacket("", player).Serialize());
        if (host) { LobbySocket.Send(new LeaveRequestPacket(RoomName.text, host, player).Serialize()); }
        if (LobbySocket.Connected) { LobbySocket.Send(new PlayerShutDownPacket("", player).Serialize()); }
    }

    public void BackToMenuScreen()
    {
        LobbyPage.gameObject.SetActive(false);
        MainMenuPage.gameObject.SetActive(true);
    }
    public void GoButton()
    {
        if (MyUserName.text != "")
        {
            UsernamePage.gameObject.SetActive(false);
            MenuPanel.gameObject.SetActive(true);
        }
        else {
            UsernamePage.gameObject.SetActive(true);
            UsernameWarn.gameObject.SetActive(true);
        }
    }
}
