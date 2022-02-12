using System.Linq;
using Artemis.Threading;
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
        UnityMainThreadDispatcher.Dispatch(() => InstantiatePlayer(message));
    }

    private void InstantiatePlayer(Message<PlayerJoinedMessage> message)
    {
        var viewPrefab = Resources.LoadAll<PlayerView>(string.Empty).Single();
        var view = Object.Instantiate(viewPrefab);
        view.Nickname.text = message.Payload.Nickname;
        view.Sprite.color = message.Payload.Color.ToUnityColor();
        view.transform.position = new Vector2(message.Payload.X, message.Payload.Y);
    }
}