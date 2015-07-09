using GhostServer.database;
using GhostServer.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhostServer.client.Characters
{
    public class Character
    {
        public int ID { get; private set; }
        public int AccountID { get; set; }
        public byte WorldID { get; set; }
        public GhostClient Client { get; private set; }

        public string Name { get; set; }
        public string Title { get; set; }
        public byte Gender { get; set; }
        public int Hair { get; set; }
        public int Eyes { get; set; }
        public byte Level { get; set; }
        public byte Class { get; set; }
        public byte ClassLV { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Sp { get; set; }
        public int MaxSp { get; set; }
        public int Exp { get; set; }
        public int Rank { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Vit { get; set; }
        public short Int { get; set; }

        public CharacterItems Items { get; private set; }

        private bool Assigned { get; set; }

        public Character(int id = 0, GhostClient gc = null)
        {
            this.ID = id;
            this.Client = gc;

            this.Items = new CharacterItems(this);
        }

        public void Load(bool IsFullLoad = true)
        {
            dynamic datum = new Datum("Characters");

            datum.Populate("id = '{0}'", this.ID);

            this.ID = datum.id;
            this.Assigned = true;

            this.AccountID = datum.accountId;
            this.WorldID = (byte)datum.worldId;
            this.Name = datum.name;
            this.Title = datum.title;
            this.Gender = (byte)datum.gender;
            this.Hair = datum.hair;
            this.Eyes = datum.eyes;
            this.Level = (byte)datum.level;
            this.Class = (byte)datum.classId;
            this.ClassLV = (byte)datum.classLv;
            this.Hp = datum.hp;
            this.MaxHp = datum.maxHp;
            this.Sp = datum.sp;
            this.MaxSp = datum.maxSp;
            this.Exp = datum.exp;
            this.Rank = datum.rank;
            this.Str = (short)datum.c_str;
            this.Dex = (short)datum.c_dex;
            this.Vit = (short)datum.c_vit;
            this.Int = (short)datum.c_int;

            this.Items.Load();
        }

        public void Save()
        {
            dynamic datum = new Datum("Characters");

            datum.accountId = this.AccountID;
            datum.worldId = this.WorldID;
            datum.name = this.Name;
            datum.title = this.Title;
            datum.gender = this.Gender;
            datum.hair = this.Hair;
            datum.eyes = this.Eyes;
            datum.level = this.Level;
            datum.classId = this.Class;
            datum.classLv = this.ClassLV;
            datum.hp = this.Hp;
            datum.maxHp = this.MaxHp;
            datum.sp = this.Sp;
            datum.maxSp = this.MaxSp;
            datum.exp = this.Exp;
            datum.rank = this.Rank;
            datum.c_str = this.Str;
            datum.c_dex = this.Dex;
            datum.c_vit = this.Vit;
            datum.c_int = this.Int;

            if (this.Assigned)
            {
                datum.Update("id = '{0}'", this.ID);
            }
            else
            {
                datum.Insert();

                this.ID = Database.Fetch("Characters", "id", "name = '{0}'", this.Name);

                this.Assigned = true;
            }

            this.Items.Save();

            Log.Inform("角色'{0}'的資料已儲存至資料庫。", this.Name);
        }

        public void Delete()
        {
            this.Items.Delete();
            
            Database.Delete("Characters", "id = '{0}'", this.ID);

            this.Assigned = false;
        }
    }
}
