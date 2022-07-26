using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PlayTimePackets;

namespace LobbyServer
{
    public class LobbyServer
    {        
        static void Main(string[] args)
        {
            string name = "THE KILLERS";
            int port = 3301;
            Guid hostID = Guid.Empty;
            Player HostPlayer = new Player("host", true);

            if (args.Length == 3)
            {

                name = args[0];
                port = Int32.Parse(args[1]);
                hostID = new Guid(args[2]);
            }
            HostPlayer = new Player("Host", hostID);
            Console.WriteLine("lobby name is " + name + " with port number: " + port + "Host ID: " + hostID);
            Console.WriteLine("Built1");

            Socket MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3300));
            LobbyInformationPacket lp = new LobbyInformationPacket(name, 1000, port, HostPlayer, HostPlayer.ID);
            Console.WriteLine(lp.hostID);
            MainSocket.Send(new LobbyInformationPacket(name, 1000, port, HostPlayer, HostPlayer.ID).Serialize());


            Console.WriteLine("Connected To Main Server");

            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            listeningSocket.Listen(10);
            listeningSocket.Blocking = false;

            List<Socket> clients = new List<Socket>();

            Console.WriteLine("Lobby Waiting For Clients");

            while (true)
            {
                //Client Coonect
                try
                {
                    clients.Add(listeningSocket.Accept());
                    Console.WriteLine("Client Connected To " + name);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
                //Client Packet Loop
                for (int i = 0; i < clients.Count; i++)
                {
                    try
                    {
                        byte[] recievedBuffer = new byte[clients[i].Available];
                        clients[i].Receive(recievedBuffer);
                        /*MessagePacket packet = (MessagePacket)new MessagePacket().DeSerialize(recievedBuffer);
                        Console.WriteLine($"{packet.player.Name} is saying {packet.Message}");*/

                        //Check for Kick Request
                        //Check for Leave Request (If Host Close Lobby)
                        //Check for Start Game

                        /*LobbyPacket lb = (LobbyPacket)new LobbyPacket().DeSerialize(recievedBuffer);
                        Console.WriteLine(lb.Name + " Has Joined The Lobby" + lb.player);*/

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

                //Main Server Packet Loop
                try
                {
                    byte[] recievedBuffer = new byte[MainSocket.Available];
                    MainSocket.Receive(recievedBuffer);
                    //Look for packet from main
                    BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                    switch (pb.Type)
                    {
                        case BasePacket.PacketType.DisplayLobby:
                            MainSocket.Send(new DisplayLobbiesPacket().Serialize());
                            break;
                    }

                    // Check for roomcode request
                    // Check for port request
                    // Check for name request
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock) throw;
                }
            }
        }

        public void GetRoomCode()
        {

        }

        public void GetPort()
        {

        }
        public void GetName()
        {

        }
        public void KickPartner()
        {

        }
        public void CloseLobby()
        {

        }
        public void GetLobbyInfo()
        {

        }
    }
}