using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PlayTimePackets;

namespace PlayTimeServer
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            listeningSocket.Listen(2);
            listeningSocket.Blocking = false;

            List<Socket> clients = new List<Socket>();

            while (true)
            {
                try
                {
                    clients.Add(listeningSocket.Accept());
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }

                for (int i = 0; i < clients.Count; i++)
                {
                    // clients[i].Receive(recievedBuffer);
                    try
                    {
                        byte[] recievedBuffer = new byte[clients[i].Available];
                        clients[i].Receive(recievedBuffer);
                        /*MessagePacket packet = (MessagePacket)new MessagePacket().DeSerialize(recievedBuffer);
                        Console.WriteLine($"{packet.player.Name} is saying {packet.Message}");*/
                        LobbyPacket lb = (LobbyPacket)new LobbyPacket().DeSerialize(recievedBuffer);
                        Console.WriteLine(lb.Name + " Has Joined The Lobby");

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
    }
}
