using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GhostServer.client.Characters;
using GhostServer.database;

namespace GhostServer.client
{
    public class Item
    {
        public CharacterItems Parent { get; set; }

        public int ID { get; private set; }
        public int ItemID { get; private set; }
        private short maxPerStack;
        private short quantity;
        public byte Slot { get; set; }

        public bool Assigned { get; set; }

        public Character Character
        {
            get
            {
                try
                {
                    return this.Parent.Parent;
                }
                catch
                {
                    return null;
                }
            }
        }

        public short MaxPerStack
        {
            get
            {
                return maxPerStack;
            }
            set
            {
                maxPerStack = value;
            }
        }

        public short Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (value > this.MaxPerStack)
                {
                    throw new ArgumentException("Quantity too high.");
                }
                else
                {
                    quantity = value;
                }
            }
        }

        public Item(int itemID, short quantity = 1, bool equipped = false)
        {
            this.ItemID = itemID;
            this.MaxPerStack = this.MaxPerStack;
            this.Quantity = quantity;
        }

        public Item(dynamic datum)
        {
            this.ID = datum.id;
            this.Assigned = true;

            this.ItemID = datum.itemId;
            this.MaxPerStack = 1;
            this.Quantity = datum.quantity;
            this.Slot = datum.slot;
        }

        public void Save()
        {
            dynamic datum = new Datum("Items");

            datum.cid = this.Character.ID;
            datum.itemId = this.ItemID;
            datum.quantity = this.Quantity;
            datum.slot = this.Slot;

            if (this.Assigned)
            {
                datum.Update("id = '{0}'", this.ID);
            }
            else
            {
                datum.Insert();

                this.ID = Database.Fetch("Items", "id", "cid = '{0}' && itemId = '{1}' && slot = '{2}'", this.Character.ID, this.ItemID, this.Slot);

                this.Assigned = true;
            }
        }

        public void Delete()
        {
            Database.Delete("Items", "id = '{0}'", this.ID);

            this.Assigned = false;
        }

        public enum ItemType : byte
        {
            Equip1,
            Equip2,
            Use,
            etc,
            Pet
        }
    }
}
