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
                message.Payload.PlayerData.Id,
                message.Payload.PlayerData.Nickname,
                message.Payload.PlayerData.Color,
                message.Payload.PlayerData.Position,
                isLocalPlayer: false);
        });
    }
}