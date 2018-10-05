using GhostServer.client.Characters;
using GhostServer.database;
using GhostServer.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace GhostServer.client
{
    public sealed class Account
    {
        public GhostClient Client { get; private set; }

        public int ID { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Pin { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Creation { get; set; }
        public int Gender { get; set; }
        public int LoggedIn { get; set; }
        public int Banned { get; set; }
        public int Master { get; set; }
        public int CashPoint { get; set; }

        public List<Character> Characters { get; set; }

        private bool Assigned { get; set; }

        public Account(GhostClient gc)
        {
            this.Client = gc;
        }

        public void Load(string username)
        {
            dynamic datum = new Datum("Accounts");

            try
            {
                datum.Populate("userName = '{0}'", username);
            }
            catch (RowNotInTableException)
            {
                throw new NoAccountException();
            }

            this.ID = datum.id;
            this.Assigned = true;

            this.Username = datum.userName;
            this.Password = datum.password;
            this.Salt = datum.salt;
            this.Pin = datum.pin;
            //this.Birthday = datum.birthday;
            //this.Creation = datum.creation;
            this.Gender = datum.gender;
            this.LoggedIn = datum.isLoggedIn;
            this.Banned = datum.isBanned;
            this.Master = datum.isMaster;
            this.CashPoint = datum.cashPoint;
        }

        public void Save()
        {
            dynamic datum = new Datum("Accounts");

            datum.userName = this.Username;
            datum.password = this.Password;
            datum.salt = this.Salt;
            datum.pin = this.Pin;
            //datum.birthday = this.Birthday;
            //datum.creation = this.Creation;
            datum.gender = this.Gender;
            datum.isLoggedIn = this.LoggedIn;
            datum.isBanned = this.Banned;
            datum.isMaster = this.Master;
            datum.cashPoint = this.CashPoint;
            
            if (this.Assigned)
            {
                datum.Update("id = '{0}'", this.ID);
            }
            else
            {
                datum.Insert();

                this.ID = Database.Fetch("Accounts", "id", "userName = '{0}'", this.Username);
                this.Assigned = true;
            }

            Log.Inform("Username '{0}' 已經儲存到資料庫。", this.Username);
        }

    }
}
