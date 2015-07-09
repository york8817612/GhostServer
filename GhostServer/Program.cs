using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GhostServer.server;

namespace GhostServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread loginServer = new Thread(new ThreadStart(LoginServer.ServerLoop));
            loginServer.Start();
            Thread charServer = new Thread(new ThreadStart(CharServer.ServerLoop));
            charServer.Start();
            Thread gameServer = new Thread(new ThreadStart(GameServer.ServerLoop));
            gameServer.Start();
        }
    }
}
