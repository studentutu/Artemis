using System.Linq;
using Artemis.Packets;
using Artemis.Extensions;
using System.Collections.Generic;

namespace Artemis.ValueObjects
{
    internal class PendingAckMessageQueue
    {
        private readonly List<Address> _list = new();
        private readonly Dictionary<Address, List<Message>> _dictionary = new();

        internal IEnumerable<(Address, List<Message>)> Get()
        {
            return _list.Select(address => (address, _dictionary[address]));
        }

        internal void Add(Address recipient, Message message)
        {
            EnsureAddressInsertion(recipient);
            _dictionary[recipient].Add(message);
        }

        internal void Remove(Address recipient, int sequence)
        {
            _dictionary[recipient].Remove(msg => msg.Sequence == sequence);
        }

        private void EnsureAddressInsertion(Address recipient)
        {
            if (!_dictionary.ContainsKey(recipient))
            {
                _list.Add(recipient);
                _dictionary.Add(recipient, new List<Message>());
            }
        }
    }
}