using UnityEngine;
using Artemis.Sample.Core;
using Artemis.Threading;
using Artemis.Sample.Packets;

public class LocalPlayer : BasePlayer
{
    public void OnFixedUpdate(DapperClient client, int tick)
    {
        var command = GetCommandForTick(tick);
        client._client.SendUnreliableMessage(command, client.ServerAddress);
    }

    private static PlayerCommand GetCommandForTick(int tick)
    {
        var horizontal = Keyboard.GetAxis(Keyboard.Key.D, Keyboard.Key.A);
        var vertical = Keyboard.GetAxis(Keyboard.Key.W, Keyboard.Key.S);
        return new PlayerCommand(tick, horizontal, vertical);
    }

    public override void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        UnityMainThreadDispatcher.Dispatch(() =>
        {
            transform.position = new Vector2(snapshot.Position.X, snapshot.Position.Y);
        });
    }
}