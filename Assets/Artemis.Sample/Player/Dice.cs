namespace Artemis.Sample.Player
{
    public static class Dice
    {
        private static readonly System.Random _random = new();

        public static float RollRange(float minInclusive, float maxExclusive)
        {
            var distance = maxExclusive - minInclusive;
            return (float) (minInclusive + distance * _random.NextDouble());
        }
    }
}