using System.Linq;
using Artemis.Packets;
using Artemis.Extensions;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Artemis.ValueObjects
{
    internal class RetransmissionQueue
    {
        private readonly List<IPEndPoint> _list = new();
        private readonly Dictionary<IPEndPoint, List<(Message, CancellationTokenRegistration)>> _dictionary = new();

        internal IEnumerable<(IPEndPoint, IEnumerable<Message>)> Get()
        {
            return _list.Select(address => (address, _dictionary[address].Select(x => x.Item1)));
        }

        internal void Add(IPEndPoint recipient, Message message, CancellationTokenRegistration registration)
        {
            EnsureAddressInsertion(recipient);
            _dictionary[recipient].Add((message, registration));
        }

        internal void Remove(IPEndPoint recipient, int sequence)
        {
            var tuple = _dictionary[recipient].Single(t => t.Item1.Sequence == sequence);
            tuple.Item2.Dispose();
            _dictionary[recipient].Remove(tuple);
        }

        private void EnsureAddressInsertion(IPEndPoint recipient)
        {
            if (!_dictionary.ContainsKey(recipient))
            {
                _list.Add(recipient);
                _dictionary.Add(recipient, new List<(Message, CancellationTokenRegistration)>());
            }
        }
    }
}