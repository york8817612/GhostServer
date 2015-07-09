using System;
using System.Text;

namespace GhostServer.tools.data
{
    public class LittleEndianAccessor
    {
        private readonly ByteArrayByteStream bs;

        public LittleEndianAccessor(ByteArrayByteStream bs)
        {
            this.bs = bs;
        }

        public byte readByte()
        {
            return (byte)bs.readByte();
        }

        public short readShort()
        {
            int byte1 = bs.readByte();
            int byte2 = bs.readByte();
            return (short)((byte2 << 8) + byte1);
        }

        public int readInt()
        {
            int byte1 = bs.readByte();
            int byte2 = bs.readByte();
            int byte3 = bs.readByte();
            int byte4 = bs.readByte();
            return (byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1;
        }

        public int readLong()
        {
            int byte1 = bs.readByte();
            int byte2 = bs.readByte();
            int byte3 = bs.readByte();
            int byte4 = bs.readByte();
            int byte5 = bs.readByte();
            int byte6 = bs.readByte();
            int byte7 = bs.readByte();
            int byte8 = bs.readByte();
            return (byte8 << 56) + (byte7 << 48) + (byte6 << 40) + (byte5 << 32) + (byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1;
        }

        public string readAsciiString(int n)
        {
            try
            {
                byte[] ret = new byte[n];
                for (int x = 0; x < n; x++)
                {
                    ret[x] = readByte();
                }
                var nullIndex = Array.IndexOf(ret, (byte)0);
                nullIndex = (nullIndex == -1) ? ret.Length : nullIndex;
                return Encoding.GetEncoding("Big5").GetString(ret, 0, nullIndex);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
                return "";
            }
        }

        public string readGhostAsciiString()
        {
            return readAsciiString(readShort());
        }
    }
}
