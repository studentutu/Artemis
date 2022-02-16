using System;
using Artemis.Sample.ValueObjects;

namespace Artemis.Sample.Packets
{
    [Serializable]
    public class PlayerCommand
    {
        public readonly int Tick;
        public readonly Int2 Movement;
        [NonSerialized] public DateTime EnqueuedAt;

        public PlayerCommand(int tick, Int2 movement)
        {
            Tick = tick;
            Movement = movement;
        }
    }
}