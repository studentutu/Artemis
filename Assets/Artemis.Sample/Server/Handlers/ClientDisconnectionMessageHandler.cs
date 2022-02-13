using Artemis.Sample.Server.Core;
using Artemis.UserInterface;
using UnityEngine;

namespace Artemis.Sample.Server.Handlers
{
    public class ClientDisconnectionMessageHandler : IMessageHandler<ClientDisconnectionMessage>
    {
        private readonly DapperServer _dapperServer;

        public ClientDisconnectionMessageHandler(DapperServer dapperServer)
        {
            _dapperServer = dapperServer;
        }

        public void Handle(Message<ClientDisconnectionMessage> message)
        {
            Debug.Log($"<b>[S]</b> Client {message.Sender} has disconnected gracefully :)");
            _dapperServer._players.Remove(_dapperServer._players.Find(p => p.Address == message.Sender));

            foreach (var player in _dapperServer._players)
            {
                _dapperServer._client.SendReliableMessage(message.Payload, player.Address, _dapperServer.CancellationTokenOnDestroy);
            }
        }
    }
}