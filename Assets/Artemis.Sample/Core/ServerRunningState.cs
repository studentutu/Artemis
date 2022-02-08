using Artemis.UserInterface;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Core
{
    public class ServerRunningState : IServerState
    {
        void IServerState.OnStateEntered(Server server)
        {
            server._client.RegisterRequestHandler<ConnectionRequest>(r => HandleConnectionRequest(r, server));
            server._client.RegisterMessageHandler<ClientDisconnectionMessage>(m => HandleDisconnectionMessage(m, server));
        }

        void IServerState.OnGUI(Server server)
        {
            GUILayoutUtilities.Button(nameof(Shutdown), () => Shutdown(server));
        }

        void IServerState.OnDestroy(Server server)
        {
            Shutdown(server);
        }

        private void Shutdown(Server server)
        {
            foreach (var connection in server._connections)
            {
                server._client.SendMessage(new ServerClosingMessage(), connection, DeliveryMethod.Unreliable);
            }
            
            server._client.Dispose();
            server._client = null;
            server.Switch(server.Stopped);
        }

        private void HandleConnectionRequest(Request<ConnectionRequest> request, Server server)
        {
            server._connections.Add(request.Sender);
            request.Reply(new ConnectionResponse());
            Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
        }

        private void HandleDisconnectionMessage(Message<ClientDisconnectionMessage> message, Server server)
        {
            server._connections.Remove(message.Sender);
            Debug.Log($"<b>[S]</b> Client {message.Sender} has disconnected gracefully :)");
        }
    }
}