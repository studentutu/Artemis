using Artemis.Sample;
using Artemis.UserInterface;
using UnityEngine;

public partial class Server
{
    private void HandleConnectionRequest(Request<ConnectionRequest> request)
    {
        request.Reply(new ConnectionResponse());
        _connections.Add(request.Sender);
        Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
    }

    private void HandleClientDisconnectionMessage(Message<ClientDisconnectionMessage> message)
    {
        _connections.Remove(message.Sender);
        Debug.Log($"<b>[S]</b> Client {message.Sender} has disconnected gracefully :)");
    }
}