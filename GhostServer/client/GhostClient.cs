using System;
using System.Net;
using System.Net.Sockets;
using GhostServer.net;
using GhostServer.server;
using GhostServer.tools;
using GhostServer.tools.data;

namespace GhostServer.client
{
    public sealed class GhostClient : GhostSession
    {
        public byte CharSID { get; private set; }
        public byte GameSID { get; private set; }
        public Account Account { get; set; }
        public long SessionID { get; private set; }
        public ServerType serverType { get; private set; }

        public World World
        {
            get
            {
                return LoginServer.Worlds[this.CharSID];
            }
            set
            {
                this.CharSID = value.ID;
            }
        }

        public GhostClient(Socket socket, ServerType serverType)
            :base(socket, serverType)
        {
            this.SessionID = Randomizer.nextLong();
            this.serverType = serverType;
        }

        protected override void Register(Socket socket, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.LoginServer:
                    LoginServer.Clients.Add(this);
                    Log.Inform("客戶端 [{0}] 已連線到登入伺服器。", socket.RemoteEndPoint);
                    break;
                case ServerType.CharServer:
                    CharServer.Clients.Add(this);
                    Log.Inform("客戶端 [{0}] 已連線到角色伺服器。", socket.RemoteEndPoint);
                    break;
                case ServerType.GameServer:
                    GameServer.Clients.Add(this);
                    Log.Inform("客戶端 [{0}] 已連線到遊戲伺服器。", socket.RemoteEndPoint);
                    break;
            }
        }

        protected override void UnRegister(Socket socket, ServerType serverType)
        {
            if (this.Account != null)
            {
                this.Account.LoggedIn = 0;
                this.Account.Save();
            }

            switch (serverType)
            {
                case ServerType.LoginServer:
                    LoginServer.Clients.Remove(this);
                    Log.Inform("客戶端 [{0}] 已關閉與登入伺服器的連線。", socket.RemoteEndPoint);
                    break;
                case ServerType.CharServer:
                    CharServer.Clients.Remove(this);
                    Log.Inform("客戶端 [{0}] 已關閉與角色伺服器的連線。", socket.RemoteEndPoint);
                    break;
                case ServerType.GameServer:
                    GameServer.Clients.Remove(this);
                    Log.Inform("客戶端 [{0}] 已關閉與遊戲伺服器的連線。", socket.RemoteEndPoint);
                    break;
            }
        }

        protected override void messageReceived(Socket socket, byte[] message)
        {
            LittleEndianAccessor lea = new LittleEndianAccessor(new ByteArrayByteStream(message));
            int hander;
            switch (this.serverType)
            {
                case ServerType.LoginServer:
                    hander = lea.readByte();
                    net.handling.ServerHandler.handlerPacket((ReceiveOperationCode.Login)hander, lea, this);
                    break;
                case ServerType.CharServer:
                    hander = lea.readShort();
                    net.handling.ServerHandler.handlerPacket((ReceiveOperationCode.Chars)hander, lea, this);
                    break;
                case ServerType.GameServer:
                    hander = lea.readShort();
                    net.handling.ServerHandler.handlerPacket((ReceiveOperationCode.Game)hander, lea, this);
                    break;
                default:
                    hander = lea.readShort();
                    net.handling.ServerHandler.handlerPacket((ReceiveOperationCode.Game)hander, lea, this);
                    break;
            }
        }        
    }
}
