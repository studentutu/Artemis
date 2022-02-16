using System.Collections.Generic;
using Artemis.Sample;
using UnityEngine;
using Artemis.Sample.Core;
using Artemis.Sample.Input;
using Artemis.Threading;
using Artemis.Sample.Packets;
using Artemis.Sample.Player;

public class LocalPlayer : BasePlayer
{
    private readonly List<PlayerCommand> _unconfirmedInputs = new();

    public void OnFixedUpdate(DapperClient client, int tick)
    {
        var command = new PlayerCommand(tick, DapperInput.GetMovementInput());
        client._client.SendUnreliableMessage(command, client.ServerAddress);
        _unconfirmedInputs.Add(command);

        UnityMainThreadDispatcher.Dispatch(() =>
        {
            transform.position = MovePlayer.Move(transform.position, command, Configuration.FixedDeltaTime);
        });
    }

    public override void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        UnityMainThreadDispatcher.Dispatch(() =>
        {
            _unconfirmedInputs.RemoveAll(ui => ui.Tick <= tick);
            transform.position = new Vector2(snapshot.Position.X, snapshot.Position.Y);

            foreach (var unconfirmedInput in _unconfirmedInputs)
            {
                transform.position = MovePlayer.Move(transform.position, unconfirmedInput, Configuration.FixedDeltaTime);
            }
        });
    }
}