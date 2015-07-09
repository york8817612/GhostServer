
namespace GhostServer.tools.data
{
    public class ByteArrayByteStream
    {
        private int pos = 0;
        private long bytesRead = 0;
        private readonly byte[] arr;

        public ByteArrayByteStream(byte[] arr)
        {
            this.arr = arr;
        }

        public long getPosition()
        {
            return pos;
        }

        public void seek(long offset)
        {
            pos = (int)offset;
        }

        public long getBytesRead()
        {
            return bytesRead;
        }

        public int readByte()
        {
            bytesRead++;
            return (arr[pos++]) & 0xFF;
        }
    }
}
