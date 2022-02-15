using System.Linq;
using Artemis.Sample.Packets;
using Artemis.Threading;
using Artemis.UserInterface;
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
                    var player = players.Single(rp => rp.Id == playerData.Id);
                    player.OnSnapshotReceived(message.Payload.Tick, playerData);
                }
            });
        }
    }
}