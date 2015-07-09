using GhostServer.tools.data;
using GhostServer.net;
using GhostServer.client;

namespace GhostServer.net.Packet
{
    public class Login
    {
        /* state
         * 07 - 因使用Bug進行遊戲，帳號已凍結
         * 08 - 因不正當賺取金錢，帳號已凍結
         * 09 - 因口出穢言，帳號已凍結
         * 10 - 因洗頻，帳號已凍結
         * 11 - 帳號暫時被凍結
         * 12 - 帳號已凍結
         * 13 - 帳號錯誤，請重新輸入
         * 14 - 您所輸入的密碼錯誤
         * 29 - 因偵測到不當的遊戲進行方式，1小時內將無法進行遊戲
         */
        /* NetCafe
         * 會員於特約網咖連線
         */
        public static byte[] Login_Ack(byte state, bool NetCafe)
        {
            PacketLittleEndianWriter plew = new PacketLittleEndianWriter();

            plew.write((byte) SendOperationCode.Login.LOGIN_ACK);
            plew.write(state);
            plew.writeBoolean(NetCafe);
            plew.writeShort(0);

            return plew.getPacket();
        }

        public static byte[] ServerList_Ack(byte serverId, string serverName, int[] loads)
        {
            PacketLittleEndianWriter plew = new PacketLittleEndianWriter();

            plew.write((byte)SendOperationCode.Login.SERVERLIST_ACK);
            plew.write(serverId);
            for (int i = 0; i < 12; i++)
            {
                plew.write(0xFF);
            }
            plew.writeInt(1); // 伺服器數量
            plew.writeShort(0); // 伺服器順序
            plew.writeInt((byte)loads.Length); // 頻道數量

            int id = 0;

            foreach (int chLoad in loads)
            {
                plew.writeShort((short)(id + 1));
                plew.writeShort((short)(id + 1));
                plew.writeGhostAsciiString("127.0.0.1");
                plew.writeInt(14101 + id);
                plew.writeInt(chLoad); // 玩家數量
                plew.writeInt(400); // 頻道人數上限
                plew.writeInt(12); // 標章類型
                plew.writeInt(0);
                plew.write(1);
                plew.writeInt(14199);
                id++;
            }

            return plew.getPacket();
        }

        /* state
         * 28 = 此為有年齡限制的頻道，請使用其他頻道
         * 04 = 此ID已連線，請稍後再試
         * else = 網路狀態錯誤
         */
        public static byte[] Game_Ack(GhostClient c, int state)
        {
            PacketLittleEndianWriter plew = new PacketLittleEndianWriter();

            plew.write((byte)SendOperationCode.Login.GAME_ACK);
            plew.write((byte)state);
            plew.writeGhostAsciiString("192.168.1.101");
            plew.writeInt(14101 + c.CharSID);
            plew.writeInt(14199);

            return plew.getPacket();
        }
    }
}
