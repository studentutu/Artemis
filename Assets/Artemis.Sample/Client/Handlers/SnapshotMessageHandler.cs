using System;
using System.Linq;
using Artemis.Sample.Core;
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
                var remotePlayers = Object.FindObjectsOfType<RemotePlayer>();
                
                foreach (var playerData in message.Payload.Players)
                {
                    var remotePlayer = remotePlayers.FirstOrDefault(rp => rp.Id == playerData.Id);

                    if (remotePlayer != null)
                    {
                        remotePlayer.SnapshotBuffer.Add(new Timed<PlayerData>(playerData, message.Payload.Tick), DateTime.Now.AddSeconds(2));
                    }
                }
            });
        }
    }
}