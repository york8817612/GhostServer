using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhostServer.net
{
    public class ReceiveOperationCode
    {
        public enum Login : byte
        {
            LOGIN_REQ = 0x30,
            SERVERLIST_REQ = 0x32,
            GAME_REQ = 0x34
        }

        public enum Chars : short
        {
            MYCHAR_INFO_REQ = 0x8,
            CREATE_MYCHAR_REQ = 0xA,
            CHECK_SAMENAME_REQ = 0xC,
            DELETE_MYCHAR_REQ = 0xE
        }

        public enum Game : short
        {
            CHARACTER_SELECT = 0x88
        }
    }
}
