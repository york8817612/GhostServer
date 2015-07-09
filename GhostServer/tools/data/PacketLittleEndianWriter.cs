using System.IO;
using System.Text;

namespace GhostServer.tools.data
{
    public class PacketLittleEndianWriter
    {
        private MemoryStream ms;
        private string codepage = "Big5";

        public PacketLittleEndianWriter()
        {
            this.ms = new MemoryStream(32);
        }

        public PacketLittleEndianWriter(int size)
        {
            this.ms = new MemoryStream(size);
        }

        public byte[] getPacket()
        {
            return ms.ToArray();
        }

        public void write(byte value)
        {
            ms.WriteByte(value);
        }

        public void write(byte[] value)
        {
            for (int x = 0; x < value.Length; x++)
            {
                ms.WriteByte(value[x]);
            }
        }

        public void writeBoolean(bool value)
        {
            ms.WriteByte((byte)(value ? 1 : 0));
        }

        public void writeShort(short value)
        {
            ms.WriteByte((byte)(value & 0xFF));
            ms.WriteByte((byte)((value >> 8) & 0xFF));
        }

        public void writeInt(int value)
        {
            ms.WriteByte((byte)(value & 0xFF));
            ms.WriteByte((byte)((value >> 8) & 0xFF));
            ms.WriteByte((byte)((value >> 16) & 0xFF));
            ms.WriteByte((byte)((value >> 24) & 0xFF));
        }

        public void writeLong(long value)
        {
            ms.WriteByte((byte)(value & 0xFF));
            ms.WriteByte((byte)((value >> 8) & 0xFF));
            ms.WriteByte((byte)((value >> 16) & 0xFF));
            ms.WriteByte((byte)((value >> 24) & 0xFF));
            ms.WriteByte((byte)((value >> 32) & 0xFF));
            ms.WriteByte((byte)((value >> 40) & 0xFF));
            ms.WriteByte((byte)((value >> 48) & 0xFF));
            ms.WriteByte((byte)((value >> 56) & 0xFF));
        }

        public void writeZeroBytes(int count)
        {
            for (int x = 0; x < count; x++)
            {
                ms.WriteByte(0);
            }
        }

        public void writeAsciiString(string s)
        {
            write(Encoding.GetEncoding(codepage).GetBytes(s));
        }

        public void writeAsciiString(string s, int max)
        {
            if (Encoding.GetEncoding(codepage).GetBytes(s).Length > max)
            {
                s = s.Substring(0, max);
            }
            write(Encoding.GetEncoding(codepage).GetBytes(s));
            for (int i = Encoding.GetEncoding(codepage).GetBytes(s).Length; i < max; i++)
            {
                ms.WriteByte(0);
            }
        }

        public void writeGhostAsciiString(string s)
        {
            writeShort((short)Encoding.GetEncoding(codepage).GetBytes(s).Length);
            writeAsciiString(s);
        }
    }
}
