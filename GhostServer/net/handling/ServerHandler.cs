using System;
using GhostServer.client;
using GhostServer.net;
using GhostServer.tools;
using GhostServer.tools.data;

namespace GhostServer.net.handling
{
    public class ServerHandler
    {
        public static void handlerPacket(ReceiveOperationCode.Login recvops, LittleEndianAccessor lea, GhostClient gc)
        {
            switch (recvops)
            {
                case ReceiveOperationCode.Login.LOGIN_REQ:
                    net.handling.Login.Login_Req(lea, gc);
                    break;
                case ReceiveOperationCode.Login.SERVERLIST_REQ:
                    net.handling.Login.ServerList_Req(lea, gc);
                    break;
                case ReceiveOperationCode.Login.GAME_REQ:
                    net.handling.Login.Game_Req(lea, gc);
                    break;
                default:
                    Console.WriteLine("未知Login Receive Operation Code {0}", recvops.ToString());
                    break;
            }
        }

        public static void handlerPacket(ReceiveOperationCode.Chars recvops, LittleEndianAccessor lea, GhostClient gc)
        {
            switch (recvops)
            {
                case ReceiveOperationCode.Chars.MYCHAR_INFO_REQ:
                    net.handling.Chars.MyChar_Info_Req(lea, gc);
                    break;
                case ReceiveOperationCode.Chars.CREATE_MYCHAR_REQ:
                    net.handling.Chars.Create_MyChar_Req(lea, gc);
                    break;
                case ReceiveOperationCode.Chars.CHECK_SAMENAME_REQ:
                    net.handling.Chars.Check_SameName_Req(lea, gc);
                    break;
                case ReceiveOperationCode.Chars.DELETE_MYCHAR_REQ:
                    net.handling.Chars.Delete_MyChar_Req(lea, gc);
                    break;
                default:
                    Console.WriteLine("未知Chars Receive Operation Code {0}", recvops.ToString());
                    break;
            }
        }

        public static void handlerPacket(ReceiveOperationCode.Game recvops, LittleEndianAccessor lea, GhostClient gc)
        {
            switch (recvops)
            {
                case ReceiveOperationCode.Game.CHARACTER_SELECT:
                    break;
                default:
                    Console.WriteLine("未知Field Receive Operation Code {0}", recvops.ToString());
                    break;
            }
        }
    }
}
