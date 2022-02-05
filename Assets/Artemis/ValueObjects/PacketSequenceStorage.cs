using System.Collections.Generic;

namespace Artemis.ValueObjects
{
    internal class PacketSequenceStorage
    {
        private readonly Dictionary<(Address, DeliveryMethod), int> _registry = new();

        internal int Get(Address address, DeliveryMethod deliveryMethod, int defaultValue)
        {
            var key = (address, deliveryMethod);
            if (!_registry.ContainsKey(key))
            {
                _registry.Add(key, defaultValue);
            }

            return _registry[key];
        }

        internal void Set(Address address, DeliveryMethod deliveryMethod, int value)
        {
            _registry[(address, deliveryMethod)] = value;
        }
    }
}