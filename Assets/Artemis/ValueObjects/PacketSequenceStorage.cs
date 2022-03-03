using System.Collections.Generic;
using System.Net;

namespace Artemis.ValueObjects
{
    internal class PacketSequenceStorage
    {
        private readonly Dictionary<(IPEndPoint, DeliveryMethod), int> _registry = new();

        internal int Get(IPEndPoint address, DeliveryMethod deliveryMethod, int defaultValue)
        {
            var key = (address, deliveryMethod);
            if (!_registry.ContainsKey(key))
            {
                _registry.Add(key, defaultValue);
            }

            return _registry[key];
        }

        internal void Set(IPEndPoint address, DeliveryMethod deliveryMethod, int value)
        {
            _registry[(address, deliveryMethod)] = value;
        }
    }
}