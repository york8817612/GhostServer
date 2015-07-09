using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;

namespace GhostServer.server
{
    public class World
    {
        public byte ID { get; set; }
        public IPAddress HostIP { get; set; }
        public short CharServerCount { get; set; } 
        public Dictionary<int, CharServer> CharServers { get; set; }

        public World()
        { 
        }
    }
}
