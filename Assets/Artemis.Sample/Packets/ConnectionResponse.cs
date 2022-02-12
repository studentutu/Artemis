using System;

namespace Artemis.Sample
{
    [Serializable]
    public class ConnectionResponse
    {
        public readonly Guid PlayerId;

        public ConnectionResponse(Guid playerId)
        {
            PlayerId = playerId;
        }
    }
}