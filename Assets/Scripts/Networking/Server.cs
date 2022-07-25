using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PlayTimePackets;
using UnityEngine.UI;
using TMPro;

public class Server : MonoBehaviour
{

    List<Socket> clients = new List<Socket>();
    Socket listeningSocket;

    [SerializeField] TMP_InputField HostUsername;
    [SerializeField] TextMeshProUGUI MyUsername;
    [SerializeField] TextMeshProUGUI ClientUsername;
    [SerializeField] Button HostButton;
    Player player;
    int number = 0;

    void Start()
    {
        /*listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
        listeningSocket.Listen(2);
        listeningSocket.Blocking = false;*/
    }

    void Update()
    {
        if (number == 0)
        {
            listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            listeningSocket.Listen(2);
            listeningSocket.Blocking = false;
            number = 1;
        }
        
        HostButton.onClick.AddListener(() =>
        {
            player = new Player(Guid.NewGuid().ToString(), HostUsername.text);
            MyUsername.text = HostUsername.text;
        });

         try
            {
                clients.Add(listeningSocket.Accept());
                Debug.Log("someoneConnected");
                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].Send(new LobbyPacket(MyUsername.text, player).Serialize());
                }
                //listeningSocket.Send(new LobbyPacket(MyUsername.text, player).Serialize());
            }
            catch (SocketException ex)
            {
                 if (ex.SocketErrorCode != SocketError.WouldBlock)
                     print(ex);
            }

        for (int i = 0; i < clients.Count; i++)
        {
            try
            {
                byte[] recievedBuffer = new byte[clients[i].Available];
                clients[i].Receive(recievedBuffer);
                // MessagePacket packet = (MessagePacket)new MessagePacket().DeSerialize(recievedBuffer);
                //  print($"{packet.player.Name} is saying {packet.Message}");
                LobbyPacket lb = (LobbyPacket)new LobbyPacket().DeSerialize(recievedBuffer);
                ClientUsername.text = lb.Name;
                for (int e = 0; e < clients.Count; e++)
                {
                    if (e != i)
                    {
                        clients[e].Send(recievedBuffer);
                    }
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.WouldBlock) throw;
            }
        }
    }
}
