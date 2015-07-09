using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GhostServer.server
{
    public sealed class Worlds : KeyedCollection<byte, World>
    {
        public Worlds() : base() { }

        protected override byte GetKeyForItem(World item)
        {
            return item.ID;
        }
    }
}
