using System.Collections.Generic;
using GhostServer.client;
using GhostServer.client.Characters;
using GhostServer.database;
using GhostServer.net;
using GhostServer.net.Packet;
using GhostServer.tools;
using GhostServer.tools.data;
using System;
using GhostServer.constants;

namespace GhostServer.net.handling
{
    public class Chars
    {
        public static void MyChar_Info_Req(LittleEndianAccessor lea, GhostClient gc)
        {
            lea.readInt();
            lea.readInt();
            string[] data = lea.readAsciiString(0x100).Split(new string[] { " 0 " }, StringSplitOptions.None);
            string username = data[1];
            string password = data[2];

            gc.Account = new Account(gc);
            try
            {
                gc.Account.Load(username);

                if (password != gc.Account.Password)
                {
                    // -10
                }
                else if (gc.Account.LoggedIn > 0)
                {
                    // -2
                }
                else
                {
                    gc.Account.Characters = new List<Character>();
                    foreach (dynamic datum in new Datums("Characters").PopulateWith("id", "accountId = '{0}' && worldId = '{1}'", gc.Account.ID, gc.World.ID))
                    {
                        Character character = new Character(datum.id, gc);
                        character.Load(false);
                        gc.Account.Characters.Add(character);
                    }
                    gc.SendPacket(net.Packet.Chars.MyChar_Info_Ack(gc, gc.Account.Characters));
                }
            }
            catch (Exception ee)
            {
                if (ee.Message.Equals("account does not"))
                {
                    // -1
                }
                else
                {
                    Log.Error(ee.ToString());
                }
            }
        }

        public static void Create_MyChar_Req(LittleEndianAccessor lea, GhostClient gc)
        {
            lea.readInt();
            lea.readInt();
            string name = lea.readAsciiString(20);
            int gender = lea.readByte();
            int unk1 = lea.readByte();
            int unk2 = lea.readByte();
            int unk3 = lea.readByte();
            int eyes = lea.readInt();
            int hair = lea.readInt();
            int weapon = lea.readInt();
            int armor = lea.readInt();
            
            Character chr = new Character();

            chr.AccountID = gc.Account.ID;
            chr.WorldID = gc.World.ID;
            chr.Name = name;
            chr.Level = 1;
            chr.Class = 0;
            chr.ClassLV = 0xFF;
            chr.Gender = (byte)gender;
            chr.Eyes = eyes;
            chr.Hair = hair;
            chr.Str = 3;
            chr.Dex = 3;
            chr.Vit = 3;
            chr.Int = 3;
            chr.Hp = 31;
            chr.MaxHp = 31;
            chr.Sp = 15;
            chr.MaxSp = 15;

            chr.Items.Add(new Item(weapon, (byte) ItemTypeConstants.EquipType.Weapon));
            chr.Items.Add(new Item(armor, (byte)ItemTypeConstants.EquipType.Dress));

            chr.Save();

            int pos;
            if ((gc.Account.Characters.Count + 1) <= 4)
            {
                gc.Account.Characters.Add(chr);
                pos = (gc.Account.Characters.Count << 8) + 1;
            }
            else if (Database.Exists("Characters", "name = '{0}'", name))
            {
                pos = -1;
            }
            else if ((gc.Account.Characters.Count + 1) > 4)
            {
                pos = -2;
            }
            else
            {
                pos = 0;
            }
            gc.SendPacket(net.Packet.Chars.Create_MyChar_Ack(pos));
        }

        public static void Check_SameName_Req(LittleEndianAccessor lea, GhostClient gc)
        {
            lea.readInt();
            lea.readInt();
            string name = lea.readAsciiString(20);
            gc.SendPacket(net.Packet.Chars.Check_SameName_Ack((Database.Exists("Characters", "name = '{0}'", name) ? 0 : 1)));
        }

        public static void Delete_MyChar_Req(LittleEndianAccessor lea, GhostClient gc)
        {
            lea.readInt();
            lea.readInt();
            int del_id = lea.readInt();

            gc.Account.Characters[del_id].Delete();

            gc.SendPacket(net.Packet.Chars.Delete_MyChar_Ack(del_id + 1));
        }
    }
}
