using System;

namespace Artemis.Sample
{
    [Serializable]
    public class ConnectionRequest
    {
        public readonly string Nickname;

        public ConnectionRequest(string nickname)
        {
            Nickname = nickname;
        }
    }
}