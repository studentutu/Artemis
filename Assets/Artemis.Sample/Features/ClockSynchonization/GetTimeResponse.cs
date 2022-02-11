using System;

namespace Artemis.Sample.Features.ClockSynchonization
{
    [Serializable]
    public class GetTimeResponse
    {
        public readonly int Tick;
        public readonly DateTime Time;
        public readonly DateTime TimeAtFirstTick;

        public GetTimeResponse(int tick, DateTime time, DateTime timeAtFirstTick)
        {
            Tick = tick;
            Time = time;
            TimeAtFirstTick = timeAtFirstTick;
        }
    }
}