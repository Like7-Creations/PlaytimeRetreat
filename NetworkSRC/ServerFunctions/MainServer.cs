using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ServerFunctions
{
    public class MainServer
    {
        public void CreateLobby(string name, int portOffset)
        {
            //spawm lobby
            Process.Start(@"LobbyServer.exe", $"{name} {3300 + portOffset}");
            portOffset++;
            //wait until 
            //Socket createSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //AddLobbyToList(createSocket);
        }

        public void RemoveLobbyFromList(Socket socket)
        {
            //LobbySockets.Remove(socket);
        }

        public void JoinClientToLobby()
        {

        }

        void AddClientSocket()
        {

        }
        void RemoveClientSocket()
        {

        }
        void DisplayLobbies()
        {

        }
    }
}
