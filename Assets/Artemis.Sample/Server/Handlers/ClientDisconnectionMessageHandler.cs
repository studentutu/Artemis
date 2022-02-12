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
        _server._players.Remove(_server._players.Find(p => p.Address == message.Sender));
        Debug.Log($"<b>[S]</b> Client {message.Sender} has disconnected gracefully :)");
    }
}