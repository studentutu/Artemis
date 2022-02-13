using System;
using Artemis.Sample.Player;

namespace Artemis.Sample
{
    [Serializable]
    public class ConnectionResponse
    {
        public readonly Guid PlayerId;
        public readonly string Nickname;
        public readonly HSV Color;
        public readonly float X;
        public readonly float Y;

        public ConnectionResponse(Guid playerId, string nickname, HSV color, float x, float y)
        {
            PlayerId = playerId;
            Nickname = nickname;
            Color = color;
            X = x;
            Y = y;
        }
    }
}