using System;
using Artemis.Sample;

public class GameClock
{
    private TimeSpan _span;
    private readonly Action _tick;

    public GameClock(Action tick)
    {
        _tick = tick;
    }

    private void Tick(TimeSpan deltaTime)
    {
        _span = _span.Add(deltaTime);
        
        while (_span > Configuration.TickInterval)
        {
            _tick.Invoke();
            _span = _span.Subtract(Configuration.TickInterval);
        }
    }
}