using System.Collections.Generic;
using Artemis.Sample;
using Artemis.Sample.Core;
using Artemis.Sample.Packets;
using Artemis.Sample.Player;
using Artemis.Threading;
using UnityEngine;

public class PredictedLocalPlayer : MonoBehaviour
{
    private int _lastPredictedTick = -1;
    private readonly List<PlayerCommand> _unconfirmedCommands = new();

    public void OnCommandSent(PlayerCommand command)
    {
        _lastPredictedTick = command.Tick;
        _unconfirmedCommands.Add(command);

        UnityMainThreadDispatcher.Dispatch(() =>
        {
            transform.position = MovePlayer.Move(transform.position, command, Configuration.FixedDeltaTime);
        });
    }

    public void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        UnityMainThreadDispatcher.Dispatch(() =>
        {
            _unconfirmedCommands.RemoveAll(unconfirmedCommand => tick >= unconfirmedCommand.Tick);
            transform.position = new Vector2(snapshot.Position.X, snapshot.Position.Y);

            foreach (var unconfirmedCommand in _unconfirmedCommands)
            {
                transform.position = MovePlayer.Move(transform.position, unconfirmedCommand, Configuration.FixedDeltaTime);
            }
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
        UnityEditor.Handles.Label(transform.position, $"F{_lastPredictedTick}");
    }
}