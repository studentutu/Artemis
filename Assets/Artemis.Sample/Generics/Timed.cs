public readonly struct Timed<T>
{
    public readonly T Value;
    public readonly int Tick;

    public Timed(T value, int tick)
    {
        Value = value;
        Tick = tick;
    }
}