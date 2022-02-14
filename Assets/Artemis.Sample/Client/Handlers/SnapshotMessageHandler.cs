using System;
using Artemis.Sample.Packets;
using Artemis.UserInterface;

namespace Artemis.Sample.Client.Handlers
{
    public class SnapshotMessageHandler : IMessageHandler<Snapshot>
    {
        public void Handle(Message<Snapshot> message)
        {
            throw new NotImplementedException();
        }
    }
}