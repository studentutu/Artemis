using System.Linq;
using Artemis.Packets;
using Artemis.Extensions;
using System.Collections.Generic;
using System.Net;

namespace Artemis.ValueObjects
{
    internal class RetransmissionQueue
    {
        private readonly List<IPEndPoint> _list = new();
        private readonly Dictionary<IPEndPoint, List<Message>> _dictionary = new();

        internal IEnumerable<(IPEndPoint, List<Message>)> Get()
        {
            return _list.Select(address => (address, _dictionary[address]));
        }

        internal void Add(IPEndPoint recipient, Message message)
        {
            EnsureAddressInsertion(recipient);
            _dictionary[recipient].Add(message);
        }

        internal void Remove(IPEndPoint recipient, int sequence)
        {
            _dictionary[recipient].Remove(msg => msg.Sequence == sequence);
        }

        private void EnsureAddressInsertion(IPEndPoint recipient)
        {
            if (!_dictionary.ContainsKey(recipient))
            {
                _list.Add(recipient);
                _dictionary.Add(recipient, new List<Message>());
            }
        }
    }
}