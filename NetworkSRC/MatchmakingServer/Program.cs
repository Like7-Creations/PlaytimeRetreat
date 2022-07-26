﻿//using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PlayTimePackets;
using System.Diagnostics;
using ServerFunctions;

namespace MatchmakingServer
{
    public class Program
    {
        //List<Socket> ClientSockets = new List<Socket>();
        List<Socket> LobbySockets = new List<Socket>();
        static int portOffset = 1;

        static void Main(string[] args)
        {

            Socket LobbyListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LobbyListeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3300));
            LobbyListeningSocket.Listen(20);
            LobbyListeningSocket.Blocking = false;

            Console.WriteLine("Lobby Socket Listening On Port: " + 3300);

            Socket ClientListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientListeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            ClientListeningSocket.Listen(2);
            ClientListeningSocket.Blocking = false;

            Console.WriteLine("Client Socket Listening On Port: " + 3000);

            List<Client> ClientSockets = new List<Client>();
            List<Socket> LobbySockets = new List<Socket>();

            Console.WriteLine("waiting for connection");


            while (true)
            {
                //Client Connect
                try
                {
                    ClientSockets.Add(new Client(ClientListeningSocket.Accept()));
                    Console.WriteLine("Client Connected");
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
                
                //Client Packet Loop
                for (int i = 0; i < ClientSockets.Count; i++)
                {
                    try
                    {
                        byte[] recievedBuffer = new byte[ClientSockets[i].Socket.Available];
                        ClientSockets[i].Socket.Receive(recievedBuffer);
                        BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                        switch (pb.Type)
                        {
                            //Check for create lobby
                            case BasePacket.PacketType.CreateLobby:
                                CreateLobbyPacket clp = (CreateLobbyPacket)new CreateLobbyPacket().DeSerialize(recievedBuffer);
                                CreateLobby(clp.Name, ClientSockets[i].Player.ID);
                                break;

                            //Check for Display Lobbies
                            case BasePacket.PacketType.DisplayLobby:
                               DisplayLobbies(LobbySockets);
                                break;

                            case BasePacket.PacketType.CreatePlayer:
                                CreatePlayerPacket cpp = (CreatePlayerPacket)new CreatePlayerPacket().DeSerialize(recievedBuffer);
                                Socket tempSocket = ClientSockets[i].Socket;
                                ClientSockets[i] = new Client(tempSocket,new Player(cpp.Name, cpp.Id));
                                break;
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode != SocketError.WouldBlock) throw;
                    }
                }
                
                //Lobby Connect
                try
                {
                    LobbySockets.Add(LobbyListeningSocket.Accept());
                    Console.WriteLine("Lobby Created & Running");
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
               
                //Lobby Packet Loop
                for (int i = 0; i < LobbySockets.Count; i++)
                {
                    
                    byte[] recievedBuffer = new byte[LobbySockets[i].Available];
                    if (LobbySockets[i].Available > 1)
                    {
                      //check for lobbypacket(roomcode)
                      //check for lobblypacket(lobbyport)
                      // check for lobbypackey(close)
                        LobbySockets[i].Receive(recievedBuffer);
                        BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                        switch (pb.Type)
                        {
                            case BasePacket.PacketType.Lobby:
                                LobbyInformationPacket lp = (LobbyInformationPacket)new LobbyInformationPacket().DeSerialize(recievedBuffer);
                                Console.WriteLine("creds recieved, grabbed: " + lp.Name + " with Lobby port: " + lp.LobbyPort + " RoomCode: " + lp.RoomCode);
                                for (int e = 0; e < ClientSockets.Count; e++)
                                {
                                    if(ClientSockets[e].Player.ID == lp.hostID)
                                    {
                                        ClientSockets[e].Socket.Send(lp.Serialize());
                                        //JoinClientToLobby(ClientSockets[e], lp.Name, lp.RoomCode, lp.LobbyPort);
                                        //Console.WriteLine(lp.player.ID + "Has Successfully Connected To " + lp.Name);
                                        ClientSockets.Remove(ClientSockets[e]);
                                    }
                                }
                                //JoinClientToLobby(ClientSockets[i], lp.Name, lp.RoomCode, lp.LobbyPort, lp.player);
                                break;
                        }
                    }
                }
            }

        }

        static void CreateLobby(string name, Guid clientId)
        {
            //spawm lobby
            Process lobby = new Process();
            //lobby.StartInfo.CreateNoWindow = false;
            lobby.StartInfo.UseShellExecute = true;
            lobby.StartInfo.FileName = "LobbyServer.exe";
            lobby.StartInfo.Arguments = $"{name} {3300 + portOffset} {clientId}";
            lobby.Start();
            portOffset++;

            //wait until 
            //Socket createSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //AddLobbyToList(createSocket);
        }

        static void RemoveLobbyFromList(Socket socket)
        {
            //LobbySockets.Remove(socket);
        }

        static void JoinClientToLobby(Client client,string name, int roomcode, int port)
        {
            client.Socket.Send(new LobbyInformationPacket(name, roomcode, port, client.Player, client.Player.ID).Serialize());
        }

        static void AddClientSocket(Socket socket, List<Socket> sockets)
        {
            sockets.Add(socket);
        }
        static void RemoveClientSocket(Socket socket, List<Socket> sockets)
        {
            sockets.Remove(socket);
        }
        static void DisplayLobbies(List<Socket> listOfSockets)
        {
            for (int i = 0; i < listOfSockets.Count; i++)
            {
                listOfSockets[i].Send(new DisplayLobbiesPacket().Serialize());
            }
        }
    }
}