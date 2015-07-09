using System;

namespace GhostServer.server
{
    public class Randomizer
    {
        private readonly static Random rand = new Random();

        public static int nextInt()
        {
            return rand.Next();
        }

        public static int nextInt(int arg0)
        {
            return rand.Next(arg0);
        }

        public static long nextLong()
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }
    }
}
