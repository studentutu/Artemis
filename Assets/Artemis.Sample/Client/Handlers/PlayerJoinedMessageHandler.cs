using Artemis.UserInterface;
using UnityEngine;

public class PlayerJoinedMessageHandler : IMessageHandler<PlayerJoinedMessage>
{
    private readonly Client _client;

    public PlayerJoinedMessageHandler(Client client)
    {
        _client = client;
    }

    public void Handle(Message<PlayerJoinedMessage> message)
    {
        Debug.Log($"IsLocalPlayer: {message.Payload.IsLocalPlayer}");
    }
}