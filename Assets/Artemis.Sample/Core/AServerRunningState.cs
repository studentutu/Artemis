using Artemis.UserInterface;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Core
{
    public class AServerRunningState : AServerState
    {
        public override void OnStateEntered(Server server)
        {
            server._client.RegisterRequestHandler<ConnectionRequest>(r => HandleConnectionRequest(r, server));
            server._client.RegisterMessageHandler<ClientDisconnectionMessage>(m => HandleDisconnectionMessage(m, server));
        }

        public override void OnGUI(Server server)
        {
            GUILayoutUtilities.Button(nameof(Shutdown), () => Shutdown(server));
        }

        public override void OnDestroy(Server server)
        {
            Shutdown(server);
        }

        private void Shutdown(Server server)
        {
            foreach (var connection in server._connections)
            {
                server._client.SendUnreliableMessage(new ServerClosingMessage(), connection);
            }
            
            server._client.Dispose();
            server._client = null;
            server.Switch(server.Stopped);
        }

        private static void HandleConnectionRequest(Request<ConnectionRequest> request, Server server)
        {
            server._connections.Add(request.Sender);
            request.Reply(new ConnectionResponse());
            Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
        }

        private static void HandleDisconnectionMessage(Message<ClientDisconnectionMessage> message, Server server)
        {
            server._connections.Remove(message.Sender);
            Debug.Log($"<b>[S]</b> Client {message.Sender} has disconnected gracefully :)");
        }
    }
}