using System;
using Artemis.Sample.Core;

namespace Artemis.Sample.Packets
{
    [Serializable]
    public class Snapshot
    {
        public readonly int Tick;
        public readonly PlayerData[] Players;

        public Snapshot(int tick, PlayerData[] players)
        {
            Players = players;
        }
    }
}