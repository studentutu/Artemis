using System;
using System.Collections.Generic;
using Artemis.Sample;
using Artemis.Sample.Core;
using Artemis.Sample.Packets;
using Artemis.Sample.Player;
using Artemis.Threading;
using UnityEngine;

public class PredictedLocalPlayer : MonoBehaviour
{
    private readonly List<PlayerCommand> _unconfirmedCommands = new();
    
    private readonly Artemis.Sample.Generics.Memory<Timed<Vector2>> _predictionBuffer = new();

    public void OnCommandSent(PlayerCommand command)
    {
        _unconfirmedCommands.Add(command);

        UnityMainThreadDispatcher.Dispatch(() =>
        {
            transform.position = MovePlayer.Move(transform.position, command, Configuration.FixedDeltaTime);
            _predictionBuffer.Add(new Timed<Vector2>(transform.position, command.Tick), DateTime.Now.AddSeconds(2));
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
        _predictionBuffer.RemoveExpiredItems();
            
        foreach (var snapshot in _predictionBuffer)
        {
            DrawPrediction(snapshot);
        }
    }

    private void DrawPrediction(Timed<Vector2> prediction)
    {
        Gizmos.color = GUI.color = Color.red;
        Gizmos.DrawWireCube(prediction.Value, Vector2.one * 0.2f);
        var style = new GUIStyle("label") {fontStyle = FontStyle.Bold};
        UnityEditor.Handles.Label(prediction.Value, $"F{prediction.Tick}", style);
    }
}