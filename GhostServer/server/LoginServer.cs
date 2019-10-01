using GhostServer.client;
using GhostServer.constants;
using GhostServer.database;
using GhostServer.tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GhostServer.server
{
    public class LoginServer
    {
        private static bool isAlive;
        private static ManualResetEvent AcceptDone;

        public static Worlds Worlds { get; private set; }
        public static List<GhostClient> Clients { get; private set; }

        public static bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;

                if (!value)
                {
                    LoginServer.AcceptDone.Set();
                }
            }
        }

        public static void ServerLoop() 
        {
            AcceptDone = new ManualResetEvent(false);
            Worlds = new Worlds();
            Clients = new List<GhostClient>();

            int PORT = ServerConstants.Login_Port;

            Log.SetLogFile(".\\Logs\\LoginLog.log");

            try
            {
                Settings.Initialize();

                Database.Test();
                Database.Analyze(false);

                TcpListener Listener = new TcpListener(IPAddress.Any, PORT);
                Listener.Start();
                Log.Success("The login server {0} is online.", Listener.LocalEndpoint);

                foreach (string worldName in Settings.GetBlocksFromBlock("Worlds", 1))
                {
                    Worlds.Add(new World()
                    {
                        ID = Settings.GetByte("ID", worldName),
                        HostIP = Settings.GetIPAddress("Host", worldName),
                        CharServerCount = Settings.GetShort("Channels", worldName),
                        //Rates = new Rates()
                        //{
                        //    Experience = 6,
                        //    QuestExperience = 6,
                        //    PartyQuestExperience = 6,

                        //    Meso = 3,
                        //    Loot = 2
                        //} // TODO: Actual rate load.
                    });
                }

                IsAlive = true;

                while (IsAlive)
                {
                    AcceptDone.Reset();
                    Listener.BeginAcceptSocket((iar) =>
                    {
                        new GhostClient(Listener.EndAcceptSocket(iar), ServerType.LoginServer);
                        AcceptDone.Set();
                    }, null);
                    AcceptDone.WaitOne();
                }

                GhostClient[] remainingClients = Clients.ToArray();
                foreach (GhostClient client in remainingClients)
                {
                    client.Dispose();
                }

                Listener.Stop();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.Read();
            }
        }

        public static void Stop()
        {
            IsAlive = false;
        }
    }
}
