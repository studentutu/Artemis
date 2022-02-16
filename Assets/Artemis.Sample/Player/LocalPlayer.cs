using UnityEngine;
using Artemis.Sample.Core;
using Artemis.Sample.Input;
using Artemis.Threading;
using Artemis.Sample.Packets;

public class LocalPlayer : BasePlayer
{
    public void OnFixedUpdate(DapperClient client, int tick)
    {
        var command = new PlayerCommand(tick, DapperInput.GetMovementInput());
        client._client.SendUnreliableMessage(command, client.ServerAddress);
    }

    public override void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        UnityMainThreadDispatcher.Dispatch(() =>
        {
            transform.position = new Vector2(snapshot.Position.X, snapshot.Position.Y);
        });
    }
}