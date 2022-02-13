using Artemis.Sample.Player;
using Artemis.Threading;
using Artemis.UserInterface;
using UnityEngine;

public class PlayerJoinedMessageHandler : IMessageHandler<PlayerJoinedMessage>
{
    public void Handle(Message<PlayerJoinedMessage> message)
    {
        UnityMainThreadDispatcher.Dispatch(() =>
        {
            InstantiatePlayer.Instantiate(
                message.Payload.Nickname,
                message.Payload.Color.ToUnityColor(),
                new Vector2(message.Payload.X, message.Payload.Y),
                isLocalPlayer: false);
        });
    }
}