using Artemis.Sample.Player;
using Artemis.Threading;
using Artemis.UserInterface;

namespace Artemis.Sample.Client.Handlers
{
    public class ClientDisconnectionMessageHandler : IMessageHandler<ClientDisconnectionMessage>
    {
        public void Handle(Message<ClientDisconnectionMessage> message)
        {
            UnityMainThreadDispatcher.Dispatch(() => DespawnPlayer.Despawn(message.Payload.PlayerId));
        }
    }
}