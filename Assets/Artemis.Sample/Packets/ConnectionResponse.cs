using System;
using Artemis.Sample.ValueObjects;

namespace Artemis.Sample
{
    [Serializable]
    public class ConnectionResponse
    {
        public readonly Guid PlayerId;
        public readonly string Nickname;
        public readonly Color Color;
        public readonly Float2 Position;

        public ConnectionResponse(Guid playerId, string nickname, Color color, Float2 position)
        {
            PlayerId = playerId;
            Nickname = nickname;
            Color = color;
            Position = position;
        }
    }
}