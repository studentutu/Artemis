using System;

namespace Artemis.Sample
{
    [Serializable]
    public readonly struct Vehicle
    {
        public readonly string Brand;

        public Vehicle(string brand)
        {
            Brand = brand;
        }
    }
}