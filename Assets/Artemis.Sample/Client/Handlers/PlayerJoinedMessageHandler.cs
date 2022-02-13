using Artemis.Sample.Player;
using Artemis.Threading;
using Artemis.UserInterface;

public class PlayerJoinedMessageHandler : IMessageHandler<PlayerJoinedMessage>
{
    public void Handle(Message<PlayerJoinedMessage> message)
    {
        UnityMainThreadDispatcher.Dispatch(() =>
        {
            SpawnPlayer.Spawn(
                message.Payload.PlayerId,
                message.Payload.Nickname,
                message.Payload.Color,
                message.Payload.Position,
                isLocalPlayer: false);
        });
    }
}