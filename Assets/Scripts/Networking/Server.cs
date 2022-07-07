using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server : MonoBehaviour
{

    List<Socket> clients = new List<Socket>();
    Socket listeningSocket;

    void Start()
    {
        listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
        listeningSocket.Listen(2);
        listeningSocket.Blocking = false;
    }

    void Update()
    {
        try
        {
            clients.Add(listeningSocket.Accept());
        }
        catch (SocketException ex)
        {
            if (ex.SocketErrorCode != SocketError.WouldBlock)
                print(ex);
        }

        for (int i = 0; i < clients.Count; i++)
        {
            // clients[i].Receive(recievedBuffer);
            try
            {
                byte[] recievedBuffer = new byte[clients[i].Available];
                clients[i].Receive(recievedBuffer);
               // MessagePacket packet = (MessagePacket)new MessagePacket().DeSerialize(recievedBuffer);
              //  print($"{packet.player.Name} is saying {packet.Message}");
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
