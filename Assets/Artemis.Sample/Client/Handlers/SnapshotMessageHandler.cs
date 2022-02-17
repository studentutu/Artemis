using Artemis.Sample.Extensions;
using Artemis.Threading;
using Artemis.UserInterface;
using Artemis.Sample.Packets;
using Object = UnityEngine.Object;

namespace Artemis.Sample.Client.Handlers
{
    public class SnapshotMessageHandler : IMessageHandler<Snapshot>
    {
        public void Handle(Message<Snapshot> message)
        {
            UnityMainThreadDispatcher.Dispatch(() =>
            {
                var players = Object.FindObjectsOfType<BasePlayer>();

                foreach (var playerData in message.Payload.Players)
                {
                    if (players.TryFind(p => p.Id == playerData.Id, out var player))
                    {
                        player.OnSnapshotReceived(message.Payload.Tick, playerData);
                    }
                }
            });
        }
    }
}