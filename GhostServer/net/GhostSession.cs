using System;
using System.Net;
using System.Net.Sockets;
using GhostServer.server;

namespace GhostServer.net
{
    public abstract class GhostSession : IDisposable
    {
        private Socket m_socket;
        private byte[] m_recvBuff;
        private byte[] m_buff;
        private int m_offset;
        private bool m_disposed;
        private object m_sendSync;
        private ServerType serverType;

        public const int ReceiveSize = 1024;

        public string Title { get; set; }

        public bool Disposed
        {
            get
            {
                return m_disposed;
            }
        }

        public GhostSession(Socket socket, ServerType serverType)
        {
            this.m_socket = socket;
            this.m_socket.NoDelay = true;

            this.Title = socket.RemoteEndPoint.ToString().Split(':')[0];

            this.m_recvBuff = new byte[ReceiveSize];
            this.m_buff = new byte[ReceiveSize];
            this.m_offset = 0;

            this.m_disposed = false;

            this.m_sendSync = new object();

            this.serverType = serverType;

            Register(socket, serverType);
            messageReceiving();
        }

        protected abstract void Register(Socket socket, ServerType serverType);
        protected abstract void UnRegister(Socket socket, ServerType serverType);
        protected abstract void messageReceived(Socket socket, byte[] message);

        private void messageReceiving()
        {
            if (m_disposed)
                return;
            SocketError errorCode = SocketError.Success;

            m_socket.BeginReceive(m_recvBuff, 0, m_recvBuff.Length, SocketFlags.None, out errorCode, endMessageReceiving, null);

            if (errorCode != SocketError.Success)
                Dispose();
        }

        private void endMessageReceiving(IAsyncResult ar)
        {
            if (!m_disposed)
            {
                SocketError errorCode = SocketError.Success;
                int length = m_socket.EndReceive(ar, out errorCode);

                if (errorCode != SocketError.Success || length == 0)
                {
                    Dispose();
                }
                else
                {
                    Append(length);
                    ManipulateBuffer();
                    messageReceiving();
                }
            }
        }

        private void Append(int length)
        {
            if (m_offset < 0)
            {
                m_offset = 0;
            }
            if (m_buff.Length - m_offset < length)
            {
                int newSize = m_buff.Length * 2;
                while (newSize < m_offset + length)
                {
                    newSize *= 2;
                }
                Array.Resize<byte>(ref m_buff, newSize);
            }
            else
            {
                Array.Resize<byte>(ref m_buff, length);
            }
            Buffer.BlockCopy(m_recvBuff, 0, m_buff, m_offset, length);
            m_offset += length;
        }

        private void ManipulateBuffer()
        {
            while (m_offset >= 4 && m_disposed == false)
            {
                Console.WriteLine("ClientTo::{0}", BitConverter.ToString(m_buff));
                int size;
                switch (serverType)
                {
                    case ServerType.LoginServer:
                        {
                            // m_buff[0] = 0xAA
                            // m_buff[1] = 0x55
                            size = (m_buff[2]) + (m_buff[3] << 8);
                            // m_buff[4] = PacketHeader[0]
                            // m_buff[5] = PacketHeader[1]

                            if (m_offset < size + 6) return;
                            break;
                        }
                    case ServerType.CharServer:
                    case ServerType.GameServer:
                        {
                            // m_buff[0] = 0x4D
                            // m_buff[1] = 0x00
                            // m_buff[2] = PacketHeader[0]
                            // m_buff[3] = PacketHeader[1]
                            size = (m_buff[4]) + (m_buff[5] << 8);

                            if (m_offset < size) return;
                            break;
                        }
                    default:
                        {
                            // m_buff[0] = 0x4D
                            // m_buff[1] = 0x00
                            // m_buff[2] = PacketHeader[0]
                            // m_buff[3] = PacketHeader[1]
                            size = (m_buff[4]) + (m_buff[5] << 8);

                            if (m_offset < size) return;
                            break;
                        }
                }

                byte[] packetBuff;

                switch (serverType)
                {
                    case ServerType.LoginServer:
                        {
                            packetBuff = new byte[size];
                            Buffer.BlockCopy(m_buff, 4, packetBuff, 0, packetBuff.Length);
                            break;
                        }
                    case ServerType.CharServer:
                    case ServerType.GameServer:
                        {
                            packetBuff = new byte[size - 2];
                            Buffer.BlockCopy(m_buff, 2, packetBuff, 0, packetBuff.Length);
                            break;
                        }
                    default:
                        {
                            packetBuff = new byte[size - 2];
                            Buffer.BlockCopy(m_buff, 2, packetBuff, 0, packetBuff.Length);
                            break;
                        }
                }

                m_offset -= size + 6;
                if (m_offset > 0)
                {
                    Buffer.BlockCopy(m_buff, size + 6, m_buff, 0, m_offset);
                }
                
                messageReceived(m_socket, packetBuff);
            }
        }

