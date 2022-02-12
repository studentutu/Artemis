using System;
using Artemis.ValueObjects;

namespace Artemis.Sample.Core
{
    public class PlayerData
    {
        public readonly Guid Id;
        public readonly Address Address;
        public readonly string Nickname;

        public PlayerData(Guid id, Address address, string nickname)
        {
            Id = id;
            Address = address;
            Nickname = nickname;
        }
    }
}