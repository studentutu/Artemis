using System;

namespace Artemis.Sample.Packets
{
    [Serializable]
    public class PlayerCommand
    {
        public readonly int Tick;
        public readonly int Horizontal;
        public readonly int Vertical;
        [NonSerialized] public DateTime EnqueuedAt;

        public PlayerCommand(int tick, int horizontal, int vertical)
        {
            Tick = tick;
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}