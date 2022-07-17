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

public class Client : MonoBehaviour
{
   // [SerializeField] GameObject connectPanel;
    [SerializeField] Button connectButton;
    [SerializeField] TMP_InputField username;
    [SerializeField] TextMeshProUGUI MyUsername;
    [SerializeField] TextMeshProUGUI PartnerUsername;

    delegate void ConnectedEvent();
    ConnectedEvent connectEvent;

    // [SerializeField] Button sendButton;
    // [SerializeField] TMP_InputField ChatInput;
    //  [SerializeField] GameObject chatPanel;
    // [SerializeField] TextMeshProUGUI chatlogs;
    // [SerializeField] TextMeshProUGUI ClientTest;

    Player player;
    Socket socket;

    string playerPrefabPath = "Prefabs/PlayerController";
    bool connected;
    void Start()
    {
        connectButton.onClick.AddListener(() =>
        {
            try
            {
                player = new Player(Guid.NewGuid().ToString(), username.text);
                MyUsername.text = username.text;

                print("connecting to server");

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
                socket.Blocking = false;
                connected = true;

                socket.Send(new LobbyPacket(MyUsername.text, player).Serialize());

                print("connecting successful");
                


                if (connectEvent != null)
                    connectEvent();
            }
            catch (SocketException ex)
            {
                print(ex);
            }
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
            if (socket.Available > 0)
            {
                byte[] recievedBuffer = new byte[socket.Available];
                socket.Receive(recievedBuffer);
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
                        LobbyPacket lp = (LobbyPacket)new LobbyPacket().DeSerialize(recievedBuffer);
                        PartnerUsername.text = lp.Name;
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
}
