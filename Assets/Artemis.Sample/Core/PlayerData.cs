using System;
using Artemis.Sample.Player;
using Artemis.ValueObjects;

namespace Artemis.Sample.Core
{
    public class PlayerData
    {
        public readonly Guid Id;
        public readonly Address Address;
        public readonly string Nickname;
        public readonly HSV Color;
        public float X;
        public float Y;

        public PlayerData(Guid id, Address address, string nickname, HSV color, float x, float y)
        {
            Id = id;
            Address = address;
            Nickname = nickname;
            Color = color;
            X = x;
            Y = y;
        }
    }
}