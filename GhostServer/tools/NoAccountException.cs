using System;

namespace GhostServer.tools
{
    public class NoAccountException : Exception
    {
        public override string Message
        {
            get
            {
                return "account does not";
            }
        }
    }
}
