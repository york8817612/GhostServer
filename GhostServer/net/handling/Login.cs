using GhostServer.client;
using GhostServer.net;
using GhostServer.net.Packet;
using GhostServer.tools;
using GhostServer.tools.data;
using System;

namespace GhostServer.net.handling
{
    public class Login
    {
        public static void Login_Req(LittleEndianAccessor lea, GhostClient gc)
        {
            string username = lea.readGhostAsciiString();
            string password = lea.readGhostAsciiString();

            if (username.IsAlphaNumeric() == false)
            {
                gc.SendPacket(net.Packet.Login.Login_Ack(13, false));
                return;
            }

            gc.Account = new Account(gc);
            try
            {
                gc.Account.Load(username);

                if (password != gc.Account.Password)
                {
                    gc.SendPacket(net.Packet.Login.Login_Ack(14, false));
                }
                else if (gc.Account.Banned > 0)
                {
                    gc.SendPacket(net.Packet.Login.Login_Ack(12, false));
                }
                else
                {
                    if (gc.Account.Master > 0)
                    {
                        gc.SendPacket(net.Packet.Login.Login_Ack(0, true));
                    }
                    else
                    {
                        gc.SendPacket(net.Packet.Login.Login_Ack(0, false));
                    }
                    gc.Account.LoggedIn = 1;
                }


            }
            catch (Exception ee)
            {
                if (ee.Message.Equals("account does not")) {
                    gc.SendPacket(net.Packet.Login.Login_Ack(13, false));
                } else {
                    Log.Error(ee.ToString());
                }
            }
        }

        public static void ServerList_Req(LittleEndianAccessor lea, GhostClient gc)
        {
            gc.SendPacket(net.Packet.Login.ServerList_Ack(0, new int[] { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}));
        }

        public static void Game_Req(LittleEndianAccessor lea, GhostClient gc)
        {
            gc.SendPacket(net.Packet.Login.Game_Ack(gc, 0));
        }
    }
}
