using System;
using Artemis.Sample.Player;
using Artemis.Sample.ValueObjects;
using Artemis.ValueObjects;

namespace Artemis.Sample.Core
{
    public class PlayerData
    {
        public readonly Guid Id;
        public readonly Address Address;
        public readonly string Nickname;
        public readonly Color Color;
        public Float2 Position;

        public PlayerData(Guid id, Address address, string nickname, Color color, Float2 position)
        {
            Id = id;
            Address = address;
            Nickname = nickname;
            Color = color;
            Position = position;
        }
    }
}