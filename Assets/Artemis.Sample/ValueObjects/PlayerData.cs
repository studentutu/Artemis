using System;
using Artemis.Sample.ValueObjects;

namespace Artemis.Sample.Core
{
    [Serializable]
    public class PlayerData
    {
        public readonly Guid Id;
        public readonly string Nickname;
        public readonly Color Color;
        public Float2 Position;

        public PlayerData(Guid id, string nickname, Color color, Float2 position)
        {
            Id = id;
            Nickname = nickname;
            Color = color;
            Position = position;
        }
    }
}