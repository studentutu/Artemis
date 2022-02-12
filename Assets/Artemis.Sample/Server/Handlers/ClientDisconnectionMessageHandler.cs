using UnityEngine;
using Artemis.UserInterface;

public class ClientDisconnectionMessageHandler : IMessageHandler<ClientDisconnectionMessage>
{
    private readonly Server _server;

    public ClientDisconnectionMessageHandler(Server server)
    {
        _server = server;
    }
    
    public void Handle(Message<ClientDisconnectionMessage> message)
    {
        _server._connections.Remove(message.Sender);
        Debug.Log($"<b>[S]</b> Client {message.Sender} has disconnected gracefully :)");
    }
}