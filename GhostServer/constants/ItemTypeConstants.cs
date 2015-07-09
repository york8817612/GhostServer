using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhostServer.constants
{
    public class ItemTypeConstants
    {
        public enum EquipType : byte
        {
            Weapon = 0xFF, // 武器[weapon]
            Mantle = 0xFE, // 披風[mantle]
            Hat = 0xFD, // 帽子[hat]
            Face2 = 0xFC, // 臉下[face2]
            Face = 0xFB, // 臉上[face]
            Outfit = 0xFA, // 服裝[outfit]
            Dress = 0xF9 // 衣服[dress]
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