        public void SendPacket(byte[] data)
        {
            if (m_disposed)
                return;

            lock (m_sendSync)
            {
                if (m_disposed)
                    return;

                byte[] ret;
                byte[] header;

                switch (serverType) 
                {
                    case ServerType.LoginServer: 
                    {
                        ret = new byte[data.Length + 6];
                        header = new byte[4] { 0xAA, 0x55, (byte)(data.Length & 0xFF), (byte)((data.Length >> 8) & 0xFF) };
                        Buffer.BlockCopy(header, 0, ret, 0, 4); // copy header to ret
                        Buffer.BlockCopy(data, 0, ret, 4, data.Length); // copy packet to ret
                        Buffer.BlockCopy(new byte[2] { 0x55, 0xAA }, 0, ret, data.Length + 4, 2); // copy end to ret
                        break;
                    }
                    case ServerType.CharServer:                        
                    case ServerType.GameServer: 
                    {
                        ret = new byte[data.Length + 2];
                        int a = 0x4D;
                        int b = (data[0]) + (data[1] << 8);
                        int c = ret.Length;
                        int crc = a + b + c;
                        header = new byte[8] { 0x4D, 0x00, 
                            data[0], data[1], 
                            (byte)(ret.Length & 0xFF), (byte)((ret.Length >> 8) & 0xFF),
                            (byte)(crc & 0xFF), (byte)((crc >> 8) & 0xFF)
                        };
                        Buffer.BlockCopy(header, 0, ret, 0, 8); // copy header to ret
                        Buffer.BlockCopy(data, 6, ret, 8, data.Length - 6); // copy packet to ret
                        break;
                    }
                    default: 
                    {
                        ret = new byte[data.Length + 2];
                        int a = 0x4D;
                        int b = (data[0]) + (data[1] << 8);
                        int c = ret.Length;
                        int crc = a + b + c;
                        header = new byte[8] { 0x4D, 0x00, 
                            data[0], data[1], 
                            (byte)(ret.Length & 0xFF), (byte)((ret.Length >> 8) & 0xFF),
                            (byte)(crc & 0xFF), (byte)((crc >> 8) & 0xFF)
                        };
                        Buffer.BlockCopy(header, 0, ret, 0, 8); // copy header to ret
                        Buffer.BlockCopy(data, 6, ret, 8, data.Length - 6); // copy packet to ret
                        Console.WriteLine("{0} 為未設定的伺服器型態。", serverType.ToString());
                        break;
                    }
                }                
                SendRawPacket(ret);
            }
        }

        public bool SendRawPacket(byte[] final)
        {
            int offset = 0;
            while (offset < final.Length)
            {
                SocketError errorCode = SocketError.Success;
                Console.WriteLine("ServerTo::{0}", BitConverter.ToString(final));
                int sent = m_socket.Send(final, offset, final.Length - offset, SocketFlags.None, out errorCode);

                if (sent == 0 || errorCode != SocketError.Success)
                {
                    Dispose();
                    return false;
                }
                offset += sent;
            }
            return true;
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;

                UnRegister(m_socket, serverType);

                try
                {                    
                    m_socket.Shutdown(SocketShutdown.Both);
                    m_socket.Disconnect(false);
                    m_socket.Dispose();
                }
                catch { }

                m_buff = null;
                m_offset = 0;
            }
        }
    }
}
