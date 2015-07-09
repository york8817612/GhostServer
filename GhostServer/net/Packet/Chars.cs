using GhostServer.tools.data;
using GhostServer.net;
using GhostServer.client;
using GhostServer.client.Characters;
using System.Collections.Generic;

namespace GhostServer.net.Packet
{
    public class Chars
    {
        public static byte[] MyChar_Info_Ack(GhostClient gc, List<Character> chars)
        {
            PacketLittleEndianWriter plew = new PacketLittleEndianWriter();

            plew.writeShort((short)SendOperationCode.Chars.MYCHAR_INFO_ACK);
            plew.writeInt(0); // length + CRC
            plew.writeInt(0);
            plew.writeInt(chars.Count); // Characters.Count

            for (int i = 0; i < 4; i++)
            {
                getCharactersData(plew, (i < chars.Count) ? chars[i] : null);
            }

            return plew.getPacket();
        }

        /*
         * -2 = 無法創立新角色，請先購買角色擴充道具，最多可同時創立4個角色。
         * -1 = 使用中的名字
         * 0 = 現在無法創立角色，請稍後
         * 1 = 創建成功
         * 2 = 創建成功
         * 3 = 創建成功
         * 4 = 創建成功
         * else = 未知的錯誤
         */
        public static byte[] Create_MyChar_Ack(int pos)
        {
            PacketLittleEndianWriter plew = new PacketLittleEndianWriter();

            plew.writeShort((short)SendOperationCode.Chars.CREATE_MYCHAR_ACK);
            plew.writeInt(0); // length + CRC
            plew.writeInt(0);
            plew.writeInt(pos);

            return plew.getPacket();
        }

        /*
         * 0 = 使用中的名字
         * 1 = 此名稱可以使用
         * 2 = 無法創立新角色，請先購買角色擴充道具，最多可同時創立4個角色。
         * else = 未知的錯誤
         */
        public static byte[] Check_SameName_Ack(int state)
        {
            PacketLittleEndianWriter plew = new PacketLittleEndianWriter();

            plew.writeShort((short)SendOperationCode.Chars.CHECK_SAMENAME_ACK);
            plew.writeInt(0); // length + CRC
            plew.writeInt(0);
            plew.writeInt(state);

            return plew.getPacket();
        }

        /*
         * -2 = 未知的錯誤
         * -1 = 角色刪除失敗
         * 1 = 角色刪除成功
         * 2 = 角色刪除成功
         * 3 = 角色刪除成功
         * 4 = 角色刪除成功
         * else = 創建1小時後才能刪除
         */
        public static byte[] Delete_MyChar_Ack(int num)
        {
            PacketLittleEndianWriter plew = new PacketLittleEndianWriter();

            plew.writeShort((short)SendOperationCode.Chars.DELETE_MYCHAR_ACK);
            plew.writeInt(0); // length + CRC
            plew.writeInt(0);
            plew.writeInt(num);

            return plew.getPacket();
        }

        public static void getCharactersData(PacketLittleEndianWriter plew, Character c)
        {
            plew.writeAsciiString(c != null ? c.Name : "", 20);
            plew.writeAsciiString(c != null ? c.Title : "", 20); // Title
            plew.write(c != null ? c.Gender : (byte)0);
            plew.write(c != null ? c.Level : (byte)0);
            plew.write(c != null ? c.Class : (byte)0);
            plew.write(c != null ? c.ClassLV : (byte)0);
            plew.write(0);
            plew.write(0);
            plew.write(0);
            plew.write(0);
            plew.writeShort((short)(c != null ? 1 : 0));
            plew.writeShort((short)(c != null ? 1 : 0));
            plew.writeInt(c != null ? 8010101 : 0); // 武器[weapon]8010101
            plew.writeInt(c != null ? c.Hair : 0); // 頭髮[hair]9010011
            plew.writeInt(c != null ? 8493122 : 0); // 披風[mantle]8493122
            plew.writeInt(c != null ? 9010011 : 0); // 帽子[hat]8610011
            plew.writeInt(c != null ? 8610021 : 0); // 臉下[face2]9410021
            plew.writeInt(c != null ? 8710013 : 0); // 臉上[face]8710013
            plew.writeInt(c != null ? 9510081 : 0); // 服裝[outfit]9510081
            plew.writeInt(c != null ? c.Eyes : 0); // 眼睛[eye]9110011
            plew.writeInt(c != null ? 9510081 : 0); // 衣服[dress]8160351
        }
    }
}
